using GetWorldInfo.Dto;
using VRChat.API.Model;


namespace GetWorldInfo
{
    public class AggregationByTags
    {
        public List<CategoryDto> Aggregate(List<WorldDto> worlds)
        {

            // タグごとに集計したワールドリストを作成
            // タグの頭3桁はソート用プレフィックスのため除外したtagsを取得
            // タグはプレフィックス"author_tag_" の付いたものを対象とし、"author_tag_" を除去して集計する
            // プレフィックスのフィルタ後、タグが0個以上のワールドを対象に、タグごとにワールドをグループ化する
            var categories = worlds
                .Where(w => w.Tags != null && w.Tags.Count > 0 && w.Category != "99_リスト除外") // タグが存在するワールドのみ対象
                .SelectMany(w => w.Tags
                    .Where(t => t.StartsWith("author_tag_")) // プレフィックスでフィルタ
                    .Select(t => new { Tag = t.Replace("author_tag_", "").ToLower(), World = w })) // プレフィックスを除去
                    .OrderBy(w => w.Tag) // タグでソート
                    .ThenBy(w => w.World.AuthorName) // 著者名でソート
                    .ThenBy(w => w.World.CreatedAt) // 作成日時で降順ソート
                .GroupBy(x => x.Tag) // タグをキーにグループ化
                .Select(g => new CategoryDto
                {
                    Category = g.Key, // グループのキーをカテゴリ名として使用
                    Worlds = g.Select(x => x.World).ToList() // グループ内のワールドをリスト化
                })
                .ToList();

            return categories;
        }

    }
}
