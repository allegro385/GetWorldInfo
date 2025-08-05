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
                    world.Memo = tsvWorld.DescriptionNote;

                    // TSV用の情報を更新
                    tsvWorld.WorldName = world.Name;
                    tsvWorld.Creator = world.AuthorName;
                    tsvWorld.CreatedAt = world.CreatedAt.ToString("yyyy/MM/dd");
                    tsvWorld.UpdatedAt = world.UpdatedAt.ToString("yyyy/MM/dd");
                    tsvWorld.result = "◯";

                    // カテゴリに追加
                    if (!categoryDict.ContainsKey(tsvWorld.Category))
                        categoryDict[tsvWorld.Category] = new List<WorldDto>();

                    categoryDict[tsvWorld.Category].Add(world);

                }
                catch (Exception)
                {
                    tsvWorld.result = "-";
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
