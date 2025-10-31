using Nile.Entities;

namespace Nile.Repository
{
    public interface IPostLikeRepository : IGenericRepository<PostLike>
    {
        Task AddLikeAsync(PostLike like);

        Task RemoveLikeAsync(Guid postId, Guid userId);

        Task<bool> HasUserLikedAsync(Guid postId, Guid userId);

        Task<int> GetLikeCountAsync(Guid postId);

        Task<IReadOnlyList<User>> GetRecentLikersAsync(Guid postId, int takeCount);

    }
}