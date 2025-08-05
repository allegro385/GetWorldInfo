using VRChat.API.Model;

namespace GetWorldInfo.Dto
{
    // ワールド情報DTO
    public class WorldDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AuthorName { get; set; }
        public int RecommendedCapacity { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public ReleaseStatus ReleaseStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Favorites { get; set; }
        public int Visits { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string ThumbnailImageUrl { get; set; } = string.Empty;
                public PlatformDto Platform { get; set; }
        public string Memo { get; set; } = string.Empty;// 個人メモ
    }
}
