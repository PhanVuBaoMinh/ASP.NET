// File: Data/ApplicationDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhanVuBaoMinh.Models;

namespace PhanVuBaoMinh.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; } // Thêm DbSet cho Order
        public DbSet<OrderDetail> OrderDetails { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2); // Đã fix cảnh báo

            // Nếu Order có thuộc tính đặc biệt (như Total), có thể thêm cấu hình tương tự
            modelBuilder.Entity<Order>()
                .Property(o => o.Total)
                .HasPrecision(18, 2); // Đảm bảo Total của Order có độ chính xác

            base.OnModelCreating(modelBuilder);
        }
    }
}