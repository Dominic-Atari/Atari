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

            // services
            services.AddScoped<PostService>();
            services.AddScoped<UserService>();

            return services;
        }
    }
}
