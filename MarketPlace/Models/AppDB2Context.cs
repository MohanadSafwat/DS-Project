using MarketPlace.Areas.Identity.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace JWTAuthentication.Authentication
{
    public class AppDB2Context : IdentityDbContext<User2>
    {
        public AppDB2Context(DbContextOptions<AppDB2Context> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User2>().HasAlternateKey(x => x.Uid).HasName("Uid");


        }

        public DbSet<User2> User { get; set; }
        public DbSet<OrderSouth> OrdersSouth { get; set; }
        public DbSet<OrderItemSouth> OrderItemsSouth { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AssociatedBoughtSouth> AssociatedBoughtSouth { get; set; }
        public DbSet<AssociatedSellSouth> AssociatedSellSouthSold { get; set; }
        public DbSet<AssociatedSellSouth> AssociatedSellSouthUnSold { get; set; }

        public DbSet<AssociatedSharedSouth> AssociatedSharedSouthSold { get; set; }
        public DbSet<AssociatedSharedSouth> AssociatedSharedSouthUnSold { get; set; }

    }
}
