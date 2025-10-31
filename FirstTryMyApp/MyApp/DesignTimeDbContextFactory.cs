using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MyApp.Data;

namespace MyApp
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            
            // Use the correct connection string for design time
            optionsBuilder.UseMySql(
                "server=localhost;port=3306;database=webapp;user=webapp;password=webapp;SslMode=none;",
                ServerVersion.Parse("8.0.33-mysql"));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
