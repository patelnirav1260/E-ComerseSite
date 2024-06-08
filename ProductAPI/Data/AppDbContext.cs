using ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<Product> products { get; set; }

    }
}
