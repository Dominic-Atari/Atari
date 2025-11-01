// Nile/Repository/CommentRepository.cs
using Microsoft.EntityFrameworkCore;
using Nile.Entities;

namespace Nile.Repository
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(NileDbContext db) : base(db) { }

        public async Task<IReadOnlyList<Comment>> GetCommentsForPostAsync(Guid postId, int skip, int take)
        {
            return await _db.Comments
                .Where(c => c.PostId == postId && c.ParentCommentId == null)
                .Include(c => c.User)
                .OrderBy(c => c.CreatedAt) // or Desc for newest-first
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Comment>> GetRepliesForCommentAsync(Guid parentCommentId)
        {
            return await _db.Comments
                .Where(c => c.ParentCommentId == parentCommentId)
                .Include(c => c.User)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _db.Comments.AddAsync(comment);
        }

        // Hard delete; if you prefer soft-delete add IsDeleted and update instead
        public async Task DeleteCommentAsync(Guid commentId, Guid requestingUserId)
        {
            var comment = await _db.Comments
                .FirstOrDefaultAsync(c => c.CommentId == commentId);

            if (comment == null) return;

            // basic rule: only the author could delete (tighten as needed for admins/mods)
            if (comment.UserId != requestingUserId)
                throw new InvalidOperationException("You can delete only your own comment.");

            _db.Comments.Remove(comment);
        }

        public async Task<Comment?> GetCommentWithAuthorAsync(Guid commentId)
        {
            return await _db.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CommentId == commentId);
        }
    }
}
