namespace GetWorldInfo
{
    public class JsonFileWriter
    {
        public static async Task WriteWorldPortalJsonAsync(
            List<CategoryDto> categoriesWithWorlds,
            string filePath)
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

                var worldList = new List<object>();
                foreach (var world in category.Worlds)
                {
                    var worldDict = new Dictionary<string, object>
                    {
                        { "ID", world.Id },
                        { "Name", world.Name },
                        { "RecommendedCapacity", world.RecommendedCapacity },
                        { "Capacity", world.Capacity },
                        { "Description", world.Description },
                        { "Platform", new Dictionary<string, bool>
                            {
                                { "PC", world.Platform.PC },
                                { "Android", world.Platform.Android },
                                { "iOS", world.Platform.iOS }
                            }
                        },
                        { "ReleaseStatus", world.ReleaseStatus.ToString() }
                    };
                    if (world.ReleaseStatus.ToString().ToLower() == "private")
                    {
                        worldDict["PermittedRoles"] = new List<string> { "マスター" };
                    }
                    worldList.Add(worldDict);
                }

                ((List<object>)output["Categorys"]).Add(new Dictionary<string, object>
                {
                { "Category", category.Category },
                { "Worlds", worldList }
                });
            }

            var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
            string resultJson = System.Text.Json.JsonSerializer.Serialize(output, options);
            await File.WriteAllTextAsync(filePath, resultJson);
        }
    }
}
