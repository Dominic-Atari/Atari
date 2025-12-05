using Microsoft.EntityFrameworkCore;
using Nile.Common;
using Nile.Common.Extensions;
using Nile.Database.Entities;

namespace Nile.Database.DataContracts;

/// <summary>
/// Main database context for the Nile application.
/// </summary>
public class DatabaseContext : DatabaseContextBase<DatabaseContext>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfigUtility config)
        : base(options, config)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ignore unmapped properties
        modelBuilder.Entity<User>()
            .Ignore(u => u.PasswordHash);

        // Relationships with Cascade Delete Strategy

        // User -> Roles (Restrict: Don't delete user if they have roles)
        modelBuilder.Entity<UserRole>()
       .HasKey(ur => new { ur.UserId, ur.RoleId });


        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserRole -> Role (Restrict: Don't delete role if it's assigned to users)
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        // User -> Posts (Restrict: Don't delete user if they have posts)
        modelBuilder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Post -> Comments (Cascade: Delete all comments when post is deleted)
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // User -> Comments (Restrict: Don't delete user if they have comments)
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Comment -> Replies (Cascade: Delete all replies when parent comment is deleted)
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Post -> Likes (Cascade: Delete all likes when post is deleted)
        modelBuilder.Entity<Like>()
            .HasOne(l => l.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // User -> Likes (Restrict: Don't delete user if they have likes)
        modelBuilder.Entity<Like>()
            .HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserRelationship -> Follower (Restrict to avoid accidental hard delete cascades)
        modelBuilder.Entity<UserRelationship>()
            .HasOne(ur => ur.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(ur => ur.FollowerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserRelationship -> Following (Restrict: Prevent deletion cycles)
        modelBuilder.Entity<UserRelationship>()
            .HasOne(ur => ur.Following)
            .WithMany(u => u.Followers)
            .HasForeignKey(ur => ur.FollowingUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Notification -> User (Restrict to align with soft-delete-only)
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Notification -> Actor (SetNull: Keep notification but clear actor when actor user is deleted)
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Actor)
            .WithMany(u => u.TriggeredNotifications)
            .HasForeignKey(n => n.ActorUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Global Query Filters - Automatically filter out soft-deleted records
        // Use .IgnoreQueryFilters() to override when needed
        modelBuilder.Entity<User>()
            .HasQueryFilter(x => !x.IsDeleted);

        modelBuilder.Entity<Post>()
            .HasQueryFilter(x => !x.IsDeleted);

        modelBuilder.Entity<Comment>()
            .HasQueryFilter(x => !x.IsDeleted);
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<UserRelationship> UserRelationships => Set<UserRelationship>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<School> Schools => Set<School>();
    public DbSet<Passwords> Passwords => Set<Passwords>();
    public DbSet<EmailConfirmations> EmailConfirmations => Set<EmailConfirmations>();
    public DbSet<ResetPasswordTokens> ResetPasswordTokens => Set<ResetPasswordTokens>();
}
