using Microsoft.EntityFrameworkCore;
using Nile.Entities;

namespace Nile
{
    public class NileDbContext : DbContext
    {
        public NileDbContext(DbContextOptions<NileDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Post> Posts { get; set; } = default!;
        public DbSet<PostLike> PostLikes { get; set; } = default!;
        public DbSet<Comment> Comments { get; set; } = default!;
        public DbSet<FriendRelationship> FriendRelationships { get; set; } = default!;
        public DbSet<Message> Messages { get; set; } = default!;
        public DbSet<Notification> Notifications { get; set; } = default!;
        public DbSet<Group> Groups { get; set; } = default!;
        public DbSet<GroupMember> GroupMembers { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //
            // USER
            //
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.MessagesSent)
                .WithOne(m => m.SenderUser)
                .HasForeignKey(m => m.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.MessagesReceived)
                .WithOne(m => m.RecipientUser)
                .HasForeignKey(m => m.RecipientUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.GroupMemberships)
                .WithOne(gm => gm.User)
                .HasForeignKey(gm => gm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.OwnedGroups)
                .WithOne(g => g.OwnerUser)
                .HasForeignKey(g => g.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Helpful indexes
            //modelBuilder.Entity<User>().HasIndex(u => u.Email);
            // If you want uniqueness at DB level: uncomment next line
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            //
            // FRIEND RELATIONSHIP
            //
            modelBuilder.Entity<FriendRelationship>()
                .HasKey(fr => fr.Id);

            modelBuilder.Entity<FriendRelationship>()
                .HasOne(fr => fr.RequesterUser)
                .WithMany() // add User.SentFriendRequests if/when you want
                .HasForeignKey(fr => fr.RequesterUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRelationship>()
                .HasOne(fr => fr.TargetUser)
                .WithMany() // add User.ReceivedFriendRequests if/when you want
                .HasForeignKey(fr => fr.TargetUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One directed row per pair, and block self-friendship
            modelBuilder.Entity<FriendRelationship>()
                .HasIndex(fr => new { fr.RequesterUserId, fr.TargetUserId })
                .IsUnique();

            modelBuilder.Entity<FriendRelationship>()
                .HasCheckConstraint("CK_Friend_NoSelf", "RequesterUserId <> TargetUserId");

            // Optional: constrain status values if you use a string status field
            // modelBuilder.Entity<FriendRelationship>()
            //     .HasCheckConstraint("CK_Friend_Status", "Status IN ('Pending','Accepted','Blocked')");

            //
            // POST
            //
            modelBuilder.Entity<Post>()
                .HasKey(p => p.PostId);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Likes)
                .WithOne(l => l.Post)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Helpful indexes
            modelBuilder.Entity<Post>().HasIndex(p => p.UserId);
            modelBuilder.Entity<Post>().HasIndex(p => p.CreatedAt);

            //
            // POSTLIKE
            //
            modelBuilder.Entity<PostLike>()
                .HasKey(pl => pl.PostLikeId);

            // Exactly one like per (Post, User)
            modelBuilder.Entity<PostLike>()
                .HasIndex(pl => new { pl.PostId, pl.UserId })
                .IsUnique();

            modelBuilder.Entity<PostLike>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Helpful index for "what did this user like?"
            modelBuilder.Entity<PostLike>().HasIndex(pl => pl.UserId);

            //
            // COMMENT
            //
            modelBuilder.Entity<Comment>()
                .HasKey(c => c.CommentId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Optional threaded replies (requires Comment.ParentCommentId in your entity)
            // modelBuilder.Entity<Comment>()
            //     .HasOne<Comment>()
            //     .WithMany()
            //     .HasForeignKey(c => c.ParentCommentId)
            //     .OnDelete(DeleteBehavior.Restrict);

            // Helpful indexes
            modelBuilder.Entity<Comment>().HasIndex(c => c.PostId);
            modelBuilder.Entity<Comment>().HasIndex(c => new { c.PostId, c.CreatedAt });

            //
            // MESSAGE
            //
            modelBuilder.Entity<Message>()
                .HasKey(m => m.MessageId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.SenderUser)
                .WithMany(u => u.MessagesSent)
                .HasForeignKey(m => m.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.RecipientUser)
                .WithMany(u => u.MessagesReceived)
                .HasForeignKey(m => m.RecipientUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Helpful indexes
            modelBuilder.Entity<Message>().HasIndex(m => new { m.SenderUserId, m.CreatedAt });
            modelBuilder.Entity<Message>().HasIndex(m => new { m.RecipientUserId, m.CreatedAt });

            //
            // NOTIFICATION
            //
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.NotificationId);

                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(n => n.Type)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(n => n.Message)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(n => n.ReferenceId)
                      .HasMaxLength(200);

                // Optional: fast lookups by user & read status
                entity.HasIndex(n => new { n.UserId, n.IsRead, n.CreatedAt });
            });

            //
            // GROUP
            //
            modelBuilder.Entity<Group>()
                .HasKey(g => g.GroupId);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.OwnerUser)
                .WithMany(u => u.OwnedGroups)
                .HasForeignKey(g => g.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Members)
                .WithOne(m => m.Group)
                .HasForeignKey(m => m.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // Helpful indexes
            modelBuilder.Entity<Group>().HasIndex(g => g.OwnerUserId);

            //
            // GROUPMEMBER
            //
            modelBuilder.Entity<GroupMember>()
                .HasKey(gm => gm.GroupMemberId);

            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.User)
                .WithMany(u => u.GroupMemberships)
                .HasForeignKey(gm => gm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(gm => gm.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // One membership per user per group
            modelBuilder.Entity<GroupMember>()
                .HasIndex(gm => new { gm.GroupId, gm.UserId })
                .IsUnique();
        }
    }
}
