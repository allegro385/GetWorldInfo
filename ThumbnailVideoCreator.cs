using System.Diagnostics;
using System.Drawing;

namespace GetWorldInfo
{
    public class ThumbnailVideoCreator
    {
        /// <summary>
        /// 指定フォルダ内のサムネイル画像を1秒ずつ切り替える動画を作成する
        /// </summary>
        /// <param name="ThumbnailPath">サムネイル画像があるフォルダ</param>
        /// <param name="outputVideoPath">出力する動画ファイルパス</param>
        public static async Task CreateThumbnailVideoAsync(string ThumbnailPath, string outputVideoPath,string outputVideoName)
        {
            // 画像の並びを取得（昇順ソート）
            var imageFiles = Directory.GetFiles(ThumbnailPath, "*.png");
            Array.Sort(imageFiles);

            if (imageFiles.Length == 0)
            {
                Console.WriteLine("サムネイル画像がありません。");
                return;
            }

            // ダミー画像（先頭画像のコピー）を先頭と末尾用に作成
            string dummyPath1 = Path.Combine(ThumbnailPath, $"{0.ToString("D5")}.png");
            string dummyPath2 = Path.Combine(ThumbnailPath, $"{(imageFiles.Length + 1).ToString("D5")}.png");
            using (var bmp = new Bitmap(imageFiles[0]))
            {
                bmp.Save(dummyPath1);
                bmp.Save(dummyPath2);
            }

            // ffmpegコマンドを実行
            try
            {
                string ffmpegArgs = $"-r 1 -i %05d.png -vcodec libx264 -profile:v baseline -pix_fmt yuv420p -movflags +faststart {outputVideoName}";
                var psi = new ProcessStartInfo("ffmpeg", ffmpegArgs)
                {
                    WorkingDirectory = ThumbnailPath,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                };

                Console.WriteLine($"[ffmpeg実行]");

                var proc = Process.Start(psi);

                //// 標準出力・エラーを同時に読み込む
                //Task<string> outputTask = proc.StandardOutput.ReadToEndAsync();
                //Task<string> errorTask = proc.StandardError.ReadToEndAsync();

                await proc.WaitForExitAsync();

                Console.WriteLine($"[ffmpeg完了]");

                //// 読み込み結果を取得
                //string output = await outputTask;
                //string error = await errorTask;

                //Console.WriteLine("[ffmpeg 出力]\n" + output);
                //Console.WriteLine("[ffmpeg エラー]\n" + error);

                //作成したVideoファイルをコピーする
                File.Copy(Path.Combine(ThumbnailPath, outputVideoName), Path.Combine(outputVideoPath, outputVideoName));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ffmpegの実行中にエラーが発生しました: {ex.Message}");
                return;
            }
        }
    }
}
