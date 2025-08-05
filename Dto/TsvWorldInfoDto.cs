namespace GetWorldInfo.Dto
{
    ///tsv形式のワールド情報DTO
    public class TsvWorldInfoDto
    {
        public string Category { get; set; }       // カテゴリ
        public string Creator { get; set; }        // 作者
        public string WorldName { get; set; }      // ワールド名
        public int RecommendedCapacity { get; set; } // 推奨収容人数
        public int Capacity { get; set; }          // 収容人数
        public bool QuestSupported { get; set; }   // クエスト対応
        public bool IosSupported { get; set; }     // iOS対応
        public bool Isprivate { get; set; }     // 非公開ワールドかどうか
        public DateTime CreatedAt { get; set; }   // 作成日時
        public DateTime UpdatedAt { get; set; }   // 更新日時
        public int Favorites { get; set; }      // お気に入り数
        public int Visits { get; set; }         // 訪問数
        public string WorldURL { get; set; } = string.Empty;// URL
        public List<string> Tags { get; set; } = new List<string>();
        public string Description { get; set; } = string.Empty;// 説明
        public string Memo { get; set; }        // 個人メモ
        public string WorldId { get; set; }     // URLから抽出
        public string Result { get; set; }
    }
}
