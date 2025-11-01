namespace Nile.Service.Dtos
{
    public sealed class FriendRequestDto
    {
        public Guid RelationshipId { get; set; }
        public Guid RequesterUserId { get; set; }
        public Guid TargetUserId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public string? RequesterDisplayName { get; set; }
        public string? RequesterAvatarUrl { get; set; }
    }
}
