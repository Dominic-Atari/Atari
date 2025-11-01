using System;

namespace Nile.Entities
{
   // Nile.Entities/Notification.cs
    public class Notification
    {
    public Guid NotificationId { get; set; }

    // Recipient (who receives this notification)
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    // Actor/Sender (who triggered it) – nullable to allow system-generated notifications
    public Guid? ActorUserId { get; set; }
    public User? ActorUser { get; set; }

    public string Type { get; set; } = string.Empty;    // "Like","Comment","FriendAccepted",...
    public string Message { get; set; } = string.Empty; // short text the UI shows
    public string? ReferenceId { get; set; }            // e.g., PostId/CommentId/FriendRel Id (as string)
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    }
}
