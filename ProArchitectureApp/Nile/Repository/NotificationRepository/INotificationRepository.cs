using Nile.Entities;

namespace Nile.Repository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
       // Get recent notifications for the bell dropdown
        Task<IReadOnlyList<Notification>> GetRecentForUserAsync(Guid userId, int takeCount);

        // Mark single notification as read
        Task MarkAsReadAsync(Guid notificationId);

        // Mark all as read
        Task MarkAllAsReadAsync(Guid userId);

        // Add new notification (like "X liked your post")
        Task AddNotificationAsync(Notification notification); 
    }
}