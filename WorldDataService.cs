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

        public async Task<List<CategoryDto>> GetCategoriesWithWorldsAsync(List<TsvWorldInfoDto> tsvWorlds)
        {

            // カテゴリごとにワールドをまとめる辞書
            var categoryDict = new Dictionary<string, List<WorldDto>>();
            foreach (var tsvWorld in tsvWorlds)
            {
                WorldDto world;
                try
                {
                    //Console.WriteLine($"[取得中] {tsvWorld.WorldId}");

                    // APIからワールド情報取得
                    world = await apiClient.GetWorldAsync(tsvWorld.WorldId);

                    // Platformの情報はAPIから取得できないためtsvの情報を仕様する。               
                    if (tsvWorld.QuestSupported) world.Platform.Android = true;
                    if (tsvWorld.IosSupported) world.Platform.iOS = true;

                    //tsvの個人メモを追加
                    world.Memo = tsvWorld.Memo;

                    // TSV用の情報を更新
                    tsvWorld.Creator = world.AuthorName;
                    tsvWorld.WorldName = world.Name;
                    tsvWorld.RecommendedCapacity = world.RecommendedCapacity;
                    tsvWorld.Capacity = world.Capacity;

                    tsvWorld.Isprivate = world.ReleaseStatus.ToString().ToLower() == "private";
                    tsvWorld.CreatedAt = world.CreatedAt;
                    tsvWorld.UpdatedAt = world.UpdatedAt;
                    tsvWorld.Favorites = world.Favorites;
                    tsvWorld.Visits = world.Visits;
                    tsvWorld.Description = world.Description ?? string.Empty;
                    tsvWorld.Tags = world.Tags ?? new List<string>();
                    
                    tsvWorld.Result = "情報取得成功";

                    // リスト除外のワールドはスキップ
                    if (tsvWorld.Category == "99_リスト除外")
                    {
                        tsvWorld.Result = "除外";
                        continue;
                    }

                    // カテゴリに追加
                    if (!categoryDict.ContainsKey(tsvWorld.Category))
                        categoryDict[tsvWorld.Category] = new List<WorldDto>();

                    categoryDict[tsvWorld.Category].Add(world);

                }
                catch (Exception e)
                {
                    tsvWorld.Result = $"情報取得失敗{e.Message}";
                    //Console.WriteLine($"Error fetching world data for ID: {tsvWorld.WorldId}");
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
