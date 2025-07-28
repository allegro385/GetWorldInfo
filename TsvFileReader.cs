using System.Text.RegularExpressions;

namespace GetWorldInfo
{
    // TSV読み込みクラス
    public class TsvFileReader
    {
        public static List<TsvWorldInfoDto> Read(string tsvPath)
        {
            var result = new List<TsvWorldInfoDto>();
            var lines = File.ReadAllLines(tsvPath);
            for (int i = 1; i < lines.Length; i++) // 1行目はヘッダー
            {
                var parts = lines[i].Split('\t');
                if (parts.Length < 7) continue;

                // URLからworldId抽出
                string url = parts[5];
                string worldId = Regex.Match(url, @"wrld_[a-zA-Z0-9\-]+").Value;

                result.Add(new TsvWorldInfoDto
                {
                    WorldName = parts[0],
                    Category = parts[1],
                    QuestSupported = parts[2] == "1",
                    IosSupported = parts[3] == "1",
                    WorldId = worldId,
                    DescriptionNote = parts[6]
                });
            }
            return result;
        }
    }
}
