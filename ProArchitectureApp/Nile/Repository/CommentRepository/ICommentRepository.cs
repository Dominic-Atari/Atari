using Nile.Entities;

namespace Nile.Repository
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        /// <summary>
        /// Returns all top-level comments for a post (no replies yet),
        /// newest or oldest first depending on how you want to display.
        /// </summary>
        Task<IReadOnlyList<Comment>> GetCommentsForPostAsync(Guid postId, int skip, int take);

        /// <summary>
        /// Returns replies for a specific parent comment (threaded discussion).
        /// </summary>
        Task<IReadOnlyList<Comment>> GetRepliesForCommentAsync(Guid parentCommentId);

        /// <summary>
        /// Adds a new comment (parent or reply). Does not SaveChanges yet.
        /// </summary>
        Task AddCommentAsync(Comment comment);

        /// <summary>
        /// Soft-delete or hard-delete a comment (depending how you implement moderation).
        /// </summary>
        Task DeleteCommentAsync(Guid commentId, Guid requestingUserId);

        /// <summary>
        /// Returns a single comment with its author (User) loaded.
        /// Helpful for moderation or editing.
        /// </summary>
        Task<Comment?> GetCommentWithAuthorAsync(Guid commentId);
    }
}