namespace GetWorldInfo
{
    public class WorldThumbnailDownloader
    {
        /// <summary>
        /// サムネイル画像をダウンロードして指定フォルダに保存
        /// </summary>
        /// <param name="categoriesWithWorlds">全カテゴリ＋ワールドリスト</param>
        /// <param name="baseFolder">ベースフォルダ</param>
        /// <returns>保存先の新規フォルダパス</returns>
        public static async Task<string> DownloadAsync(List<CategoryDto> categoriesWithWorlds, string baseFolder = ".")
        {
            var now = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var folder = Path.Combine(baseFolder, $"Thumbnail_{now}");
            Directory.CreateDirectory(folder);

            int index = 1;
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36");
            client.DefaultRequestHeaders.Referrer = new Uri("https://vrchat.com/");

            foreach (var category in categoriesWithWorlds)
            {
                foreach (var world in category.Worlds)
                {
                    if (string.IsNullOrEmpty(world.ThumbnailImageUrl)) continue;

                    var fileName = $"WorldThumbnail_{index.ToString("D5")}.jpg";
                    var filePath = Path.Combine(folder, fileName);

                    try
                    {
                        var bytes = await client.GetByteArrayAsync(world.ThumbnailImageUrl);
                        await File.WriteAllBytesAsync(filePath, bytes);
                        //Console.WriteLine($"[サムネDL] {fileName} ({world.Name})");
                        index++;
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"[DL失敗] {world.Name}({world.Id}) - {ex.Message}");
                    }
                }
            }
            return folder;
        }
    }
}
