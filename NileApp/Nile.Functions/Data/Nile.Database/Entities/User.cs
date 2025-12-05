using Nile.Common;

namespace Nile.Database.Entities;

public class User
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Bio { get; set; }
    public string? Location { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? Gender { get; set; }
    public bool IsActive { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation properties
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<UserRelationship> Following { get; set; } = new List<UserRelationship>();
    public ICollection<UserRelationship> Followers { get; set; } = new List<UserRelationship>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Notification> TriggeredNotifications { get; set; } = new List<Notification>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
