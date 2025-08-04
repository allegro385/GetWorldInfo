using System.Text;

namespace GetWorldInfo
{
    public class TsvFileWriter
    {
        public static void Write(string path, List<TsvWorldInfoDto> tsvworlds)
        {
            var sb = new StringBuilder();
            sb.AppendLine("カテゴリ\t作者\tワールド名\tクエスト対応\tiOS対応\tPrivate\tURL\t説明\t結果\t作成日");

            foreach (var world in tsvworlds)
            {
                string quest = world.QuestSupported ? "1" : "";
                string ios = world.IosSupported ? "1" : "";
                string isPrivate = world.Isprivate ? "1" : "";

                sb.AppendLine($"{world.Category}\t{world.Creator}\t{world.WorldName}\t{quest}\t{ios}\t{isPrivate}\t{world.WorldURL}\t{world.DescriptionNote}\t{world.result}\t{world.CreatedAt}");
            }

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}
