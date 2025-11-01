using Nile.Entities;
using Nile.Repository;
using Nile.ResponseDTOs.Notifications;

namespace Nile.Service
{
    public class NotificationService
    {
        private readonly INotificationRepository _notifications;
        private readonly IUserRepository _users;

        public NotificationService(INotificationRepository notifications, IUserRepository users)
        {
            _notifications = notifications;
            _users = users;
        }

        public async Task<IReadOnlyList<NotificationDto>> GetRecentAsync(Guid userId, int take = 20)
        {
            _ = await _users.GetByIdAsync(userId) ?? throw new InvalidOperationException("User not found.");
            var list = await _notifications.GetRecentForUserAsync(userId, take);
            return list.Select(Map).ToList();
        }

        // Nile.Service/NotificationService.cs
        public async Task<NotificationDto> CreateAsync(CreateNotificationRequest req)
        {
            if (req.UserId == Guid.Empty) throw new ArgumentException("UserId required.");
            if (string.IsNullOrWhiteSpace(req.Type)) throw new ArgumentException("Type required.");
            if (string.IsNullOrWhiteSpace(req.Message)) throw new ArgumentException("Message required.");

            var recipient = await _users.GetByIdAsync(req.UserId)
                            ?? throw new InvalidOperationException("Recipient user not found.");

            User? actor = null;
            if (req.ActorUserId.HasValue)
            {
                if (req.ActorUserId.Value == req.UserId)
                    throw new ArgumentException("Actor and recipient cannot be the same for actor-generated notifications.");

                actor = await _users.GetByIdAsync(req.ActorUserId.Value)
                      ?? throw new InvalidOperationException("Actor user not found.");
            }

            var n = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = req.UserId,
                ActorUserId = req.ActorUserId,
                Type = req.Type,
                Message = req.Message,
                ReferenceId = req.ReferenceId,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notifications.AddNotificationAsync(n);
            await _notifications.SaveChangesAsync();

            return new NotificationDto
            {
                NotificationId = n.NotificationId,
                UserId = n.UserId,
                ActorUserId = n.ActorUserId,
                ActorDisplayName = actor?.DisplayName,
                ActorAvatarUrl = actor?.AvatarUrl,
                Type = n.Type,
                Message = n.Message,
                ReferenceId = n.ReferenceId,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            };
        }


        public async Task MarkReadAsync(Guid notificationId)
        {
            await _notifications.MarkAsReadAsync(notificationId);
            await _notifications.SaveChangesAsync();
        }

        public async Task MarkAllReadAsync(Guid userId)
        {
            _ = await _users.GetByIdAsync(userId) ?? throw new InvalidOperationException("User not found.");
            await _notifications.MarkAllAsReadAsync(userId);
            await _notifications.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid notificationId)
        {
            var entity = await _notifications.GetByIdAsync(notificationId);
            if (entity is null) return;
            _notifications.Remove(entity);
            await _notifications.SaveChangesAsync();
        }

        private static NotificationDto Map(Notification n) => new()
        {
            NotificationId = n.NotificationId,
            UserId = n.UserId,
            Type = n.Type,
            Message = n.Message,
            ReferenceId = n.ReferenceId,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt
        };
    }
}
