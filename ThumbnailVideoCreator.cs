using System.Diagnostics;
using System.Drawing;

namespace GetWorldInfo
{
    public class ThumbnailVideoCreator
    {
        /// <summary>
        /// 指定フォルダ内のサムネイル画像を1秒ずつ切り替える動画を作成する
        /// </summary>
        /// <param name="folderPath">サムネイル画像があるフォルダ</param>
        /// <param name="outputVideoPath">出力する動画ファイルパス</param>
        public static async Task CreateThumbnailVideoAsync(string folderPath, string outputVideoPath)
        {
            // 画像の並びを取得（昇順ソート）
            var imageFiles = Directory.GetFiles(folderPath, "WorldThumbnail_*.jpg");
            Array.Sort(imageFiles);

            if (imageFiles.Length == 0)
            {
                Console.WriteLine("サムネイル画像がありません。");
                return;
            }

            // ダミー画像を先頭と末尾用に作成
            string dummyPath = Path.Combine(folderPath, "dummy.jpg");
            using (var bmp = new Bitmap(imageFiles[0]))
            {
                bmp.Save(dummyPath);
            }

            // ffmpeg用のリストファイルを作成
            string listFile = Path.Combine(folderPath, "list.txt");
            using (var sw = new StreamWriter(listFile))
            {
                sw.WriteLine($"file '{dummyPath}'");
                sw.WriteLine("duration 1");

                foreach (var img in imageFiles)
                {
                    sw.WriteLine($"file '{img}'");
                    sw.WriteLine("duration 1");
                }

                sw.WriteLine($"file '{dummyPath}'");
                sw.WriteLine("duration 1");
            }

            // ffmpegコマンドを実行
            try
            {
                string ffmpegArgs = $"-y -f concat -safe 0 -i \"{listFile}\" -vsync vfr -pix_fmt yuv420p \"{outputVideoPath}\"";
                var psi = new ProcessStartInfo("ffmpeg", ffmpegArgs)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };
                var proc = Process.Start(psi);
                string output = await proc.StandardOutput.ReadToEndAsync();
                string error = await proc.StandardError.ReadToEndAsync();
                await proc.WaitForExitAsync();

                Console.WriteLine("[ffmpeg 出力]\n" + output);
                Console.WriteLine("[ffmpeg エラー]\n" + error);

                Console.WriteLine($"動画作成完了: {outputVideoPath}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ffmpegの実行中にエラーが発生しました: {ex.Message}");
                return;
            }
        }
    }
}
