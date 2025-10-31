using Microsoft.EntityFrameworkCore;
namespace Dominic.Net.Models
{
    public class DominicShopDbContext : DbContext
    {
        // Constructor to pass options to the base DbContext
        public DominicShopDbContext(DbContextOptions<DominicShopDbContext> options) : base(options)
        {
        }

        public DbSet<Pie> Pies { get; set; } // create Pies table in database
        public DbSet<Category> Categories { get; set; } // create Categories table in database
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; } // create ShoppingCartItems table in database
    }
}