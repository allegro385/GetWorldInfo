using VRChat.API.Model;

namespace GetWorldInfo.Dto
{
    // ワールド情報DTO
    public class WorldDto
    {
        /// <summary>
        /// カテゴリ（入力データから取得）
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// World ID（URLから加工して取得）
        /// </summary>
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public int RecommendedCapacity { get; set; }
        public int Capacity { get; set; }

        /// <summary>
        /// プラットフォーム（入力データから取得）
        /// </summary>
        public PlatformDto Platform { get; set; } = new PlatformDto();
        public ReleaseStatus ReleaseStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Favorites { get; set; }
        public int Visits { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// 作者のワールド説明
        /// </summary>
        public string Description { get; set; } = string.Empty;

        public string ThumbnailImageUrl { get; set; } = string.Empty;
        /// <summary>
        /// URL（入力データから取得）
        /// </summary>
        public string WorldURL { get; set; } = string.Empty;

        /// <summary>
        /// 個人メモ（入力データから取得）
        /// </summary>
        public string Memo { get; set; } = string.Empty;

        /// <summary>
        /// 結果メッセージ
        /// </summary>
        public string Result { get; set; } = string.Empty;
    
        public WorldDto Clone()
        {
            return new WorldDto
            {
                Category = this.Category,
                Id = this.Id,
                Name = this.Name,
                AuthorName = this.AuthorName,
                RecommendedCapacity = this.RecommendedCapacity,
                Capacity = this.Capacity,
                Platform = new PlatformDto
                {
                    PC = this.Platform.PC,
                    Android = this.Platform.Android,
                    iOS = this.Platform.iOS
                },
                ReleaseStatus = this.ReleaseStatus,
                CreatedAt = this.CreatedAt,
                UpdatedAt = this.UpdatedAt,
                Favorites = this.Favorites,
                Visits = this.Visits,
                Tags = new List<string>(this.Tags),
                Description = this.Description,
                ThumbnailImageUrl = this.ThumbnailImageUrl,
                WorldURL = this.WorldURL,
                Memo = this.Memo,
                Result = this.Result
            };
        }
    }
}
