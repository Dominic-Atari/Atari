using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nile.Entities;
using Nile.Repository;

namespace Nile.Service
{
    public class UserService
    {
        private readonly IUserRepository _users;

        public UserService(IUserRepository users)
        {
            _users = users;
        }

        // Create/register new user
        public async Task<User> CreateUserAsync(string displayName, string email, string passwordHash, string? avatarUrl)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("Display name is required.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash is required.");

            // ensure email not already in use
            var existing = await _users.GetByEmailAsync(email);
            if (existing != null)
                throw new InvalidOperationException("Email already in use.");

            var user = new User
            {
                UserId = Guid.NewGuid(),
                DisplayName = displayName,
                Email = email,
                PasswordHash = passwordHash,
                AvatarUrl = avatarUrl ?? string.Empty,
                CreatedAt = DateTime.UtcNow,
                IsOnline = false
            };

            await _users.AddAsync(user);
            await _users.SaveChangesAsync();

            return user;
        }

        // get a profile by id
        public Task<User?> GetUserByIdAsync(Guid userId)
        {
            return _users.GetByIdAsync(userId);
        }

        // get their accepted friends
        public Task<IReadOnlyList<User>> GetFriendsAsync(Guid userId)
        {
            return _users.GetFriendsAsync(userId);
        }

        // get pending friend requests
        public Task<IReadOnlyList<FriendRelationship>> GetIncomingFriendRequestsAsync(Guid userId)
        {
            return _users.GetIncomingFriendRequestsAsync(userId);
        }

        // are these two friends already?
        public Task<bool> AreFriendsAsync(Guid a, Guid b)
        {
            return _users.AreFriendsAsync(a, b);
        }
    }
}
