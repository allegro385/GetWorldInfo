using GetWorldInfo.Dto;
using System;
using System.Collections.Generic;

namespace GetWorldInfo
{
    public class WorldThumbnailDownloader
    {
        /// <summary>
        /// WorldのIDとサムネイル画像Pathの辞書
        /// </summary>
        private Dictionary<string,string> ImageDictionary = new Dictionary<string, string>();

        private static readonly HttpClient client = new HttpClient();
        static WorldThumbnailDownloader()
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36");
            client.DefaultRequestHeaders.Referrer = new Uri("https://vrchat.com/");
        }

        /// <summary>
        /// サムネイル画像をダウンロードして指定フォルダに保存
        /// </summary>
        /// <param name="worlds">ワールドリスト</param>
        /// <param name="outputPath">出力フォルダ</param>
        public async Task DownloadAsync(List<WorldDto> worlds, string outputPath)
        {       
            int index = 1;
            foreach (var world in worlds)
            {
                if (string.IsNullOrEmpty(world.ThumbnailImageUrl)) continue;

                if(ImageDictionary.ContainsKey(world.Id)) {
                    // 既にダウンロード済みのサムネイルはスキップ
                    continue;
                }

                var fileName = $"{index.ToString("D5")}.png";
                var filePath = Path.Combine(outputPath, fileName);

                try
                {
                    var bytes = await client.GetByteArrayAsync(world.ThumbnailImageUrl);
                    await File.WriteAllBytesAsync(filePath, bytes);
                    ImageDictionary[world.Id] = filePath; // IDとファイル名を記録
                    index++;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"[DL失敗] {world.Name}({world.Id}) - {ex.Message}");
                }
            }            
        }

        /// <summary>
        /// 設定済みのサムネイル画像ファイル名からイメージ画像をコピーするメソッド
        /// <param name="categories">カテゴリリスト</param>
        /// <paramref name="outputPath">出力パス</paramref>
        /// </summary>
        public void CopyThumbnails(List<CategoryDto> categories, string outputPath)
        {
            var index = 1;

            foreach (var category in categories)
            {
                foreach (var world in category.Worlds)
                {
                    if (!ImageDictionary.ContainsKey(world.Id)) continue;
                    var sourcePath = ImageDictionary[world.Id];
                    var destPath = Path.Combine(outputPath, $"{index.ToString("D5")}.png");
                    try
                    {
                        if (File.Exists(sourcePath))
                        {
                            File.Copy(sourcePath, destPath, true);
                            index++;
                        }
                        else
                        {
                            Console.WriteLine($"[ファイル未検出] {sourcePath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[コピー失敗] {world.Name}({world.Id}) - {ex.Message}");
                    }
                }
            }
        }
    }
}
