namespace GetWorldInfo.Dto
{
    ///tsv形式のワールド情報DTO
    public class TsvWorldInfoDto
    {
        public string Category { get; set; }       // カテゴリ
        public string Creator { get; set; }        // 作者
        public string WorldName { get; set; }      // ワールド名
        public bool QuestSupported { get; set; }   // クエスト対応
        public bool IosSupported { get; set; }     // iOS対応
        public bool Isprivate { get; set; }     // 非公開ワールドかどうか
        public string WorldURL { get; set; }       // URL
        public string WorldId { get; set; }        // URLから抽出
        public string DescriptionNote { get; set; } // 説明欄
        public string CreatedAt { get; set; } // 作成日時
        public string UpdatedAt { get; set; } // 更新日時
        public string result { get; set; }
    }
}
