// Nile.Service/PostLikeService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nile.Entities;
using Nile.Repository;

namespace Nile.Service
{
    public class PostLikeService
    {
        private readonly IPostLikeRepository _likes;
        private readonly IPostRepository _posts;
        private readonly IUserRepository _users;

        public PostLikeService(IPostLikeRepository likes, IPostRepository posts, IUserRepository users)
        {
            _likes = likes;
            _posts = posts;
            _users = users;
        }

        public async Task LikeAsync(Guid postId, Guid userId)
        {
            // validate user & post exist
            _ = await _users.GetByIdAsync(userId) ?? throw new InvalidOperationException("User does not exist.");
            _ = await _posts.GetByIdAsync(postId) ?? throw new InvalidOperationException("Post does not exist.");

            // idempotent: if already liked, no-op
            if (await _likes.HasUserLikedAsync(postId, userId)) return;

            var like = new PostLike
            {
                PostLikeId = Guid.NewGuid(),
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _likes.AddLikeAsync(like);
            await _likes.SaveChangesAsync();
        }

        public async Task UnlikeAsync(Guid postId, Guid userId)
        {
            await _likes.RemoveLikeAsync(postId, userId);
            await _likes.SaveChangesAsync();
        }

        public Task<int> CountAsync(Guid postId) => _likes.GetLikeCountAsync(postId);

        public Task<IReadOnlyList<User>> RecentLikersAsync(Guid postId, int take = 10)
            => _likes.GetRecentLikersAsync(postId, take);
    }
}
