// Nile.Service/CommentService.cs
using Nile.Entities;
using Nile.Repository;
using Nile.Service.Dtos;

namespace Nile.Service
{
    public class CommentService
    {
        private readonly ICommentRepository _comments;
        private readonly IPostRepository _posts;
        private readonly IUserRepository _users;

        public CommentService(ICommentRepository comments, IPostRepository posts, IUserRepository users)
        {
            _comments = comments;
            _posts = posts;
            _users = users;
        }

        public async Task<CommentDto> CreateAsync(CreateCommentRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.ContentText))
                throw new ArgumentException("Content cannot be empty.");

            // Ensure post and user exist
            _ = await _posts.GetByIdAsync(req.PostId) ?? throw new InvalidOperationException("Post not found.");
            var user = await _users.GetByIdAsync(req.UserId) ?? throw new InvalidOperationException("User not found.");

            if (req.ParentCommentId != null)
            {
                _ = await _comments.GetByIdAsync(req.ParentCommentId.Value)
                    ?? throw new InvalidOperationException("Parent comment not found.");
            }

            var entity = new Comment
            {
                CommentId = Guid.NewGuid(),
                PostId = req.PostId,
                UserId = req.UserId,
                ContentText = req.ContentText.Trim(),
                CreatedAt = DateTime.UtcNow,
                ParentCommentId = req.ParentCommentId
            };

            await _comments.AddCommentAsync(entity);
            await _comments.SaveChangesAsync();

            return new CommentDto
            {
                CommentId = entity.CommentId,
                PostId = entity.PostId,
                UserId = entity.UserId,
                AuthorDisplayName = user.DisplayName,
                AuthorAvatarUrl = user.AvatarUrl,
                ContentText = entity.ContentText,
                CreatedAt = entity.CreatedAt,
                ParentCommentId = entity.ParentCommentId
            };
        }

        public async Task<IReadOnlyList<CommentDto>> GetForPostAsync(Guid postId, int skip, int take)
        {
            var list = await _comments.GetCommentsForPostAsync(postId, skip, take);
            return list.Select(c => new CommentDto
            {
                CommentId = c.CommentId,
                PostId = c.PostId,
                UserId = c.UserId,
                AuthorDisplayName = c.User?.DisplayName ?? string.Empty,
                AuthorAvatarUrl = c.User?.AvatarUrl,
                ContentText = c.ContentText,
                CreatedAt = c.CreatedAt,
                ParentCommentId = c.ParentCommentId
            }).ToList();
        }

        public async Task<IReadOnlyList<CommentDto>> GetRepliesAsync(Guid parentCommentId)
        {
            var list = await _comments.GetRepliesForCommentAsync(parentCommentId);
            return list.Select(c => new CommentDto
            {
                CommentId = c.CommentId,
                PostId = c.PostId,
                UserId = c.UserId,
                AuthorDisplayName = c.User?.DisplayName ?? string.Empty,
                AuthorAvatarUrl = c.User?.AvatarUrl,
                ContentText = c.ContentText,
                CreatedAt = c.CreatedAt,
                ParentCommentId = c.ParentCommentId
            }).ToList();
        }

        public async Task DeleteAsync(Guid commentId, Guid requestingUserId)
        {
            await _comments.DeleteCommentAsync(commentId, requestingUserId);
            await _comments.SaveChangesAsync();
        }
    }
}
