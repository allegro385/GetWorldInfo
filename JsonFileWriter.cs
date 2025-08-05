using GetWorldInfo.Dto;

namespace GetWorldInfo
{
    public class JsonFileWriter
    {
        public static async Task WriteWorldPortalJsonAsync(List<CategoryDto> categoriesWithWorlds, string filePath)
        {
            // 出力用辞書組立
            var output = new Dictionary<string, object>
            {
                { "ReverseCategorys", false },
                { "ShowPrivateWorld", true },
                { "Categorys", new List<object>() },
                { "Roles", new List<object>() }
            };

            // Roles設定
            var roles = new List<object> {
            new Dictionary<string, object>
            {
                { "RoleName", "マスター" },
                { "DisplayNames", new List<string> { "あれぐろ" } }
            }
            };
            output["Roles"] = roles;

            foreach (var category in categoriesWithWorlds)
            {
                //カテゴリの頭3桁はソート用プレフィックスのため除外したcategoryを取得
                string categoryName = category.Category.Length > 3 ? category.Category.Substring(3) : category.Category;

                var worldList = new List<object>();
                foreach (var world in category.Worlds)
                {
                    //ワールド説明文を更新
                    var description = world.Description;

                    //タグ追記
                    if (world.Tags != null && world.Tags.Count > 0)
                    {
                        // 追加したいプレフィックス
                        string[] addPrefixes = { "author_tag_" };

                        var filteredTags = world.Tags
                            .Where(t => addPrefixes.Any(prefix => t.StartsWith(prefix)))
                            .Select(t => $"#{t.Replace("author_tag_", "")}");

                        if (filteredTags.Any())
                        {
                            if (!string.IsNullOrWhiteSpace(description.Trim()))
                            {
                                description += $"\n";
                            }
                            var tagString = string.Join(" ", filteredTags);
                            description += $"タグ：{tagString}";
                        }
                    }

                    //登録日時、更新日付追記
                    if (!string.IsNullOrWhiteSpace(description.Trim()))
                    {
                        description += $"\n";
                    }
                    description += $"更新情報：公開日 {world.CreatedAt.ToString("yyyy/MM/dd")} - 更新日 {world.UpdatedAt.ToString("yyyy/MM/dd")}";

                    //個人メモ追記
                    if (!string.IsNullOrWhiteSpace(world.Memo?.Trim()))
                    {
                        if (!string.IsNullOrWhiteSpace(description.Trim()))
                        {
                            description += $"\n";
                        }
                        description += $"個人メモ：{world.Memo.Trim()}";
                    }

                    var worldDict = new Dictionary<string, object>
                    {
                        { "ID", world.Id },
                        { "Name", world.Name },
                        { "RecommendedCapacity", world.RecommendedCapacity },
                        { "Capacity", world.Capacity },
                        { "Description", description },
                        { "Platform", new Dictionary<string, bool>
                            {
                                { "PC", world.Platform.PC },
                                { "Android", world.Platform.Android },
                                { "iOS", world.Platform.iOS }
                            }
                        },
                        { "ReleaseStatus", world.ReleaseStatus.ToString() }
                    };

                    //ワールドにRoleを付与する
                    if (world.ReleaseStatus.ToString().ToLower() == "private")
                    {
                        worldDict["PermittedRoles"] = new List<string> { "マスター" };
                    }
                    worldList.Add(worldDict);
                }

                ((List<object>)output["Categorys"]).Add(new Dictionary<string, object>
                {
                { "Category", categoryName },
                { "Worlds", worldList }
                });
            }

            var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
            string resultJson = System.Text.Json.JsonSerializer.Serialize(output, options);
            await File.WriteAllTextAsync(filePath, resultJson);
        }
    }
}
