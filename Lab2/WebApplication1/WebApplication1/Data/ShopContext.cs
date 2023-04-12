using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }

        public DbSet<Shop> Shop { get; set; }
    }
}