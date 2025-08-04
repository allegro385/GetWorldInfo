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
                if (parts.Length < 8) continue;

                // URLからworldId抽出
                string url = parts[6];
                string worldId = Regex.Match(url, @"wrld_[a-zA-Z0-9\-]+").Value;

                result.Add(new TsvWorldInfoDto
                {
                    Category = parts[0],
                    Creator = parts[1],
                    WorldName = parts[2],
                    QuestSupported = parts[3] == "1",
                    IosSupported = parts[4] == "1",
                    Isprivate = parts[5] == "1",
                    WorldURL = url,
                    WorldId = worldId,
                    DescriptionNote = parts[7],
                    result = ""
                });
            }
            return result;
        }
    }
}
