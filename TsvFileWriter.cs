using GetWorldInfo.Dto;
using System.Text;

namespace GetWorldInfo
{
    public class TsvFileWriter
    {
        public static void Write(string path, List<TsvWorldInfoDto> tsvworlds)
        {
            var sb = new StringBuilder();
            sb.AppendLine("カテゴリ\t作者\tワールド名\t推奨人数\t収容人数\tクエスト対応\tiOS対応\tPrivate\t作成日時\t更新日時\tお気に入り\t訪問数\tURL\tタグ\tワールド説明\t個人メモ\t結果");

            foreach (var world in tsvworlds)
            {
                string quest = world.QuestSupported ? "1" : "";
                string ios = world.IosSupported ? "1" : "";
                string isPrivate = world.Isprivate ? "1" : "";
                string tags = string.Join(",", world.Tags);

                sb.AppendLine($"{world.Category}\t{world.Creator}\t{world.WorldName}\t{world.RecommendedCapacity}\t{world.Capacity}\t{quest}\t{ios}\t{isPrivate}\t{world.CreatedAt:yyyy-MM-dd HH:mm:ss}\t{world.UpdatedAt:yyyy-MM-dd HH:mm:ss}\t{world.Favorites}\t{world.Visits}\t{world.WorldURL}\t{tags}\t{world.Description}\t{world.Memo}\t{world.Result}");

            }
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}
