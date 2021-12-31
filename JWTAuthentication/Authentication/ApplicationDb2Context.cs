using MarketPlace.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace JWTAuthentication.Authentication
{
    public class ApplicationDb2Context : IdentityDbContext<User2>
    {
        public ApplicationDb2Context(DbContextOptions<ApplicationDb2Context> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User2>().HasAlternateKey(x => x.Uid).HasName("Uid");
            

        }

        public DbSet<User2> User { get; set; }
         public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AssociatedBought> AssociatedBought { get; set; }
        public DbSet<AssociatedSell> AssociatedSell { get; set; }
        public DbSet<AssociatedShared> AssociatedShared { get; set; }

    }
}
