using GetWorldInfo.Dto;
using System.Text.RegularExpressions;

namespace GetWorldInfo
{
    // TSV読み込みクラス
    public class TsvFileReader
    {
        public static List<WorldDto> Read(string tsvPath)
        {
            var result = new List<WorldDto>();
            var lines = File.ReadAllLines(tsvPath);
            for (int i = 1; i < lines.Length; i++) // 1行目はヘッダー
            {
                var parts = lines[i].Split('\t');
                if (parts.Length < 16) continue;

                // URLからworldId抽出
                string url = parts[12];
                string worldId = Regex.Match(url, @"wrld_[a-zA-Z0-9\-]+").Value;

                result.Add(new WorldDto
                {
                    Category = parts[0],
                    AuthorName = parts[1],
                    Name = parts[2],
                    RecommendedCapacity = int.TryParse(parts[3], out var recCap) ? recCap : 0,
                    Capacity = int.TryParse(parts[4], out var cap) ? cap : 0,
                    Platform = new PlatformDto
                    {
                        PC = true,
                        Android = parts[5] == "1",
                        iOS = parts[6] == "1"
                    },
                    ReleaseStatus = (parts[7] == "1") ? VRChat.API.Model.ReleaseStatus.Private : VRChat.API.Model.ReleaseStatus.Public,
                    CreatedAt = DateTime.TryParse(parts[8], out var createdAt) ? createdAt : DateTime.MinValue,
                    UpdatedAt = DateTime.TryParse(parts[9], out var updatedAt) ? updatedAt : DateTime.MinValue,
                    Favorites = int.TryParse(parts[10], out var favorites) ? favorites : 0,
                    Visits = int.TryParse(parts[11], out var visits) ? visits : 0,
                    WorldURL = url,
                    Id = worldId,
                    Tags = parts[13].Split(',').Select(tag => tag.Trim()).ToList(),
                    Description = parts[14],
                    Memo = parts[15],
                    Result = ""
                });
            }
            return result;
        }
    }
}
