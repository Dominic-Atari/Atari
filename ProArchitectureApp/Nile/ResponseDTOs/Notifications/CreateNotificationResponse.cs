namespace Nile.ResponseDTOs.Notifications
{
    public sealed class CreateNotificationRequest
{
    public Guid UserId { get; set; }          // recipient
    public Guid? ActorUserId { get; set; }     // sender/actor (nullable)
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ReferenceId { get; set; }
}

}

    