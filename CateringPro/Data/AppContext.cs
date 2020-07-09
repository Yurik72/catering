﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using CateringPro.Core;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics;

namespace CateringPro.Data
{
    public class AppDbContext : IdentityDbContext<CompanyUser, CompanyRole,string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int companyId=-1;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetCompanyID(int val)
        {
            companyId = val;
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Categories> Categories { get; set; }

        public DbSet<UserGroups> UserGroups { get; set; }

        public DbSet<IngredientCategories> IngredientCategories { get; set; }
        //     public DbSet<Pizzas> Pizzas { get; set; }
        public DbSet<Ingredients> Ingredients { get; set; }
    //    public DbSet<PizzaIngredients> PizzaIngredients { get; set; }
    //    public DbSet<Reviews> Reviews { get; set; }
   //     public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
      //  public DbSet<Order> Orders { get; set; }
    //    public DbSet<OrderDetail> OrderDetails { get; set; }


        public DbSet<Dish> Dishes { get; set; }

        public DbSet<DishCategory> DishCategory { get; set; }

        public DbSet<DayDish> DayDish { get; set; }

        public DbSet<DayComplex> DayComplex { get; set; }
        public DbSet<DishIngredients> DishIngredients { get; set; }


        public DbSet<MassEmail> MassEmail { get; set; }
        public DbSet<Complex> Complex { get; set; }

        public DbSet<DishComplex> DishComplex { get; set; }

        public DbSet<Docs> Docs { get; set; }

        public DbSet<DocLines> DocLines { get; set; }

        public DbSet<Consignment> Consignment { get; set; }
        public DbSet<UserWeekBasket> UserWeekBasket { get; set; }

        public DbSet<UserDay> UserDay { get; set; }
        public DbSet< UserDayDish> UserDayDish { get; set; }

        public DbSet<UserDayComplex> UserDayComplex { get; set; }
        public DbSet<Pictures> Pictures { get; set; }

        protected int CompanyId
        {
            get {
                if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User != null)
                    return _httpContextAccessor.HttpContext.User.GetCompanyID();
                return companyId;
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

           // modelBuilder.Entity<CompanyUser>().HasKey(u =>u.tag);

            base.OnModelCreating(modelBuilder);

            

            //P key
            modelBuilder.Entity<UserWeekBasket>()
                .HasKey(o => new { o.UserId, o.BasketDate });

            modelBuilder.Entity<UserDayDish>()
               .HasKey(o => new { o.UserId, o.Date,o.DishId, o.CompanyId,o.ComplexId});
            //many to many Dish <-> catgories
            modelBuilder.Entity<UserDayDish>()
                   .Property(d => d.Date)
                   .HasColumnType("date");

            modelBuilder.Entity<UserDayComplex>()
            .HasKey(o => new { o.UserId, o.Date, o.ComplexId, o.CompanyId });
            //many to many Dish <-> catgories
            modelBuilder.Entity<UserDayComplex>()
                   .Property(d => d.Date)
                   .HasColumnType("date");

            modelBuilder.Entity<UserDay>()
             .HasKey(o => new { o.UserId, o.Date, o.CompanyId });
            modelBuilder.Entity<UserDay>()
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


            //complex 
            modelBuilder.Entity<DishComplex>()
             .HasKey(bc => new { bc.DishId, bc.ComplexId,bc.DishCourse });
            //day dish
            modelBuilder.Entity<DishComplex>()
                 .HasOne(c => c.Dish)
                 .WithMany(a => a.DishComplex)
                 .IsRequired()
                
                 .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DishComplex>()
                 .HasOne(c => c.Complex)
                 .WithMany(a => a.DishComplex)
                 .IsRequired()
                
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DayDish>()
                   .Property(d => d.Date)
                   .HasColumnType("date");

            modelBuilder.Entity<DayDish>()
              .HasKey(d => new { d.Date , d.DishId,d.CompanyId });


            modelBuilder.Entity<DayComplex>()
                   .Property(d => d.Date)
                   .HasColumnType("date");

            modelBuilder.Entity<DayComplex>()
              .HasKey(d => new { d.Date, d.ComplexId, d.CompanyId });

            ////documents
            modelBuilder.Entity<Docs>()
             .HasKey(d =>  d.Id);

            modelBuilder.Entity<DocLines>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<DocLines>()
               .HasOne(d => d.Docs)
               .WithMany(a => a.DocLines)
               .IsRequired().OnDelete(DeleteBehavior.ClientCascade);

            //stock
            modelBuilder.Entity<Consignment>()
             .HasKey(c => new { c.CompanyId, c.LineId,c.IngredientsId });
            modelBuilder.Entity<Consignment>()
                 .HasOne(c => c.Ingredients)
                 .WithMany(a => a.Consignments)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);

            Expression<Func<CompanyData,bool>> test = u => u.CompanyId == this.CompanyId;

            //modelBuilder.Entity<Categories>().HasQueryFilter(u => u.CompanyId == this.CompanyId);
            //modelBuilder.Entity<Dish>().HasQueryFilter(u =>u.CompanyId== this.CompanyId);
            // to do dynamically
            SetGlobalFilters(modelBuilder);

        }
        private void SetGlobalFilters(ModelBuilder modelBuilder)
        {
            Assembly.GetExecutingAssembly().DefinedTypes.ToList().ForEach(ti =>
            {
                if(!ti.IsGenericType && !ti.IsAbstract && ti.AsType().IsSubclassOf(typeof(CompanyData)))
                {
                    try
                    {
                        Type t = ti.AsType();
                        
                        
                        MethodInfo method = typeof(ModelBuilder).GetMethods().
                        SingleOrDefault(m => m.Name == nameof(ModelBuilder.Entity) && m.ReturnType.IsGenericType);
                        MethodInfo generic = method.MakeGenericMethod(t);
                        EntityTypeBuilder x = generic.Invoke(modelBuilder, null) as EntityTypeBuilder;
                        ParameterExpression parameter = Expression.Parameter(t, "x");
                        //the left member
                        MemberExpression leftMember = Expression.Property(parameter, "CompanyId");
                        //the right member
                        Expression propertyExpr = Expression.Property(Expression.Constant(this), "CompanyId");

                        Expression equalExpression = Expression.Equal(leftMember, propertyExpr);
                        //the lambda of the equal expression
                        LambdaExpression lambda = Expression.Lambda(equalExpression, parameter);
                        x.HasQueryFilter(lambda);
                        // Type tf = typeof(Func<,>);
                      

                    }
                    catch(Exception ex)
                    {

                    }

                }
            });
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //      "Server = (localdb)\\mssqllocaldb; Database = PizzaShop; Trusted_Connection = True; ");
        //}

    }
}
