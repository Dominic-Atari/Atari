using Microsoft.EntityFrameworkCore;
using Nile.Entities;

namespace Nile
{
    public class NileDbContext : DbContext
    {
        public NileDbContext(DbContextOptions<NileDbContext> options)
            : base(options)
        {
        }

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

            //
            // FRIEND RELATIONSHIP
            //
            modelBuilder.Entity<FriendRelationship>()
                .HasKey(fr => fr.Id);

            modelBuilder.Entity<FriendRelationship>()
                .HasOne(fr => fr.RequesterUser)
                .WithMany() // we haven't added SentFriendRequests to User yet
                .HasForeignKey(fr => fr.RequesterUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRelationship>()
                .HasOne(fr => fr.TargetUser)
                .WithMany() // we haven't added ReceivedFriendRequests to User yet
                .HasForeignKey(fr => fr.TargetUserId)
                .OnDelete(DeleteBehavior.Restrict);

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

            //
            // POSTLIKE
            //
            modelBuilder.Entity<PostLike>()
                .HasKey(l => l.PostLikeId);

            modelBuilder.Entity<PostLike>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

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
        }
    }
}
