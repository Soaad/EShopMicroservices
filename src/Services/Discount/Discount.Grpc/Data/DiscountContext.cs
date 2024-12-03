using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext:DbContext
    {
      public  DbSet<Coupon> Coupons { set; get; } = default;
        public DiscountContext(DbContextOptions<DiscountContext> options):base(options)
        {
                
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon { Id=1,Amount=1500,Description="Iphone x with many features", ProductName="Iphon x"},
                  new Coupon { Id = 2, Amount = 1500, Description = "Iphone pro max released on 2020", ProductName = "Iphon 11 pro max" }
                );
        }
    }
}
