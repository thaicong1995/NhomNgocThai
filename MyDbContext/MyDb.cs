using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.VNpay;

namespace WebApi.MyDbContext
{
    public class MyDb : DbContext
    {
        public MyDb(DbContextOptions options) : base(options) 
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<AcessToken> AccessTokens { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Revenue> Revenues { get; set; }
        public DbSet<Discounts> Discounts { get; set; }
        public DbSet<DiscountUsage> DiscountUsages { get; set; }
        public DbSet<AddCardATM> AddCardATM { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u._userStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Shop>()
               .Property(u => u._shopStatus)
               .HasConversion<string>();

            modelBuilder.Entity<Product>()
               .Property(u => u._productStatus)
               .HasConversion<string>();

            modelBuilder.Entity<Product>()
                 .Property(p => p.Price)
                 .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Wallet>()
               .Property(p => p.Monney)
               .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Revenue>()
              .Property(p => p.Monney_Reveneu)
              .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
              .Property(p => p.TotalPrice)
              .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
            .Property(p => p._orderStatus)
            .HasConversion<string>();

            modelBuilder.Entity<Order>()
             .Property(p => p.payMethod)
             .HasConversion<string>();

            modelBuilder.Entity<Order>()
             .Property(p => p.Price)
             .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
              .Property(p => p.PriceQuantity)
              .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CartItem>()
              .Property(p => p.Price)
              .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CartItem>()
              .Property(p => p.PriceQuantity)
              .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Discounts>()
            .Property(p => p.Value)
            .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Discounts>()
            .Property(p => p._discountStatus)
             .HasConversion<string>();
        }

    }
}
