using CateringPro.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Dotmim.Sync;
//using Dotmim.Sync.SqlServer;
//using Dotmim.Sync.Sqlite;
//using Dotmim.Sync.Enumerations;
using System.Threading;
using CateringPro.Data;
using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using SQLitePCL;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Crypto.Prng;

namespace CateringPro.Core
{
    public interface IDbSyncer
    {
        int GetDefaultCompanyId();
        string GetOutput();
        Task InitialSyncByDBContext(int companyId, DateTime daydate);
        Task SyncDb(int? companyId = default, DateTime? daydate = default);
        Task SyncOrders(int companyId, DateTime daydate);
        Task SyncOrdersWeek(int companyId, DateTime startdate);
    }
    public class DbSyncer : IDbSyncer
    {
        private readonly IConfiguration _config;
        private readonly ILogger<CompanyUser> _logger;
        private readonly AppDbContext _context;
        private readonly RemoteDbContext _remotecontext;
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly IServiceProvider _serviceProvider;
        private StringBuilder _output = new StringBuilder();
        public DbSyncer(IConfiguration iConfig, ILogger<CompanyUser> logger, AppDbContext context, IWebHostEnvironment hostingEnv, RemoteDbContext remotecontext, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = iConfig;
            _context = context;
            _hostingEnv = hostingEnv;
            _remotecontext = remotecontext;
            _serviceProvider = serviceProvider;

    }
        public string GetOutput()
        {
            return _output.ToString();
        }
        public async Task SyncDb(int? companyId=default,DateTime? daydate=default)
        {
            if (_hostingEnv.EnvironmentName != "LocalProduction")
                return;

            var serverConnectionString = _config.GetSection("ConnectionStrings").GetSection("RemoteConnection").Value;
            var clientConnectionString = _config.GetSection("ConnectionStrings").GetSection("LocalConnection").Value;
            var filename = _config.GetSection("ConnectionStrings").GetSection("LocalFileName").Value;
            DateTime day = daydate.HasValue ? daydate.Value : DateTime.Now;
            int cid = companyId.HasValue ? companyId.Value : 1;
            _output.Clear();
            if (DbIsEmpty())
            {
                await InitialSyncByDBContext(cid, day);
                await SyncOrders(cid, day);
            }
            else
            {
                await SyncOrders(cid, day);
            }
            return;
 
        }
        public async Task SyncOrdersWeek(int companyId, DateTime startdate)
        {
            for (var i = 0; i < 7; i++)
            {
                DateTime date = startdate.AddDays(i);
                await SyncOrders(companyId, date.ResetHMS());
            }
        }
        public async Task SyncOrders(int companyId, DateTime daydate)
        {
            if (_hostingEnv.EnvironmentName != "LocalProduction")
                return;

            CleanTable(_remotecontext.UserDayDish, companyId, (d) => d.Date == daydate);
            CleanTable(_remotecontext.UserDayComplex, companyId, (d) => d.Date == daydate);
            CleanTable(_remotecontext.UserDay, companyId, (d) => d.Date == daydate);
            CleanTable(_remotecontext.DayDish, companyId, (d) => d.Date == daydate);
            CleanTable(_remotecontext.DayComplex, companyId, (d) => d.Date == daydate);


            CopyDataTable(_remotecontext.DayDish, companyId,(d)=>d.Date== daydate);
            CopyDataTable(_remotecontext.DayComplex, companyId, (d) => d.Date == daydate);
            CopyDataTable(_remotecontext.UserDay, companyId, (d) => d.Date == daydate);
            CopyDataTable(_remotecontext.UserDayDish, companyId, (d) => d.Date == daydate);
            CopyDataTable(_remotecontext.UserDayComplex, companyId, (d) => d.Date == daydate);
        }
        public int GetDefaultCompanyId()
        {
            try
            {
              //  var companies = _remotecontext.Companies.IgnoreQueryFilters().ToList();
                var company = _remotecontext.Companies.IgnoreQueryFilters().FirstOrDefault(c => c.IsDefault.HasValue && c.IsDefault.Value);
                if (company == null)
                    company = _remotecontext.Companies.IgnoreQueryFilters().FirstOrDefault();
              
                if (company != null)
                    return company.Id;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getCompanyId");
            }
            return -1;
        }
        public async Task InitialSyncByDBContext(int companyId , DateTime daydate )
        {
            if (_hostingEnv.EnvironmentName != "LocalProduction")
                return;
            var tables = new string[] {  "AspNetRoleClaims", "AspNetRoles", "AspNetUserClaims", "AspNetUserLogins", "AspNetUserRoles", "AspNetUsers", "AspNetUserTokens", 
                "Categories", "Companies", "CompanyUserCompanies", "Complex",  "DishCategory", "DishComplex", "DeliveryQueues",
                "Dishes",  "DishesKind", "DishIngredients",  "IngredientCategories", "Ingredients",/*"Pictures",*/ 
                  "UserGroups", "UserInGroups", "UserSubGroups",  };
            var witdate_tables = new string[] {    "DayComplex", "DayDish",
                "UserDay", "UserDayComplex", "UserDayDish" };
            
            _remotecontext.SetCompanyID(companyId);
            _context.SetCompanyID(companyId);


            CleanTable("AspNet"+nameof(_remotecontext.UserClaims));
            CleanTable("AspNet" + nameof(_remotecontext.UserRoles));
            CleanTable("AspNet" + nameof(_remotecontext.UserTokens));
            CleanTable("AspNet" + nameof(_remotecontext.RoleClaims));
            CleanTable("AspNet" + nameof(_remotecontext.Roles));
            CleanTable("AspNet" + nameof(_remotecontext.UserLogins));
            CleanTable(nameof(_remotecontext.CompanyUserCompanies));
            CleanTable("AspNet" + nameof(_remotecontext.Users));
 

            CleanTable(nameof(_remotecontext.Pictures));
            CleanTable(nameof(_remotecontext.UserGroups));
            CleanTable(nameof(_remotecontext.UserSubGroups));

            CleanTable(nameof(_remotecontext.DishComplex));
            CleanTable(nameof(_remotecontext.Complex));
            CleanTable(nameof(_remotecontext.Dishes));
            CleanTable(nameof(_remotecontext.Ingredients));
            CleanTable(nameof(_remotecontext.DishCategory));
            CleanTable(nameof(_remotecontext.DishesKind));
            CleanTable(nameof(_remotecontext.IngredientCategories));
            CleanTable(nameof(_remotecontext.Categories));
            CleanTable(nameof(_remotecontext.Companies));


            CopyDataTable(_remotecontext.Companies, companyId);

           

            CopyDataTable(_remotecontext.Pictures, companyId);

            CopyDataTable(_remotecontext.UserGroups, -1);
            CopyDataTable(_remotecontext.UserSubGroups, -1);
            CopyDataTable(_remotecontext.Roles, companyId);
            CopyDataTable(_remotecontext.RoleClaims, companyId);
            CopyDataTable(_remotecontext.Users, companyId);
            CopyDataTable(_remotecontext.UserRoles, companyId);
            CopyDataTable(_remotecontext.UserTokens, companyId);
            CopyDataTable(_remotecontext.UserLogins, companyId);
            CopyDataTable(_remotecontext.UserClaims, companyId);
            CopyDataTable(_remotecontext.CompanyUserCompanies, companyId);

            CopyDataTable(_remotecontext.Categories, companyId);
            CopyDataTable(_remotecontext.IngredientCategories, companyId);
            CopyDataTable(_remotecontext.DishesKind, companyId);
            CopyDataTable(_remotecontext.DishCategory, companyId);
            CopyDataTable(_remotecontext.Ingredients, companyId);
        
            CopyDataTable(_remotecontext.Dishes, companyId);
            CopyDataTable(_remotecontext.Complex, companyId);
            CopyDataTable(_remotecontext.DishComplex, companyId);


            /*

            var categories =await _remotecontext.Categories.AsNoTracking().ToListAsync();
            try
            {
                _context.Database.BeginTransaction();
                //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Categories ON");
                _context.Categories.AddRange(categories);
                _context.SaveChanges();
                //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Categories OFF");
                _context.Database.CommitTransaction();
            }
            catch(Exception ex)
            {

            }
            */
        }
        private bool DbIsEmpty()
        {
            try
            {
                return _context.UserDayDish.Count() == 0;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Check if db is empty");
                return false;
            }
           
        }
        // private bool CleanTable<T>(DbSet<T> dbset) where T : class
        private bool CleanTable(string name)
        {
            try
            {
                if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.Sqlite")
                    return false;
                string cmd = $"delete from {name} ";
                _context.Database.ExecuteSqlRaw(cmd);
                _output.Append($"table {name} cleaned {Environment.NewLine}");
             }
            catch (Exception ex)
            {
                _output.Append($"Error clean table {name}  {Environment.NewLine}");
                _logger.LogError(ex, "CleanTable");
                return false;
            }
            return true;
        }
        private bool CleanTable<T>(DbSet<T> dbset, int companyid, Expression<Func<T, bool>> predicate) where T : class
        {
            try
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    using (AppDbContext local = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>())
                    {
                        local.SetCompanyID(companyid);
                        var query = local.Set<T>().AsNoTracking().Where(predicate).ToList();
                        local.RemoveRange(query);
                        local.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                //IEnumerable<T> dst = _context.Set<T>().AsNoTracking().ToList();
                _output.Append($"Error clean table {typeof(T).Name}  {Environment.NewLine}");
                _logger.LogError(ex, "CleanTable");
                return false;
            }
            return true;
        }
        private bool CopyDataTable<T>(DbSet<T> dbset,int companyid, Expression<Func<T,bool>> predicate=default) where T : class
        {
            const int batchsize = 20;
            //IEnumerable<T> test = null;
            int inserted = 0;
            try
            {
                //_context.Query(_context.Model.FindEntityType(tablename).ClrType);
                //_context.Database.BeginTransaction();
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    using (AppDbContext local = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>()) {
                        using (AppDbContext remote = serviceScope.ServiceProvider.GetRequiredService<RemoteDbContext>())
                        {
                            
                            local.SetCompanyID(companyid);
                            remote.SetCompanyID(companyid);
                            for (; true;)

                            {
                                var query = remote.Set<T>().AsQueryable();
                                if (companyid < 0)
                                    query = query.IgnoreQueryFilters();
                                if (predicate != null)
                                    query = query.Where(predicate);
                                IEnumerable<T> src = query.AsNoTracking().Skip(inserted).Take(batchsize).ToList();
                                //test = src;
                                if (src.Count() == 0)
                                    break;
                                inserted += src.Count();
                                //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Categories ON");
                                local.Set<T>().AddRange(src);
                                local.SaveChanges();
                                //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Categories OFF");

                            }
                            _output.Append($" {typeof(T).Name} Downloaded {inserted} records {Environment.NewLine}");
                            //_context.Database.CommitTransaction();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //IEnumerable<T> dst = _context.Set<T>().AsNoTracking().ToList();
                _output.Append($"Error download table {typeof(T).Name}  {Environment.NewLine}");
                _logger.LogError(ex, "CopyDataTable");
                return false;
            }
            return true;

        }
        private bool CopyDataTable(string tablename) 
        {

            try
            {
                Type myType = _context.Model.FindEntityType(tablename).ClrType;

                var dbSetMethodInfo = typeof(DbContext).GetMethod("Set");
                dynamic dbSet = dbSetMethodInfo.MakeGenericMethod(myType).Invoke(_context, null);
                dynamic Rec = Activator.CreateInstance(myType);

                Rec.active = true;
                dbSet.Add(Rec);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CopyDataTable");
                return false;
            }
            return true;

        }
        /*
        private SyncSetup CreateSyncSetup()
        {
          
            var tables = new string[] {  "AspNetRoleClaims", "AspNetRoles", "AspNetUserClaims", "AspNetUserLogins", "AspNetUserRoles", "AspNetUsers","AspNetUserTokens",
                "Categories", "Companies", "CompanyUserCompanies", "Complex",  "DishCategory", "DishComplex", "DeliveryQueues",
                "Dishes",  "DishesKind", "DishIngredients",  "IngredientCategories", "Ingredients","Pictures",
                  "UserGroups", "UserInGroups", "UserSubGroups",  };
            var witdate_tables = new string[] {    "DayComplex", "DayDish",  
                "UserDay", "UserDayComplex", "UserDayDish" };
            
          var full_tables = tables.ToList();
          full_tables.AddRange(witdate_tables.ToList());
          var syncSetup = new SyncSetup(full_tables);
          syncSetup.Tables.ToList().ForEach(t => {
              t.SyncDirection = SyncDirection.DownloadOnly;
          });
          witdate_tables.ToList().ForEach(t =>
          {
              var filter = new SetupFilter(t);

              filter.AddParameter("dt", DbType.Date, false);
              filter.AddWhere("Date", t, "dt");
              syncSetup.Filters.Add(filter);


          });
          return syncSetup;
          
            return null;
        }
*/
    }
}
/*
 // Create 2 Sql Sync providers
 // First provider is using the Sql change tracking feature. Don't forget to enable it on your database until running this code !
 // For instance, use this SQL statement on your server database : ALTER DATABASE AdventureWorks  SET CHANGE_TRACKING = ON  (CHANGE_RETENTION = 10 DAYS, AUTO_CLEANUP = ON)  
 // Otherwise, if you don't want to use Change Tracking feature, just change 'SqlSyncChangeTrackingProvider' to 'SqlSyncProvider'
 var serverProvider = new SqlSyncProvider(serverConnectionString);

 // Second provider is using plain old Sql Server provider, relying on triggers and tracking tables to create the sync environment
 var clientProvider = new Dotmim.Sync.Sqlite.SqliteSyncProvider(filename); //new SqlSyncProvider(clientConnectionString);

 // Tables involved in the sync process:
 var tables = new string[] { "Dishes", "Ingredients" };
 var cts = new CancellationTokenSource();
 var progress = new SynchronousProgress<ProgressArgs>(args =>
 _logger.LogWarning($"{args.Context.SyncStage}:\t{args.Message}"));

 // Creating an agent that will handle all the process
 var agent = new SyncAgent(clientProvider, serverProvider, CreateSyncOptions(),  CreateSyncSetup());
 if (!agent.Parameters.Contains("dt"))
     agent.Parameters.Add("dt", daydate.HasValue? daydate.Value:DateTime.Now);

 var remoteProgress = new SynchronousProgress<ProgressArgs>(s =>
 {
     Console.ForegroundColor = ConsoleColor.Yellow;
     Console.WriteLine($"{s.Context.SyncStage}:\t{s.Message}");
     Console.ResetColor();
     _logger.LogWarning($"{s.Context.SyncStage}:\t{s.Message}");
 });
 agent.AddRemoteProgress(remoteProgress);

 // do
 //   {
 // Launch the sync process
 try
 {

     var s1 = await agent.SynchronizeAsync(SyncType.Reinitialize, cts.Token, progress);
 }
 catch (Exception ex)
 {
     _logger.LogError(ex, "SyncError");
 }
 await SyncByDBContext();
 // Write results
 //Console.WriteLine(s1);

 // } while (Console.ReadKey().Key != ConsoleKey.Escape);

 Console.WriteLine("End");
 */