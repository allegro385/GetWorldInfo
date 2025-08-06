using GetWorldInfo.Dto;


namespace GetWorldInfo
{
    public class AggregationByCategories
    {
        public List<CategoryDto> Aggregate(List<WorldDto> worlds)
        //public List<CategoryDto> Aggregate(List<WorldDto> worlds)
        {
            // カテゴリの昇順、著者の昇順、作成日時の降順でソートし、"99_リスト除外"を除いたリストを作成する
            var worldsList = worlds
                .Where(w => w.Category != "99_リスト除外")
                .OrderBy(w => w.Category)
                .ThenBy(w => w.AuthorName)
                .ThenByDescending(w => w.CreatedAt)
                .ToList();

            // カテゴリごとにワールドをまとめる
            // カテゴリの頭3桁はソート用プレフィックスのため除外したcategoryを取得
            var categories = worldsList
                .GroupBy(w => w.Category.Substring(0, 3)) // カテゴリの頭3桁をキーにグループ化
                .Select(g => new CategoryDto
                {
                    Category = g.Key, // グループのキーをカテゴリ名として使用
                    Worlds = g.ToList() // グループ内のワールドをリスト化
                })
                .ToList();

            // 各カテゴリのワールド数を表示
            foreach (var category in categories)
            {
                Console.WriteLine($"カテゴリ: {category.Category}, ワールド数: {category.Worlds.Count}");
            }

            return categories;
        }

    }
}
