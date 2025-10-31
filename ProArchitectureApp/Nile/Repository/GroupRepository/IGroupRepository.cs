using System.Text.RegularExpressions;
using Nile.Entities;

namespace Nile.Repository
{
    public interface IGroupRepository : IGenericRepository<GroupMember>
    {
        Task<bool> IsMemberAsync(Guid userId, Guid groupId);

        Task AddMemberAsync(GroupMember member);

        Task RemoveMemberAsync(Guid userId, Guid groupId);

        Task<GroupMember?> GetMembershipAsync(Guid userId, Guid groupId);
    }
}