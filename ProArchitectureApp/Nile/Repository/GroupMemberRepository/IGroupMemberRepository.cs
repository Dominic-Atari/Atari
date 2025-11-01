// Nile/Repository/GroupMemberRepository/IGroupMemberRepository.cs
using Nile.Entities;

namespace Nile.Repository.GroupMemberRepository
{
    public interface IGroupMemberRepository : IGenericRepository<GroupMember>
    {
        Task<bool> IsMemberAsync(Guid groupId, Guid userId);
        Task<bool> IsAdminAsync(Guid groupId, Guid userId);
        Task<GroupMember?> GetAsync(Guid groupId, Guid userId);

    new Task AddAsync(GroupMember member);
        Task RemoveAsync(Guid groupId, Guid userId);
        Task ChangeRoleAsync(Guid groupId, Guid userId, string role);

        Task<IReadOnlyList<GroupMember>> GetMembersAsync(Guid groupId);
        Task<IReadOnlyList<Group>> GetGroupsForUserAsync(Guid userId);
    }
}
