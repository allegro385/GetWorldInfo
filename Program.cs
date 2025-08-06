namespace GetWorldInfo
{
    class Program
    {
        static async Task Main()
        {
            var config = AppConfig.Load();

            var apiClient = new WorldApiClient();
            var service = new WorldDataService(apiClient);

            var tsvData = TsvFileReader.Read(Path.Combine(config.InputPath, $"{config.InputTsvName}"));

            if (config.shoriSign >= 1)
            {
                Console.WriteLine("ワールド情報の取得");

                //出力フォルダの設定
                var now = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var OutputPath = Path.Combine(config.OutputPath, $"{now}");
                Directory.CreateDirectory(OutputPath);

                var worldList = await service.GetCategoriesWithWorldsAsync(tsvData);

                //tsvファイルの出力
                TsvFileWriter.Write(Path.Combine(OutputPath, $"{config.InputTsvName}"), worldList);


                if (config.shoriSign >= 2)
                {

                    Console.WriteLine("サムネイル動画の作成");
                    //サムネイルフォルダの作成
                    var ThumbnailPath = Path.Combine(OutputPath, "Thumbnail");
                    Directory.CreateDirectory(ThumbnailPath);

                    //サムネイル画像の取得
                    var thumbnailDownloader = new WorldThumbnailDownloader();
                    await thumbnailDownloader.DownloadAsync(worldList, ThumbnailPath);

                    //カテゴリごとにワールドをまとめる
                    var categoriesWithWorlds = new AggregationByCategories().Aggregate(worldList);

                    //JSONファイルの出力
                    await JsonFileWriter.WriteWorldPortalJsonAsync(categoriesWithWorlds, Path.Combine(OutputPath, $"{config.OutputJsonName}"));

                    //サムネイル動画の作成
                    var ThumbnailPath1 = Path.Combine(OutputPath, "Thumbnail_1");
                    Directory.CreateDirectory(ThumbnailPath1);

                    thumbnailDownloader.CopyThumbnails(categoriesWithWorlds, ThumbnailPath1);

                    await ThumbnailVideoCreator.CreateThumbnailVideoAsync(ThumbnailPath1, OutputPath, config.OutputVideoName);
                }
            }
            Console.WriteLine($"処理完了");
        }
    }
}
