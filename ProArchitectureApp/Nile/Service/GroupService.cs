// Nile.Service/GroupService.cs
using Nile.Entities;
using Nile.Repository;
using Nile.Repository.GroupMemberRepository;
using Nile.Service.Dtos;

namespace Nile.Service
{
    public class GroupService
    {
        private readonly IGroupRepository _groups;
        private readonly IGroupMemberRepository _members;
        private readonly IUserRepository _users;

        public GroupService(IGroupRepository groups, IGroupMemberRepository members, IUserRepository users)
        {
            _groups = groups;
            _members = members;
            _users = users;
        }

        public async Task<GroupDto> CreateAsync(CreateGroupRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                throw new ArgumentException("Name is required.");

            _ = await _users.GetByIdAsync(req.OwnerUserId) ?? throw new InvalidOperationException("Owner not found.");

            var group = new Group
            {
                GroupId = Guid.NewGuid(),
                Name = req.Name.Trim(),
                Description = (req.Description ?? string.Empty).Trim(),
                OwnerUserId = req.OwnerUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _groups.AddAsync(group);

            // owner becomes admin member
            var gm = new GroupMember
            {
                GroupMemberId = Guid.NewGuid(),
                GroupId = group.GroupId,
                UserId = req.OwnerUserId,
                Role = "admin",
                JoinedAt = DateTime.UtcNow
            };
            await _members.AddAsync(gm);

            await _groups.SaveChangesAsync(); // one SaveChanges for both (shared dbcontext)

            return Map(group, memberCount: 1);
        }

        public async Task<GroupDto?> GetAsync(Guid groupId)
        {
            var g = await _groups.GetWithMembersAsync(groupId);
            if (g == null) return null;
            var count = g.Members?.Count ?? 0;
            return Map(g, count);
        }

        public async Task<IReadOnlyList<GroupDto>> MyGroupsAsync(Guid userId)
        {
            var groups = await _members.GetGroupsForUserAsync(userId);
            var result = new List<GroupDto>(groups.Count);
            foreach (var g in groups)
            {
                var full = await _groups.GetWithMembersAsync(g.GroupId);
                result.Add(Map(g, full?.Members?.Count ?? 0));
            }
            return result;
        }

        public async Task<IReadOnlyList<GroupMemberDto>> GetMembersAsync(Guid groupId)
        {
            var list = await _members.GetMembersAsync(groupId);
            return list.Select(MapMember).ToList();
        }

        public async Task AddMemberAsync(AddMemberRequest req, Guid actingUserId)
        {
            // only admins can add
            if (!await _members.IsAdminAsync(req.GroupId, actingUserId))
                throw new InvalidOperationException("Only admins can add members.");

            if (await _members.IsMemberAsync(req.GroupId, req.UserId)) return;

            _ = await _groups.GetByIdAsync(req.GroupId) ?? throw new InvalidOperationException("Group not found.");
            _ = await _users.GetByIdAsync(req.UserId)  ?? throw new InvalidOperationException("User not found.");

            var gm = new GroupMember
            {
                GroupMemberId = Guid.NewGuid(),
                GroupId = req.GroupId,
                UserId = req.UserId,
                Role = string.IsNullOrWhiteSpace(req.Role) ? "member" : req.Role,
                JoinedAt = DateTime.UtcNow
            };
            await _members.AddAsync(gm);
            await _members.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(Guid groupId, Guid userId, Guid actingUserId)
        {
            if (!await _members.IsAdminAsync(groupId, actingUserId))
                throw new InvalidOperationException("Only admins can remove members.");

            await _members.RemoveAsync(groupId, userId);
            await _members.SaveChangesAsync();
        }

        public async Task ChangeRoleAsync(ChangeRoleRequest req, Guid actingUserId)
        {
            if (!await _members.IsAdminAsync(req.GroupId, actingUserId))
                throw new InvalidOperationException("Only admins can change roles.");

            await _members.ChangeRoleAsync(req.GroupId, req.UserId, req.Role);
            await _members.SaveChangesAsync();
        }

        private static GroupDto Map(Group g, int memberCount) => new()
        {
            GroupId = g.GroupId,
            OwnerUserId = g.OwnerUserId,
            Name = g.Name,
            Description = g.Description,
            CreatedAt = g.CreatedAt,
            MemberCount = memberCount
        };

        private static GroupMemberDto MapMember(GroupMember m) => new()
        {
            GroupMemberId = m.GroupMemberId,
            GroupId = m.GroupId,
            UserId = m.UserId,
            Role = m.Role,
            JoinedAt = m.JoinedAt,
            DisplayName = m.User?.DisplayName ?? "",
            AvatarUrl = m.User?.AvatarUrl
        };
    }
}
