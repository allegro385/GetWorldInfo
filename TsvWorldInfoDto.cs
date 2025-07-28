namespace GetWorldInfo
{
    public class TsvWorldInfoDto
    {
        public string WorldName { get; set; }      // ワールド名
        public string Category { get; set; }       // カテゴリ
        public bool QuestSupported { get; set; }   // クエスト対応
        public bool IosSupported { get; set; }     // iOS対応
        public string WorldId { get; set; }        // URLから抽出
        public string DescriptionNote { get; set; } // 説明欄
    }
}
