using Microsoft.EntityFrameworkCore;
using Nile.Entities;

namespace Nile.Repository
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(NileDbContext db) : base(db) { }

        public async Task<IReadOnlyList<Notification>> GetRecentForUserAsync(Guid userId, int takeCount)
        {
            return await _db.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(takeCount)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var n = await _db.Notifications.FirstOrDefaultAsync(x => x.NotificationId == notificationId);
            if (n is null) return;
            n.IsRead = true;
            _db.Notifications.Update(n);
        }

        public async Task MarkAllAsReadAsync(Guid userId)
        {
            await _db.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsRead, true));
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _db.Notifications.AddAsync(notification);
        }
    }
}
