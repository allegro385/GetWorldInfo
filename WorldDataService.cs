namespace GetWorldInfo
{
    // サービス（Applicationロジック）
    public class WorldDataService
    {
        private readonly WorldApiClient apiClient;

        public WorldDataService(WorldApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<List<CategoryDto>> GetCategoriesWithWorldsAsync(List<TsvWorldInfoDto> tsvWorlds)
        {
            // カテゴリごとにワールドをまとめる辞書
            var categoryDict = new Dictionary<string, List<WorldDto>>();
            foreach (var tsvWorld in tsvWorlds)
            {
                WorldDto world;
                try
                {
                    Console.WriteLine($"[取得中] {tsvWorld.WorldId}");
                    // APIからワールド情報取得
                    world = await apiClient.GetWorldAsync(tsvWorld.WorldId);

                    // Platform補正
                    if (tsvWorld.QuestSupported) world.Platform.Android = true;
                    else world.Platform.Android = false;

                    if (tsvWorld.IosSupported) world.Platform.iOS = true;
                    else world.Platform.iOS = false;

                    // タグ追記
                    if (world.Tags != null && world.Tags.Count > 0)
                    {
                        // 除外したいプレフィックス
                        string[] ignorePrefixes = {"system_" };

                        var filteredTags = world.Tags
                            .Where(t => !ignorePrefixes.Any(prefix => t.StartsWith(prefix)))                           
                            .Select(t => $"#{t.Replace("author_tag_","")}");

                        if (filteredTags.Any())
                        {
                            var tagString = string.Join(" ", filteredTags);
                            world.Description += $"\nタグ：{tagString}";
                        }
                    }

                    //登録日付、更新日付追記
                    world.Description += $"\n更新情報：公開日 {world.CreatedAt.ToString("yyyy/MM/dd")} - 更新日 {world.UpdatedAt.ToString("yyyy/MM/dd")}";

                    // 説明文追記
                    if (!string.IsNullOrWhiteSpace(tsvWorld.DescriptionNote.Trim()))
                    {
                        world.Description += $"\n個人メモ：{tsvWorld.DescriptionNote.Trim()}";
                    }

                    // カテゴリに追加
                    if (!categoryDict.ContainsKey(tsvWorld.Category))
                        categoryDict[tsvWorld.Category] = new List<WorldDto>();

                    categoryDict[tsvWorld.Category].Add(world);

                }
                catch (Exception)
                {
                    Console.WriteLine($"Error fetching world data for ID: {tsvWorld.WorldId}");
                }
            }

            // CategoryDtoへまとめる
            var result = new List<CategoryDto>();
            foreach (var pair in categoryDict)
            {
                result.Add(new CategoryDto
                {
                    Category = pair.Key,
                    Worlds = pair.Value
                });
            }
            return result;
        }
    }

}
