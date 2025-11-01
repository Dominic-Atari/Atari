using System;

namespace Nile.Entities
{
    public class Message
    {
        public Guid MessageId { get; set; }

        public Guid SenderUserId { get; set; }
        public Guid RecipientUserId { get; set; }

        public string Text { get; set; } = string.Empty;
        public bool IsRead { get; set; }

        public DateTime SentAt { get; set; }
        public DateTime CreatedAt { get; set; }


        // navs
        public User SenderUser { get; set; } = default!;
        public User RecipientUser { get; set; } = default!;
    }
}
