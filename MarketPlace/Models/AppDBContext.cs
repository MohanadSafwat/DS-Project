using MarketPlace.Areas.Identity.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace JWTAuthentication.Authentication
{
    public class AppDBContext : IdentityDbContext<User>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().HasAlternateKey(x => x.Uid).HasName("Uid");


        }

        public DbSet<User> User { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AssociatedBought> AssociatedBought { get; set; }
        public DbSet<AssociatedSell> AssociatedSellSold { get; set; }
        public DbSet<AssociatedShared> AssociatedSharedSold { get; set; }
        public DbSet<AssociatedSell> AssociatedSellUnSold { get; set; }
        public DbSet<AssociatedShared> AssociatedSharedUnSold { get; set; }

    }
}
