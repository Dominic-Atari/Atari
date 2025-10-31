using System;
using System.Collections.Generic;
using Nile.Entities;
namespace Nile.Entities
{
    public class User
    {
        public Guid UserId { get; set; }

        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public bool IsOnline { get; set; }

        // Posts authored by this user
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        // Likes this user made
        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();

        // Comments this user wrote
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // Messages
        public ICollection<Message> MessagesSent { get; set; } = new List<Message>();
        public ICollection<Message> MessagesReceived { get; set; } = new List<Message>();

        // Notifications delivered to this user
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        // Groups this user is a member of
        public ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();

        // Groups this user owns
        public ICollection<Group> OwnedGroups { get; set; } = new List<Group>();

        // Friend relationships involving this user:
        // We'll map both sides (sent/received) in the DbContext using .WithMany()
        // without forcing navigation collections here yet.
        // We can add SentFriendRequests / ReceivedFriendRequests later if we want.
    }
}
