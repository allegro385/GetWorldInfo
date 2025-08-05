using GetWorldInfo.Dto;

namespace GetWorldInfo
{
    public class WorldThumbnailDownloader
    {
        private static readonly HttpClient client = new HttpClient();
        static WorldThumbnailDownloader()
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36");
            client.DefaultRequestHeaders.Referrer = new Uri("https://vrchat.com/");
        }

        /// <summary>
        /// サムネイル画像をダウンロードして指定フォルダに保存
        /// </summary>
        /// <param name="categoriesWithWorlds">全カテゴリ＋ワールドリスト</param>
        /// <param name="outputPath">出力フォルダ</param>
        public static async Task DownloadAsync(List<CategoryDto> categoriesWithWorlds, string outputPath)
        {       
            int index = 1;

            foreach (var category in categoriesWithWorlds)
            {
                foreach (var world in category.Worlds)
                {
                    if (string.IsNullOrEmpty(world.ThumbnailImageUrl)) continue;

                    var fileName = $"{index.ToString("D5")}.png";
                    var filePath = Path.Combine(outputPath, fileName);

                    try
                    {
                        //var bytes = await client.GetByteArrayAsync(world.ThumbnailImageUrl);
                        var bytes = await client.GetByteArrayAsync(world.ThumbnailImageUrl);
                        await File.WriteAllBytesAsync(filePath, bytes);
                        //Console.WriteLine($"[サムネDL] {fileName} ({world.Name}){world.ThumbnailImageUrl}");
                        index++;
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"[DL失敗] {world.Name}({world.Id}) - {ex.Message}");
                    }
                }
            }
        }
    }
}
