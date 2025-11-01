namespace Nile.ResponseDTOs.Notifications
{
    public sealed class NotificationDto
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }              // recipient
        public Guid? ActorUserId { get; set; }        // sender
        public string? ActorDisplayName { get; set; } // optional for UI
        public string? ActorAvatarUrl { get; set; }   // optional for UI
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? ReferenceId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}