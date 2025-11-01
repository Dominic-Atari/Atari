// Nile/Repository/GroupMemberRepository/GroupMemberRepository.cs
using Microsoft.EntityFrameworkCore;
using Nile.Entities;

namespace Nile.Repository.GroupMemberRepository
{
    public class GroupMemberRepository : GenericRepository<GroupMember>, IGroupMemberRepository
    {
        public GroupMemberRepository(NileDbContext db) : base(db) { }

        public async Task<bool> IsMemberAsync(Guid groupId, Guid userId) =>
            await _db.GroupMembers.AnyAsync(x => x.GroupId == groupId && x.UserId == userId);

        public async Task<bool> IsAdminAsync(Guid groupId, Guid userId) =>
            await _db.GroupMembers.AnyAsync(x => x.GroupId == groupId && x.UserId == userId && x.Role == "admin");

        public async Task<GroupMember?> GetAsync(Guid groupId, Guid userId) =>
            await _db.GroupMembers.FirstOrDefaultAsync(x => x.GroupId == groupId && x.UserId == userId);

        public override async Task AddAsync(GroupMember member)
        {
            await _db.GroupMembers.AddAsync(member);
        }

        public async Task RemoveAsync(Guid groupId, Guid userId)
        {
            var gm = await GetAsync(groupId, userId);
            if (gm != null) _db.GroupMembers.Remove(gm);
        }

        public async Task ChangeRoleAsync(Guid groupId, Guid userId, string role)
        {
            var gm = await GetAsync(groupId, userId);
            if (gm == null) return;
            gm.Role = role;
            _db.GroupMembers.Update(gm);
        }

        public async Task<IReadOnlyList<GroupMember>> GetMembersAsync(Guid groupId)
        {
            return await _db.GroupMembers
                .Where(x => x.GroupId == groupId)
                .Include(x => x.User)
                .OrderBy(x => x.Role) // admin/mod/member
                .ThenBy(x => x.JoinedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Group>> GetGroupsForUserAsync(Guid userId)
        {
            return await _db.GroupMembers
                .Where(x => x.UserId == userId)
                .Include(x => x.Group)
                .Select(x => x.Group)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }
    }
}
