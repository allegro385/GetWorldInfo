using System.Text.Json;

namespace GetWorldInfo
{
    public class AppConfig
    {
        public string InputTsvPath { get; set; }
        public string OutputJsonPath { get; set; }
        public string ThumbnailFolder { get; set; }
        public string OutputVideoPath { get; set; }

        public static AppConfig Load(string path = "appsettings.json")
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppConfig>(json);
        }
    }
}
