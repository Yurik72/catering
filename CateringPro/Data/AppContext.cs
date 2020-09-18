using Microsoft.EntityFrameworkCore;
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
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;

namespace CateringPro.Data
{
    public class AppDbContext : IdentityDbContext<CompanyUser, CompanyRole,string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int companyId=-1;
        private bool isCompanyIdSet=false;
        private readonly IWebHostEnvironment _hostingEnv;
#if DEBUG
        public EventHandler Disposing;
#endif
        protected AppDbContext()
        {

        }
#if DEBUG
        public override void Dispose()
        {
            Disposing?.Invoke(this, new EventArgs());
            base.Dispose();
        }
        public override ValueTask DisposeAsync()
        {
            Disposing?.Invoke(this, new EventArgs());
            return base.DisposeAsync();
        }
#endif
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor,IWebHostEnvironment hostingEnv) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _hostingEnv = hostingEnv;
        }

        public void SetCompanyID(int val)
        {
            companyId = val;
            isCompanyIdSet = true;
        }
        public bool IsHttpContext()
        {
            return _httpContextAccessor != null && _httpContextAccessor.HttpContext != null;
        }
        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyUser> CompanyUser { get; set; }



        public DbSet<CompanyUserCompany> CompanyUserCompanies { get; set; }

        public DbSet<Address> Addresses { get; set; }
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
        //added discount
        public DbSet<Discount> Discounts { get; set; }

        public DbSet<EmailQueue> EmailQueues { get; set; }

        public DbSet<DishCategory> DishCategory { get; set; }

        public DbSet<DayDish> DayDish { get; set; }

        public DbSet<DayComplex> DayComplex { get; set; }

        public DbSet<DeliveryQueue> DeliveryQueues { get; set; }

        public DbSet<NotOrderedQueue> NotOrderedQueues { get; set; }
        public DbSet<DishIngredients> DishIngredients { get; set; }


        public DbSet<MassEmail> MassEmail { get; set; }
        public DbSet<Complex> Complex { get; set; }

        public DbSet<DishComplex> DishComplex { get; set; }


        public DbSet<DishKind> DishesKind { get; set; }
        public DbSet<Docs> Docs { get; set; }

        public DbSet<DocLines> DocLines { get; set; }

        public DbSet<Consignment> Consignment { get; set; }

        public DbSet<ConsignmentMove> ConsignmentMove { get; set; }
        public DbSet<UserWeekBasket> UserWeekBasket { get; set; }

        public DbSet<UserDay> UserDay { get; set; }
        public DbSet< UserDayDish> UserDayDish { get; set; }

        public DbSet<UserDayComplex> UserDayComplex { get; set; }
        public DbSet<Pictures> Pictures { get; set; }


        public DbSet<UserFinance> UserFinances { get; set; }
        public DbSet<UserFinIncome> UserFinIncomes{ get; set; }

        public DbSet<UserFinOutCome> UserFinOutComes { get; set; }

        public DbSet<UserSubGroup> UserSubGroups { get; set; }

        public int CompanyId
        {
            get {
                if (isCompanyIdSet)
                    return companyId;
                if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User != null)
                    return _httpContextAccessor.HttpContext.User.GetCompanyID();
                return companyId;
            }
        }
        public ClaimsPrincipal CurrentUser
        {
            get
            {
                if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User != null)
                    return _httpContextAccessor.HttpContext.User;
                return null;
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

           // modelBuilder.Entity<CompanyUser>().HasKey(u =>u.tag);

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<CompanyUser>()
                 .Property(d => d.ChildBirthdayDate)
                 .HasColumnType("date");
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
            modelBuilder.Entity<Complex>().HasOne(bc => bc.Category);
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
            modelBuilder.Entity<Docs>()
                 .HasOne(c => c.Address)
                 .WithMany(a => a.Docs)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);
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
            modelBuilder.Entity<Consignment>()
                 .HasOne(c => c.Address)
                 .WithMany(a => a.Consignments)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConsignmentMove>()
            .HasKey(c => new { c.CompanyId, c.LineId, c.LineOutId,c.IngredientsId });

            modelBuilder.Entity<CompanyUser>()
                 .HasOne(u => u.UserGroup)
                 .WithMany(a => a.CompanyUsers)
                
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CompanyUserCompany>()
                .HasKey(cu => new { cu.CompanyId, cu.CompanyUserId });
            modelBuilder.Entity<CompanyUserCompany>()
                .HasOne(cu => cu.Company)
                .WithMany(u => u.CompanyUserCompany)
                .HasForeignKey(cu => cu.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CompanyUserCompany>()
                .HasOne(cu => cu.CompanyUser)
                .WithMany(u => u.CompanyUserCompany)
                .HasForeignKey(u => u.CompanyUserId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Complex>()
                   .HasOne(c => c.Category)
                   .WithMany(a => a.Complexes)
                   .HasForeignKey(u => u.CategoriesId).IsRequired(false)
                   .OnDelete(DeleteBehavior.NoAction);

            /*delivery*/
            modelBuilder.Entity<DeliveryQueue>()
                 .HasIndex(p => new { p.DayDate, p.UserId,p.DishId }).IsUnique(true);

            modelBuilder.Entity<CompanyUser>()
                 .HasIndex(p => new { p.CardTag }).IsUnique(true);

            modelBuilder.Entity<CompanyUser>()
                 .HasOne(u => u.UserSubGroup)
                 .WithMany(a => a.CompanyUsers)
                  .HasForeignKey(u => u.UserSubGroupId).IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserSubGroup>()
                 .HasOne(u => u.Parent)
                   .WithMany(u => u.UserSubGroups)
                   .HasForeignKey(u => u.ParentId).IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            /* fin section */
          //  modelBuilder.Entity<CompanyUser>()
          //      .HasIndex(c => new { c.Id, c.CompanyId }).IsUnique(false);
         //   modelBuilder.Entity<CompanyUser>()
          //       .HasOne(c => c.UserFinance)
         //        .WithOne(b => b.CompanyUser)
          //       .HasForeignKey<UserFinance>(b => b.Id)
          //      .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UserFinIncome>()
               .HasKey(i => new { i.Id,i.TransactionDate });
            modelBuilder.Entity<UserFinOutCome>()
              .HasKey(i => new { i.Id, i.DayDate });
            modelBuilder.Entity<UserFinIncome>()
                  .HasOne(c => c.UserFinance)
                  .WithMany(a => a.UserFinIncomes)
                  .HasForeignKey(f => new { f.Id, f.CompanyId })
                  .IsRequired(true)
                  .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UserFinOutCome>()
                  .HasOne(c => c.UserFinance)
                  .WithMany(a => a.UserFinOutComes)
                  .HasForeignKey(f => new { f.Id, f.CompanyId })
                  .IsRequired(true)
                  .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UserFinOutCome>()
               .Property(d => d.DayDate)
               .HasColumnType("date");
            modelBuilder.Entity<UserFinOutCome>()
               .Property(d => d.TransactionDate)
               .HasDefaultValueSql("(getdate())");
            modelBuilder.Entity<UserFinOutCome>()
               .Property(d => d.TransactionDate)
               .HasColumnType("datetime")
               .HasDefaultValueSql("(getdate())");

            modelBuilder.Entity<UserFinance>()
                .HasKey(u => new { u.Id, u.CompanyId });
            modelBuilder.Entity<UserFinance>()
                .Property(d => d.LastUpdated)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            modelBuilder.Entity<UserFinance>()
               .Property(d => d.Balance)
               .HasDefaultValueSql("(0.0)");

            modelBuilder.Entity<UserFinance>()
               .Property(d => d.TotalIncome)
               .HasDefaultValueSql("(0.0)");

            modelBuilder.Entity<UserFinance>()
               .Property(d => d.TotalOutCome)
               .HasDefaultValueSql("(0.0)");


            modelBuilder.Entity<UserFinance>()
               .Property(d => d.TotalOrders)
               .HasDefaultValueSql("(0)");
            modelBuilder.Entity<UserFinance>()
               .Property(d => d.TotalPreOrderedAmount)
               .HasDefaultValueSql("(0.0)");

            modelBuilder.Entity<UserFinance>()
               .Property(d => d.TotalPreOrders)
               .HasDefaultValueSql("(0)");
            //Expression<Func<CompanyData,bool>> test = u => u.CompanyId == this.CompanyId;
            if (_hostingEnv == null)//remote
            {
                modelBuilder.Entity<EmailQueue>().Property(e => e.Subject).HasColumnType("nvarchar(MAX)");
                modelBuilder.Entity<UserFinIncome>().Property(e => e.ReturnCallBackData).HasColumnType("nvarchar(MAX)");
                modelBuilder.Entity<UserFinIncome>().Property(e => e.TransactionData).HasColumnType("nvarchar(MAX)");
            }
           else  if ( _hostingEnv.EnvironmentName == "LocalProduction")
            {
                modelBuilder.Entity<EmailQueue>().Property(e => e.Subject).HasColumnType("nvarchar(400)");
                modelBuilder.Entity<UserFinIncome>().Property(e => e.ReturnCallBackData).HasColumnType("nvarchar(400)");
                modelBuilder.Entity<UserFinIncome>().Property(e => e.TransactionData).HasColumnType("nvarchar(400)");
            }
            else
            {
                modelBuilder.Entity<EmailQueue>().Property(e => e.Subject).HasColumnType("nvarchar(MAX)");
                modelBuilder.Entity<UserFinIncome>().Property(e => e.ReturnCallBackData).HasColumnType("nvarchar(MAX)");
                modelBuilder.Entity<UserFinIncome>().Property(e => e.TransactionData).HasColumnType("nvarchar(MAX)");

            }
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
