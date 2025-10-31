using Nile.Entities;

namespace Nile.Repository
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<IReadOnlyList<Post>> GetRecentPostsAsync(int takeCount);

        Task<IReadOnlyList<Post>> GetPostsByUserAsync(Guid userId, int skip, int take);

        Task<IReadOnlyList<Post>> GetFeedForUserAsync(Guid userId, int skip, int take);

        Task<Post?> GetPostWithDetailsAsync(Guid postId);

        Task AddLikeAsync(PostLike like);
        Task RemoveLikeAsync(Guid postId, Guid userId);
        Task<bool> HasUserLikedAsync(Guid postId, Guid userId);
        Task<int> GetLikeCountAsync(Guid postId);

        Task AddCommentAsync(Comment comment);
    }
}
