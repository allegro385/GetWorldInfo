using GetWorldInfo.Dto;
using VRChat.API.Api;
using VRChat.API.Client;
using VRChat.API.Model;


namespace GetWorldInfo
{
    // APIクライアント（VRChat API想定）
    public class WorldApiClient
    {
        public async Task<WorldDto> GetWorldAsync(string worldId)
        {
            try
            {
                // Create a configuration for us to log in
                Configuration config = new Configuration();

                // We have to identify ourselves according to the vrchat tos,
                // We can't use emails here bc http header parser complains
                config.UserAgent = "VRCPortalFetcher/1.0";

                // Create a client to hold all our cookies :D
                ApiClient client = new ApiClient();

                // We also need to create instances of the other APIs we'll need
                WorldsApi worldApi = new WorldsApi(client, client, config);
                // TODO: VRChat API実装
                World world = worldApi.GetWorld($"{worldId}");
                await Task.Delay(300);

                //WorldAPIからの情報を移送する
                return new WorldDto
                {
                    Id = world.Id,
                    Name = world.Name,
                    AuthorName = world.AuthorName,
                    RecommendedCapacity = world.RecommendedCapacity,
                    Capacity = world.Capacity,
                    Description = world.Description,
                    Platform = new PlatformDto { PC = true, Android = false, iOS = false },
                    ReleaseStatus = world.ReleaseStatus,
                    CreatedAt = world.CreatedAt,
                    UpdatedAt = world.UpdatedAt,
                    Favorites = world.Favorites,
                    Visits = world.Visits,
                    Tags = world.Tags ?? new List<string>(),
                    ThumbnailImageUrl = world.ThumbnailImageUrl ?? string.Empty

                };
            }
            catch (Exception ex)
            {
                // エラーログ出力
                Console.WriteLine($"[エラー] {worldId} の情報取得に失敗: {ex.Message}");
                return new WorldDto
                {
                    Id = worldId,
                };
            }
        }
    }
}
