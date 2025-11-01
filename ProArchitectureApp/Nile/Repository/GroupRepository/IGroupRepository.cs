// Nile/Repository/IGroupRepository.cs
using Nile.Entities;

namespace Nile.Repository
{
    public interface IGroupRepository : IGenericRepository<Group>
    {
        Task<Group?> GetWithMembersAsync(Guid groupId);
        Task<IReadOnlyList<Group>> GetOwnedByAsync(Guid ownerUserId);
        Task<IReadOnlyList<Group>> SearchByNameAsync(string term, int take);
    }
}
