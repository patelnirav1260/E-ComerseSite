using CoupenAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoupenAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<Coupon> coupons { get; set; }

    }
}
