namespace GetWorldInfo
{
    class Program
    {
        static async Task Main()
        {
            var config = AppConfig.Load();

            var apiClient = new WorldApiClient();
            var service = new WorldDataService(apiClient);
            var tsvData = TsvFileReader.Read(config.InputTsvPath);

            var categoriesWithWorlds = await service.GetCategoriesWithWorldsAsync(tsvData);
            await JsonFileWriter.WriteWorldPortalJsonAsync(categoriesWithWorlds, config.OutputJsonPath);

            var thumbnailFolder = await WorldThumbnailDownloader.DownloadAsync(categoriesWithWorlds, config.ThumbnailFolder);
            await ThumbnailVideoCreator.CreateThumbnailVideoAsync(thumbnailFolder, config.OutputVideoPath);

            Console.WriteLine($"✅ JSONファイル生成完了: {config.OutputJsonPath}");
        }
    }
}
