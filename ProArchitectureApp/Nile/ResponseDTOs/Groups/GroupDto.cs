namespace Nile.Service.Dtos
{
    public sealed class GroupDto
    {
        public Guid GroupId { get; set; }
        public Guid OwnerUserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int MemberCount { get; set; }
    }
}