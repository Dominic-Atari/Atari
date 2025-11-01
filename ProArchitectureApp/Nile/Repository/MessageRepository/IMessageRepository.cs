using Nile.Entities;

namespace Nile.Repository
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task SendAsync(Message m);

        // A <-> B thread (both directions), newest last for chat display
        Task<IReadOnlyList<Message>> GetThreadAsync(Guid a, Guid b, int skip, int take);

        Task MarkReadAsync(Guid messageId);

        // Mark all messages from `other` to `me` as read
        Task MarkThreadReadAsync(Guid me, Guid other);

        // Optional: inbox preview (last N messages where user is sender or recipient)
        Task<IReadOnlyList<Message>> GetInboxAsync(Guid userId, int skip, int take);
    }
}
