using Microsoft.EntityFrameworkCore;
using Nile.Repository;
using Nile.Repository.PostRepository;
using Nile.Repository.UserRepository;
using Nile.Service;

namespace Nile
{
    public static class Project
    {
        public static IServiceCollection AddNile(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("NileDb");

            services.AddDbContext<NileDbContext>(options =>
            {
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                );
            });

            // generic repo
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // repositories
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostLikeRepository, PostLikeRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<Nile.Repository.GroupMemberRepository.IGroupMemberRepository, Nile.Repository.GroupMemberRepository.GroupMemberRepository>();
            services.AddScoped<IFriendRelationshipRepository, FriendRelationshipRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();


            // services
            services.AddScoped<PostService>();
            services.AddScoped<UserService>();
            services.AddScoped<PostLikeService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<MessageService>();
            services.AddScoped<GroupService>();
            services.AddScoped<FriendService>();
            services.AddScoped<CommentService>();
            
            return services;
        }
    }
}
