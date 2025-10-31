using Nile.Entities;
using Nile.Repository;

namespace Nile.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);

        // Friends the user has (accepted only)
        Task<IReadOnlyList<User>> GetFriendsAsync(Guid userId);

        // Pending friend requests TO this user
        Task<IReadOnlyList<FriendRelationship>> GetIncomingFriendRequestsAsync(Guid userId);

        // Check if A and B are friends (accepted)
        Task<bool> AreFriendsAsync(Guid userAId, Guid userBId);
    }
}
