// Nile.Service/PostLikeRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nile.Entities;
using Nile.Repository;

namespace Nile.Repository
{
    public class PostLikeRepository : GenericRepository<PostLike>, IPostLikeRepository
    {
        public PostLikeRepository(NileDbContext db) : base(db) { }

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

        public async Task<bool> HasUserLikedAsync(Guid postId, Guid userId)
        {
            return await _db.PostLikes.AnyAsync(l => l.PostId == postId && l.UserId == userId);
        }

        public async Task<int> GetLikeCountAsync(Guid postId)
        {
            return await _db.PostLikes.CountAsync(l => l.PostId == postId);
        }

        public async Task<IReadOnlyList<User>> GetRecentLikersAsync(Guid postId, int takeCount)
        {
            return await _db.PostLikes
                .Where(l => l.PostId == postId)
                .OrderByDescending(l => l.CreatedAt)
                .Take(takeCount)
                .Select(l => l.User)
                .ToListAsync();
        }
    }
}