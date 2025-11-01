using Microsoft.EntityFrameworkCore;
using Nile.Entities;

namespace Nile.Repository
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(NileDbContext db) : base(db) { }

        public async Task SendAsync(Message m)
        {
            await _db.Messages.AddAsync(m);
        }

        public async Task<IReadOnlyList<Message>> GetThreadAsync(Guid a, Guid b, int skip, int take)
        {
            return await _db.Messages
                .Where(m => (m.SenderUserId == a && m.RecipientUserId == b) ||
                            (m.SenderUserId == b && m.RecipientUserId == a))
                .OrderBy(m => m.CreatedAt) // chronological for chat display
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task MarkReadAsync(Guid messageId)
        {
            await _db.Messages
                .Where(m => m.MessageId == messageId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsRead, true));
        }

        public async Task MarkThreadReadAsync(Guid me, Guid other)
        {
            await _db.Messages
                .Where(m => m.RecipientUserId == me && m.SenderUserId == other && !m.IsRead)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsRead, true));
        }

        public async Task<IReadOnlyList<Message>> GetInboxAsync(Guid userId, int skip, int take)
        {
            // Simple inbox: latest messages where user is sender or recipient
            return await _db.Messages
                .Where(m => m.RecipientUserId == userId || m.SenderUserId == userId)
                .OrderByDescending(m => m.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}
