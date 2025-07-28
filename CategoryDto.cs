namespace GetWorldInfo
{
    // カテゴリ＋ワールド集約用DTO
    public class CategoryDto
    {
        public string Category { get; set; }
        public List<WorldDto> Worlds { get; set; }
    }
}
