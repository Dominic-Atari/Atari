using Microsoft.EntityFrameworkCore;
using MyApp;
using MyApp.Model;

namespace MyApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
    }
}