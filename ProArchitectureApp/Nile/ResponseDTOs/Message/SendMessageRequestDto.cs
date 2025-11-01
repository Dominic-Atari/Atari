namespace Nile.Service.Dtos
{
    public sealed class SendMessageRequestDto
    {
        public Guid SenderUserId { get; set; }
        public Guid RecipientUserId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
