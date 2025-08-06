using GetWorldInfo.Dto;

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
        /// <summary>
        /// APIからワールド情報を取得し、カテゴリごとにまとめる
        /// tsvWorldsの情報を更新し、取得結果を返す
        /// </summary>
        /// <param name="tsvWorlds"></param>
        /// <returns></returns>

        public async Task<List<CategoryDto>> GetCategoriesWithWorldsAsync(List<WorldDto> tsvWorlds)
        {
            // カテゴリごとにワールドをまとめる辞書
            var categoryDict = new Dictionary<string, List<WorldDto>>();
            foreach (var tsvWorld in tsvWorlds)
            {
                WorldDto world;
                try
                {
                    //Console.WriteLine($"[取得中] {tsvWorld.Id}");

                    // APIからワールド情報取得
                    world = await apiClient.GetWorldAsync(tsvWorld.Id);
                                     
                    //APIから取得できない情報を入力データの情報から設定する。
                    world.Category = tsvWorld.Category;
                    world.Platform.PC = tsvWorld.Platform.PC;
                    world.Platform.Android = tsvWorld.Platform.Android;
                    world.Platform.iOS = tsvWorld.Platform.iOS;
                    world.WorldURL = tsvWorld.WorldURL;
                    world.Memo = tsvWorld.Memo;

                    // TSV用の情報を更新
                    tsvWorld.AuthorName = world.AuthorName;
                    tsvWorld.Name = world.Name;
                    tsvWorld.RecommendedCapacity = world.RecommendedCapacity;
                    tsvWorld.Capacity = world.Capacity;

                    tsvWorld.ReleaseStatus = world.ReleaseStatus;
                    
                    tsvWorld.CreatedAt = world.CreatedAt;
                    tsvWorld.UpdatedAt = world.UpdatedAt;
                    tsvWorld.Favorites = world.Favorites;
                    tsvWorld.Visits = world.Visits;
                    tsvWorld.Description = world.Description ?? string.Empty;
                    tsvWorld.Tags = world.Tags ?? new List<string>();
                    
                    tsvWorld.Result = "情報取得成功";

                    // リスト除外のワールドはスキップ
                    if (world.Category == "99_リスト除外")
                    {
                        world.Result = "除外";
                        continue;
                    }

                    // カテゴリに追加
                    if (!categoryDict.ContainsKey(world.Category))
                        categoryDict[world.Category] = new List<WorldDto>();

                    categoryDict[world.Category].Add(world);

                }
                catch (Exception e)
                {
                    tsvWorld.Result = $"情報取得失敗{e.Message}";
                    //Console.WriteLine($"Error fetching world data for ID: {tsvWorld.Id}");
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
