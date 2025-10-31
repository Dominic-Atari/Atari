using Nile.Entities;

namespace Nile.Repository
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        // Send message (store it)
        Task AddMessageAsync(Message message);

        // Get full conversation between two users (sorted oldest -> newest or newest -> oldest)
        Task<IReadOnlyList<Message>> GetConversationAsync(Guid userAId, Guid userBId, int takeLast);

        // How many unread messages does a user have?
        Task<int> GetUnreadCountAsync(Guid userId);

        // Mark messages as seen in a conversation
        Task MarkConversationSeenAsync(Guid readerUserId, Guid otherUserId);
    }
}