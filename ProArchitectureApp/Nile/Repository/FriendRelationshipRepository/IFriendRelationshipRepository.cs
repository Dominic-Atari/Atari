using Nile.Entities;
namespace Nile.Repository
{
    public interface IFriendRelationshipRepository : IGenericRepository<FriendRelationship>
    {
        Task<FriendRelationship?> GetRelationshipAsync(Guid requesterId, Guid targetId);

        Task<IReadOnlyList<FriendRelationship>> GetPendingRequestsForUserAsync(Guid userId);

        Task<IReadOnlyList<User>> GetAcceptedFriendsForUserAsync(Guid userId);

        Task CreateRequestAsync(FriendRelationship relationship);

        Task AcceptRequestAsync(Guid relationshipId);

    }
}