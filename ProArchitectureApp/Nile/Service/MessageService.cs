using Nile.Entities;
using Nile.Repository;
using Nile.Service.Dtos;

namespace Nile.Service
{
    public class MessageService
    {
        private readonly IMessageRepository _messages;
        private readonly IUserRepository _users;

        public MessageService(IMessageRepository messages, IUserRepository users)
        {
            _messages = messages;
            _users = users;
        }

        public async Task<MessageDto> SendAsync(SendMessageRequestDto req)
        {
            if (req.SenderUserId == Guid.Empty || req.RecipientUserId == Guid.Empty)
                throw new ArgumentException("Sender and recipient are required.");
            if (req.SenderUserId == req.RecipientUserId)
                throw new ArgumentException("Cannot send a message to yourself.");
            if (string.IsNullOrWhiteSpace(req.Text))
                throw new ArgumentException("Message text is required.");

            _ = await _users.GetByIdAsync(req.SenderUserId)   ?? throw new InvalidOperationException("Sender not found.");
            _ = await _users.GetByIdAsync(req.RecipientUserId)?? throw new InvalidOperationException("Recipient not found.");

            var entity = new Message
            {
                MessageId = Guid.NewGuid(),
                SenderUserId = req.SenderUserId,
                RecipientUserId = req.RecipientUserId,
                Text = req.Text,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _messages.SendAsync(entity);
            await _messages.SaveChangesAsync();

            return Map(entity);
        }

        public async Task<IReadOnlyList<MessageDto>> GetThreadAsync(Guid a, Guid b, int skip, int take)
        {
            var items = await _messages.GetThreadAsync(a, b, skip, take);
            return items.Select(Map).ToList();
        }

        public async Task MarkReadAsync(Guid messageId)
        {
            await _messages.MarkReadAsync(messageId);
            await _messages.SaveChangesAsync();
        }

        public async Task MarkThreadReadAsync(Guid me, Guid other)
        {
            await _messages.MarkThreadReadAsync(me, other);
            await _messages.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<MessageDto>> GetInboxAsync(Guid userId, int skip, int take)
        {
            var items = await _messages.GetInboxAsync(userId, skip, take);
            return items.Select(Map).ToList();
        }

        private static MessageDto Map(Message m) => new()
        {
            MessageId = m.MessageId,
            SenderUserId = m.SenderUserId,
            RecipientUserId = m.RecipientUserId,
            Text = m.Text,
            IsRead = m.IsRead,
            CreatedAt = m.CreatedAt
        };
    }
}
