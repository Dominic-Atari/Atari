namespace Nile.Entities
{
    using System;
    public class GroupMember
    {
        public Guid GroupMemberId { get; set; }

        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }

        // "member", "admin", "mod"
        public string Role { get; set; } = "member";

        public DateTime JoinedAt { get; set; }

        // nav
        public Group Group { get; set; } = default!;
        public User User { get; set; } = default!;
    }
}