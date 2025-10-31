using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nile;
using Nile.Entities;
using Nile.Repository;

namespace Nile.Repository.UserRepository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(NileDbContext db) : base(db)
        {
        }

        // For login / lookup
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        // All accepted friends of a user
        // We treat friendship as symmetric:
        // - If A sent request to B and B accepted => they are friends
        // - If B sent request to A and A accepted => still friends
        public async Task<IReadOnlyList<User>> GetFriendsAsync(Guid userId)
        {
            // friends where 'userId' was the requester
            var fromMe = _db.FriendRelationships
                .Where(fr =>
                    fr.RequesterUserId == userId &&
                    fr.Status == "Accepted")
                .Select(fr => fr.TargetUser);

            // friends where 'userId' was the target
            var toMe = _db.FriendRelationships
                .Where(fr =>
                    fr.TargetUserId == userId &&
                    fr.Status == "Accepted")
                .Select(fr => fr.RequesterUser);

            // union both directions
            var all = await fromMe
                .Union(toMe)
                .Distinct()
                .ToListAsync();

            return all;
        }

        // Incoming friend requests:
        // - TargetUserId = me
        // - Status = "Pending"
        // return the FriendRelationship rows so the UI
        // can show: "Dominic wants to be your friend"
        public async Task<IReadOnlyList<FriendRelationship>> GetIncomingFriendRequestsAsync(Guid userId)
        {
            return await _db.FriendRelationships
                .Where(fr =>
                    fr.TargetUserId == userId &&
                    fr.Status == "Pending")
                .Include(fr => fr.RequesterUser) // so caller can show requester info
                .ToListAsync();
        }

        // Check if two users are already friends (Accepted)
        public async Task<bool> AreFriendsAsync(Guid userAId, Guid userBId)
        {
            return await _db.FriendRelationships.AnyAsync(fr =>
                fr.Status == "Accepted" &&
                (
                    (fr.RequesterUserId == userAId && fr.TargetUserId == userBId) ||
                    (fr.RequesterUserId == userBId && fr.TargetUserId == userAId)
                )
            );
        }
    }
}
