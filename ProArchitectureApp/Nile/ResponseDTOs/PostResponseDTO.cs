namespace Nile.ResponseDTOs
{
    public class PostResponseDTO
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string ContentText { get; set; } = string.Empty;
        public string? MediaUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // optional lightweight user info to show in feed UI
        public string? UserDisplayName { get; set; }
        public string? UserAvatarUrl { get; set; }
    }
}
