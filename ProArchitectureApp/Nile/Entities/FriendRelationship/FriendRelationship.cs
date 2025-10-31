using System;

namespace Nile.Entities
{
    public class FriendRelationship
    {
        public Guid Id { get; set; }

        // who sent the friend request
        public Guid RequesterUserId { get; set; }

        // who received the friend request
        public Guid TargetUserId { get; set; }

        // "Pending", "Accepted", "Blocked"
        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime? AcceptedAt { get; set; }

        // navigation
        public User RequesterUser { get; set; } = default!;
        public User TargetUser { get; set; } = default!;
    }
}
