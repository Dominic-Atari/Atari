using System;

namespace Nile.Entities
{
    public class Notification
    {
        public Guid NotificationId { get; set; }

        // who this notification is for
        public Guid UserId { get; set; }

        // e.g. "like", "comment", "friend_request", "message"
        public string Type { get; set; } = string.Empty;

        // Human readable text e.g. "Dominic liked your post"
        public string Message { get; set; } = string.Empty;

        // optional: something to navigate to in the app (post ID, user ID, etc.)
        public string? ReferenceId { get; set; }

        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        // nav
        public User User { get; set; } = default!;
    }
}
