// Nile/Repository/GroupRepository.cs
using Microsoft.EntityFrameworkCore;
using Nile.Entities;

namespace Nile.Repository
{
    public class GroupRepository : GenericRepository<Group>, IGroupRepository
    {
        public GroupRepository(NileDbContext db) : base(db) { }

        public async Task<Group?> GetWithMembersAsync(Guid groupId)
        {
            return await _db.Groups
                .Where(g => g.GroupId == groupId)
                .Include(g => g.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Group>> GetOwnedByAsync(Guid ownerUserId)
        {
            return await _db.Groups
                .Where(g => g.OwnerUserId == ownerUserId)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Group>> SearchByNameAsync(string term, int take)
        {
            term = term.Trim();
            return await _db.Groups
                .Where(g => g.Name.Contains(term))
                .OrderBy(g => g.Name)
                .Take(take)
                .ToListAsync();
        }
    }
}
