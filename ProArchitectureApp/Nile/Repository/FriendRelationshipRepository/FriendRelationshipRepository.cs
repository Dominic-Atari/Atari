using Microsoft.EntityFrameworkCore;
using Nile.Entities;

namespace Nile.Repository
{
    public class FriendRelationshipRepository : GenericRepository<FriendRelationship>, IFriendRelationshipRepository
    {
        public FriendRelationshipRepository(NileDbContext db) : base(db) { }

        public async Task<FriendRelationship?> GetRelationshipAsync(Guid requesterId, Guid targetId)
        {
            return await _db.FriendRelationships
                .FirstOrDefaultAsync(fr => fr.RequesterUserId == requesterId && fr.TargetUserId == targetId);
        }

        public async Task<IReadOnlyList<FriendRelationship>> GetPendingRequestsForUserAsync(Guid userId)
        {
            return await _db.FriendRelationships
                .Where(fr => fr.TargetUserId == userId && fr.Status == "Pending")
                .Include(fr => fr.RequesterUser)
                .OrderByDescending(fr => fr.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<User>> GetAcceptedFriendsForUserAsync(Guid userId)
        {
            // Symmetric: requester->target OR target->requester with Status=Accepted
            var fromMe = _db.FriendRelationships
                .Where(fr => fr.RequesterUserId == userId && fr.Status == "Accepted")
                .Select(fr => fr.TargetUser);

            var toMe = _db.FriendRelationships
                .Where(fr => fr.TargetUserId == userId && fr.Status == "Accepted")
                .Select(fr => fr.RequesterUser);

            return await fromMe.Union(toMe).Distinct().ToListAsync();
        }

        public async Task CreateRequestAsync(FriendRelationship relationship)
        {
            await _db.FriendRelationships.AddAsync(relationship);
        }

        public async Task AcceptRequestAsync(Guid relationshipId)
        {
            await _db.FriendRelationships
                .Where(fr => fr.Id == relationshipId && fr.Status == "Pending")
                .ExecuteUpdateAsync(u => u
                    .SetProperty(fr => fr.Status, "Accepted")
                    .SetProperty(fr => fr.AcceptedAt, DateTime.UtcNow));
        }
    }
}
