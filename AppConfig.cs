using System.Text.Json;

namespace GetWorldInfo
{
    public class AppConfig
    {
        public string InputPath { get; set; }
        public string InputTsvName { get; set; }
        public string OutputJsonName { get; set; }
        public string OutputPath { get; set; }
        public string OutputVideoName { get; set; }

        public int shoriSign { get; set; }

        public static AppConfig Load(string path = "appsettings.json")
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppConfig>(json);
        }
    }
}
