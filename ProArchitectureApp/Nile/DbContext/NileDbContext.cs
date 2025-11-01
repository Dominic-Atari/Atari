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
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();

            //
            // ==========================
// FRIEND RELATIONSHIP
// ==========================
modelBuilder.Entity<FriendRelationship>()
    .HasKey(fr => fr.Id);

modelBuilder.Entity<FriendRelationship>()
    .HasOne(fr => fr.RequesterUser)
    .WithMany() // add User.SentFriendRequests later if desired
    .HasForeignKey(fr => fr.RequesterUserId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<FriendRelationship>()
    .HasOne(fr => fr.TargetUser)
    .WithMany() // add User.ReceivedFriendRequests later if desired
    .HasForeignKey(fr => fr.TargetUserId)
    .OnDelete(DeleteBehavior.Restrict);

// One directed row per pair
modelBuilder.Entity<FriendRelationship>()
    .HasIndex(fr => new { fr.RequesterUserId, fr.TargetUserId })
    .IsUnique();

// No self-requests (CHECK enforced on SQL Server/Postgres/MySQL 8+)
modelBuilder.Entity<FriendRelationship>()
    .ToTable(t => t.HasCheckConstraint("CK_Friend_NoSelf", "RequesterUserId <> TargetUserId"));

// ---------- Helpful indexes ----------
modelBuilder.Entity<FriendRelationship>()
    .HasIndex(fr => new { fr.TargetUserId, fr.Status, fr.CreatedAt }); // pending inbox, newest first

modelBuilder.Entity<FriendRelationship>()
    .HasIndex(fr => new { fr.RequesterUserId, fr.Status });            // accepted scans (outbound)

modelBuilder.Entity<FriendRelationship>()
    .HasIndex(fr => new { fr.TargetUserId, fr.Status });               // accepted scans (inbound)

// (Optional) constrain allowed status values at DB level
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
            modelBuilder.Entity<Post>()
                .HasIndex(p => p.UserId);
            modelBuilder.Entity<Post>()
                .HasIndex(p => p.CreatedAt);

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
            modelBuilder.Entity<PostLike>()
                .HasIndex(pl => pl.UserId);

            //
            // COMMENT
            //
            // COMMENT
modelBuilder.Entity<Comment>()
    .HasKey(c => c.CommentId);

modelBuilder.Entity<Comment>()
    .HasOne(c => c.Post)
    .WithMany(p => p.Comments)
    .HasForeignKey(c => c.PostId)
    .OnDelete(DeleteBehavior.Cascade);

modelBuilder.Entity<Comment>()
    .HasOne(c => c.User)
    .WithMany(u => u.Comments)
    .HasForeignKey(c => c.UserId)
    .OnDelete(DeleteBehavior.Restrict);

// self-reference for replies
modelBuilder.Entity<Comment>()
    .HasOne(c => c.ParentComment)
    .WithMany(c => c.Replies)
    .HasForeignKey(c => c.ParentCommentId)
    .OnDelete(DeleteBehavior.Restrict);


// helpful indexes
modelBuilder.Entity<Comment>().HasIndex(c => new { c.PostId, c.CreatedAt });
modelBuilder.Entity<Comment>().HasIndex(c => c.ParentCommentId);

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
            modelBuilder.Entity<Message>()
                .HasIndex(m => new { m.SenderUserId, m.CreatedAt });
            modelBuilder.Entity<Message>()
                .HasIndex(m => new { m.RecipientUserId, m.CreatedAt });
            modelBuilder.Entity<Message>()
                .HasIndex(m => m.IsRead);

            //
            // NOTIFICATION
            //
           // In NileDbContext.OnModelCreating
            modelBuilder.Entity<Notification>(entity =>
            {
            entity.HasKey(n => n.NotificationId);

    // Recipient
            entity.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

    // Actor/Sender (nullable)
    entity.HasOne(n => n.ActorUser)
          .WithMany() // you can add User.SentNotifications later if you want
          .HasForeignKey(n => n.ActorUserId)
          .OnDelete(DeleteBehavior.Restrict);

    entity.Property(n => n.Type).IsRequired().HasMaxLength(50);
    entity.Property(n => n.Message).IsRequired().HasMaxLength(500);
    entity.Property(n => n.ReferenceId).HasMaxLength(200);

    // Helpful index for inbox queries
    entity.HasIndex(n => new { n.UserId, n.IsRead, n.CreatedAt });

    // Optional: prevent “notify yourself” for actor-generated events
    // (only if ActorUserId is present)
    // entity.HasCheckConstraint("CK_Notification_ActorNotRecipient",
    //     "(ActorUserId IS NULL) OR (ActorUserId <> UserId)");
});


            // ==========================
// GROUP
// ==========================
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

// Helpful index for owner lookups
modelBuilder.Entity<Group>()
    .HasIndex(g => g.OwnerUserId);

// ==========================
// GROUP MEMBER
// ==========================
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

// Fast queries: "my groups" and roster views
modelBuilder.Entity<GroupMember>()
    .HasIndex(gm => gm.UserId);
modelBuilder.Entity<GroupMember>()
    .HasIndex(gm => new { gm.GroupId, gm.Role, gm.JoinedAt });

// (Optional) constrain Role values
// NOTE: MySQL 8+ enforces CHECK constraints; older MySQL ignores them.
modelBuilder.Entity<GroupMember>()
    .ToTable(t => t.HasCheckConstraint("CK_GroupMember_Role", "Role IN ('admin','mod','member')"));

        }
    }
}
