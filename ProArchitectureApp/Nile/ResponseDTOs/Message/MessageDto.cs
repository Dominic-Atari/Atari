namespace Nile.Service.Dtos
{
    public sealed class MessageDto
    {
        public Guid MessageId { get; set; }
        public Guid SenderUserId { get; set; }
        public Guid RecipientUserId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}