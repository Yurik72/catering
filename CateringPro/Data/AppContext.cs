﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CateringPro.Data
{
    public class AppDbContext : IdentityDbContext<CompanyUser, CompanyRole,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Pizzas> Pizzas { get; set; }
        public DbSet<Ingredients> Ingredients { get; set; }
        public DbSet<PizzaIngredients> PizzaIngredients { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        public DbSet<Dish> Dishes { get; set; }

        public DbSet<DishCategory> DishCategory { get; set; }

        public DbSet<DayDish> DayDish { get; set; }

        public DbSet<DishIngredients> DishIngredients { get; set; }
        public DbSet<UserWeekBasket> UserWeekBasket { get; set; }

        public DbSet< UserDayDish> UserDayDish { get; set; }

        public DbSet<Pictures> Pictures { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

           // modelBuilder.Entity<CompanyUser>().HasKey(u =>u.tag);

            base.OnModelCreating(modelBuilder);

            

            //P key
            modelBuilder.Entity<UserWeekBasket>()
                .HasKey(o => new { o.UserId, o.BasketDate });

            modelBuilder.Entity<UserDayDish>()
               .HasKey(o => new { o.UserId, o.Date,o.DishId, o.CompanyId});
            //many to many Dish <-> catgories
            modelBuilder.Entity<UserDayDish>()
                   .Property(d => d.Date)
                   .HasColumnType("date");

            modelBuilder.Entity<DishCategory>()
                .HasKey(bc => new { bc.DishId, bc.CategoryId });
            modelBuilder.Entity<DishCategory>()
                .HasOne(bc => bc.Dish)
                .WithMany(b => b.DishCategories)
                .HasForeignKey(bc => bc.DishId);
            modelBuilder.Entity<DishCategory>()
                .HasOne(bc => bc.Categories)
                .WithMany(c => c.DishCategories)
                .HasForeignKey(bc => bc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DishIngredients>()
               .HasKey(bc => new { bc.DishId, bc.IngredientId });

            modelBuilder.Entity<Dish>()
                 .HasOne(c => c.Category)
                 .WithMany(a => a.Dishes)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DayDish>()
                .HasOne(dd=> dd.Dish).WithMany(a=>a.DayDishes)
                .IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserDayDish>()
               .HasOne(ud => ud.Dish).WithMany(a => a.UserDayDishes)
               .IsRequired().OnDelete(DeleteBehavior.Restrict);

            //day dish


            modelBuilder.Entity<DayDish>()
                   .Property(d => d.Date)
                   .HasColumnType("date");

            modelBuilder.Entity<DayDish>()
              .HasKey(d => new { d.Date , d.DishId,d.CompanyId });
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //      "Server = (localdb)\\mssqllocaldb; Database = PizzaShop; Trusted_Connection = True; ");
        //}

    }
}
