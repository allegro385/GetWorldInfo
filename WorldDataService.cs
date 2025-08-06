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
        /// APIからワールド情報を取得し、入力データを更新する。
        /// </summary>
        /// <param name="tsvWorlds"></param>
        /// <returns></returns>

        public async Task<List<WorldDto>> GetCategoriesWithWorldsAsync(List<WorldDto> tsvWorlds)
        {
            var result = new List<WorldDto>();
            foreach (var tsvWorld in tsvWorlds)
            {
                WorldDto world;
                try
                {
                    // APIからワールド情報取得
                    world = await apiClient.GetWorldAsync(tsvWorld.Id);
                                     
                    //APIから取得できない情報を入力データの情報から設定する。
                    world.Category = tsvWorld.Category;
                    world.Platform.PC = tsvWorld.Platform.PC;
                    world.Platform.Android = tsvWorld.Platform.Android;
                    world.Platform.iOS = tsvWorld.Platform.iOS;
                    world.WorldURL = tsvWorld.WorldURL;
                    world.Memo = tsvWorld.Memo;

                    world.Result = "情報取得成功";

                    result.Add(world);
                }
                catch (Exception e)
                {
                    tsvWorld.Result = $"情報取得失敗{e.Message}";
                    result.Add(tsvWorld);
                }
                
            }

            return result;
        }
    }
}
