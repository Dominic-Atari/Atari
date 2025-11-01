namespace Nile.ResponseDTOs.FriendRequestDto
{
     public sealed class FriendDto
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
    }
}