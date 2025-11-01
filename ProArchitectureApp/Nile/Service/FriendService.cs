using Nile.Entities;
using Nile.Repository;
using Nile.ResponseDTOs.FriendRequestDto;
using Nile.Service.Dtos;

namespace Nile.Service
{
    public class FriendService
    {
        private readonly IFriendRelationshipRepository _friends;
        private readonly IUserRepository _users;

        public FriendService(IFriendRelationshipRepository friends, IUserRepository users)
        {
            _friends = friends;
            _users = users;
        }

        public async Task<FriendRequestDto> SendAsync(SendFriendRequestDto req)
        {
            if (req.RequesterUserId == Guid.Empty || req.TargetUserId == Guid.Empty)
                throw new ArgumentException("Both requester and target are required.");
            if (req.RequesterUserId == req.TargetUserId)
                throw new ArgumentException("Cannot send a friend request to yourself.");

            var requester = await _users.GetByIdAsync(req.RequesterUserId)
                           ?? throw new InvalidOperationException("Requester not found.");
            _ = await _users.GetByIdAsync(req.TargetUserId)
                ?? throw new InvalidOperationException("Target not found.");

            // Prevent duplicate pending requests in either direction
            var dup = await _friends.GetRelationshipAsync(req.RequesterUserId, req.TargetUserId)
                   ?? await _friends.GetRelationshipAsync(req.TargetUserId, req.RequesterUserId);

            if (dup is not null && dup.Status is "Pending" or "Accepted")
                throw new InvalidOperationException("Friend request already exists or you are already friends.");

            var rel = new FriendRelationship
            {
                Id = Guid.NewGuid(),
                RequesterUserId = req.RequesterUserId,
                TargetUserId = req.TargetUserId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _friends.CreateRequestAsync(rel);
            await _friends.SaveChangesAsync();

            return new FriendRequestDto
            {
                RelationshipId = rel.Id,
                RequesterUserId = rel.RequesterUserId,
                TargetUserId = rel.TargetUserId,
                Status = rel.Status,
                CreatedAt = rel.CreatedAt,
                RequesterDisplayName = requester.DisplayName,
                RequesterAvatarUrl = requester.AvatarUrl
            };
        }

        public async Task AcceptAsync(Guid relationshipId, Guid actingUserId)
        {
            // Enforce "only the target can accept"
            var existing = await _friends.GetByIdAsync(relationshipId)
                           ?? throw new InvalidOperationException("Friend request not found.");

            if (existing.Status != "Pending")
                throw new InvalidOperationException("Request is not pending.");

            if (existing.TargetUserId != actingUserId)
                throw new InvalidOperationException("Only the target user can accept this request.");

            await _friends.AcceptRequestAsync(relationshipId);
            await _friends.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<FriendRequestDto>> GetPendingAsync(Guid userId)
        {
            var list = await _friends.GetPendingRequestsForUserAsync(userId);
            return list.Select(fr => new FriendRequestDto
            {
                RelationshipId = fr.Id,
                RequesterUserId = fr.RequesterUserId,
                TargetUserId = fr.TargetUserId,
                Status = fr.Status,
                CreatedAt = fr.CreatedAt,
                AcceptedAt = fr.AcceptedAt,
                RequesterDisplayName = fr.RequesterUser?.DisplayName,
                RequesterAvatarUrl = fr.RequesterUser?.AvatarUrl
            }).ToList();
        }

        public async Task<IReadOnlyList<FriendDto>> GetFriendsAsync(Guid userId)
        {
            var users = await _friends.GetAcceptedFriendsForUserAsync(userId);
            return users.Select(u => new FriendDto
            {
                UserId = u.UserId,
                DisplayName = u.DisplayName,
                AvatarUrl = u.AvatarUrl
            }).ToList();
        }
    }
}
