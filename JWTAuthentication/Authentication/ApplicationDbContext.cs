using MarketPlace.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace JWTAuthentication.Authentication
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
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
        public DbSet<AssociatedSell> AssociatedSell { get; set; }
        public DbSet<AssociatedShared> AssociatedShared { get; set; }

    }
}
