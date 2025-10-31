using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nile.Entities;
using Nile.Repository;

namespace Nile.Service
{
    public class PostService
    {
        private readonly IPostRepository _posts;
        private readonly IUserRepository _users;

        public PostService(IPostRepository posts, IUserRepository users)
        {
            _posts = posts;
            _users = users;
        }

        public Task<IReadOnlyList<Post>> GetRecentPostsAsync(int take = 5)
        {
            return _posts.GetRecentPostsAsync(take);
        }

        public Task<IReadOnlyList<Post>> GetFeedForUserAsync(Guid userId, int skip = 0, int take = 20)
        {
            return _posts.GetFeedForUserAsync(userId, skip, take);
        }

        public Task<Post?> GetPostWithDetailsAsync(Guid postId)
        {
            return _posts.GetPostWithDetailsAsync(postId);
        }

        public async Task<Post> CreatePostAsync(Guid userId, string contentText, string? mediaUrl)
        {
            // 1. validate content
            if (string.IsNullOrWhiteSpace(contentText) && string.IsNullOrWhiteSpace(mediaUrl))
                throw new ArgumentException("Post must have text or media.");

            // 2. validate user exists
            var user = await _users.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User does not exist.");

            // 3. create post
            var post = new Post
            {
                PostId = Guid.NewGuid(),
                UserId = userId,
                ContentText = contentText,
                MediaUrl = mediaUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _posts.AddAsync(post);
            await _posts.SaveChangesAsync();

            return post;
        }

        public async Task LikePostAsync(Guid postId, Guid userId)
        {
            // (optional) ensure user exists before liking
            var user = await _users.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User does not exist.");

            bool alreadyLiked = await _posts.HasUserLikedAsync(postId, userId);
            if (alreadyLiked)
                return;

            var like = new PostLike
            {
                PostLikeId = Guid.NewGuid(),
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _posts.AddLikeAsync(like);
            await _posts.SaveChangesAsync();
        }

        public async Task UnlikePostAsync(Guid postId, Guid userId)
        {
            await _posts.RemoveLikeAsync(postId, userId);
            await _posts.SaveChangesAsync();
        }

        public async Task<Comment> AddCommentAsync(Guid postId, Guid userId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Comment text is required.");

            var user = await _users.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User does not exist.");

            var comment = new Comment
            {
                CommentId = Guid.NewGuid(),
                PostId = postId,
                UserId = userId,
                ContentText = text,
                CreatedAt = DateTime.UtcNow
            };

            await _posts.AddCommentAsync(comment);
            await _posts.SaveChangesAsync();

            return comment;
        }
    }
}
