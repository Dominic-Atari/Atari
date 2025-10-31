using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nile;
using Nile.Entities;
using Nile.Repository;

namespace Nile.Repository.PostRepository
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(NileDbContext db) : base(db)
        {
        }

        // --- Feed / read ---

        public async Task<IReadOnlyList<Post>> GetRecentPostsAsync(int takeCount)
        {
            return await _db.Posts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .Take(takeCount)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Post>> GetFeedForUserAsync(Guid userId, int skip, int take)
        {
            // v1 feed = just this user's own posts.
            return await _db.Posts
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(skip)
                .Take(take)
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .ToListAsync();
        }

        public async Task<Post?> GetPostWithDetailsAsync(Guid postId)
        {
            return await _db.Posts
                .Where(p => p.PostId == postId)
                .Include(p => p.User)
                .Include(p => p.Likes)
                    .ThenInclude(l => l.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync();
        }

        // --- Likes ---

        public async Task<bool> HasUserLikedAsync(Guid postId, Guid userId)
        {
            return await _db.PostLikes
                .AnyAsync(l => l.PostId == postId && l.UserId == userId);
        }

        public async Task AddLikeAsync(PostLike like)
        {
            await _db.PostLikes.AddAsync(like);
        }

        public async Task RemoveLikeAsync(Guid postId, Guid userId)
        {
            var existing = await _db.PostLikes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            if (existing != null)
            {
                _db.PostLikes.Remove(existing);
            }
        }

        // --- Comments ---

        public async Task AddCommentAsync(Comment comment)
        {
            await _db.Comments.AddAsync(comment);
        }

        public Task<IReadOnlyList<Post>> GetPostsByUserAsync(Guid userId, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetLikeCountAsync(Guid postId)
        {
            throw new NotImplementedException();
        }
    }
}
