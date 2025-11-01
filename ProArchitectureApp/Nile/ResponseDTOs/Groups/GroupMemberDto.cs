namespace Nile.Service.Dtos
{
     public sealed class GroupMemberDto
    {
        public Guid GroupMemberId { get; set; }
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; } = "member";
        public DateTime JoinedAt { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
    }
}