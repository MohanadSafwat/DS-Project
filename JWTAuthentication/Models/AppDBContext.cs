﻿// using MarketPlace.Models;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using JWTAuthentication.Authentication;

// namespace MarketPlace.Models
// {
//     public class AppDBContext :IdentityDbContext<User>
//     {
//         public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
//         {

//         }
//         protected override void OnModelCreating(ModelBuilder builder)
//         {
//             base.OnModelCreating(builder);
//             builder.Entity<User>().HasAlternateKey(x => x.Uid).HasName("Uid");

//         }


//         public DbSet<Order> Orders { get; set; }
//         public DbSet<OrderItem> OrderItems { get; set; }
//         public DbSet<Product> Products { get; set; }
//         public DbSet<AssociatedBought> AssociatedBought { get; set; }
//         public DbSet<AssociatedSell> AssociatedSell { get; set; }
//         public DbSet<AssociatedShared> AssociatedShared { get; set; }


//     }
// }
