using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CateringPro.Models;
using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using CateringPro.Core;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.Extensions.Hosting;
using jsreport.AspNetCore;
using jsreport.Local;
using jsreport.Binary;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using CateringPro.Helpers;
using Microsoft.Net.Http.Headers;

namespace CateringPro
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.EnvironmentName == "LocalProduction")
            {
                var filename = Configuration.GetSection("ConnectionStrings").GetSection("LocalFileName").Value;
                var clientConnectionString = Configuration.GetSection("ConnectionStrings").GetSection("LocalConnection").Value;
                var serverConnectionString = Configuration.GetSection("ConnectionStrings").GetSection("RemoteConnection").Value;
                services.AddDbContext<AppDbContext>(options =>
                          options.UseSqlite(clientConnectionString, options =>
                          {
                              // options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                          })
                          );
                services.AddDbContext<RemoteDbContext>(options =>
                          options.UseSqlServer(serverConnectionString, options =>
                          {
                                              // options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                                          })
                          );
            }
            else
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }
            services.AddLogging();
            services.AddIdentity<CompanyUser, CompanyRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    // .AddDefaultUI()
                    .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
                    .AddDefaultTokenProviders();
                   


           
           
           // services.AddTransient<ICategoryRepository, CategoryRepository>();
           
           

            services.AddTransient<IDayDishesRepository, DayDishesRepository>();
            services.AddTransient<IUserDayDishesRepository, UserDayDishesRepository>();
            services.AddTransient<IDishesRepository, DishesRepository>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IDocRepository, DocRepository>();
            services.AddTransient<IStockRepository, StockRepository>();
            services.AddTransient<IComplexRepository, ComplexRepository>();
            services.AddTransient<IMassEmailRepository, MassEmailRepository>();
            services.AddTransient<IUserGroupsRepository, UserGroupsRepository>();
            services.AddTransient<ICompanyUserRepository, CompanyUserRepository>();
            services.AddTransient<IUserFinRepository, UserFinRepository>();
            services.AddTransient<IServiceRepository, ServiceRepository>();
            services.AddTransient<IGenericModelRepository<Address>, GenericModelRepository<Address>>();
            services.AddTransient<IGenericModelRepository<Discount>, GenericModelRepository<Discount>>();
            services.AddTransient<IGenericModelRepository<Ingredients>, GenericModelRepository<Ingredients>>();
            services.AddTransient<IGenericModelRepository<IngredientCategories>, GenericModelRepository<IngredientCategories>>();
            services.AddTransient<IGenericModelRepository<Categories>, GenericModelRepository<Categories>>();
            services.AddTransient<IGenericModelRepository<DishKind>, GenericModelRepository<DishKind>>();
            services.AddTransient<IGenericModelRepository<Ingredients>, GenericModelRepository<Ingredients>>();

            services.AddTransient<IPluginsRepository, PluginsRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

           // services.AddScoped<IUserClaimsPrincipalFactory<CompanyUser>, UserClaimsPrincipalFactory<CompanyUser, CompanyRole>>();
            services.AddScoped<IUserClaimsPrincipalFactory<CompanyUser>, CustomClaimsPrincipalFactory>();

            //services.AddScoped<IUserClaimsPrincipalFactory<CompanyUser>, UserClaimsPrincipalFactory<CompanyUser, CompanyRole>>();
            // Add scheduled tasks & scheduler
            if (Environment.EnvironmentName != "LocalProduction")
            {
                services.AddSingleton<IScheduledTask, QuoteOfTheDayTask>();
                services.AddSingleton<IScheduledTask, WriteOffProductionTask>();
                services.AddSingleton<IScheduledTask, MakeOrdersPaymentTask>();
                services.AddSingleton<IScheduledTask, EMailSenderTask>();
                services.AddSingleton<IScheduledTask, CleanUpTask>();

                services.AddScheduler((sender, args) =>
                {
                    Console.Write(args.Exception.Message);
                    args.SetObserved();
                });

            }
            services.AddMemoryCache();
            // services.AddDistributedMemoryCache();
            services.AddResponseCaching();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
             
            });
            services.AddTransient<SharedViewLocalizer>();
            services.AddTransient<URLHelperContextLess>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider = (t, f) => f.Create(typeof(SharedResources)));

            services.AddJsReport(new LocalReporting()
                .Configure(cfg=>
                {
                    cfg.HttpPort = 14740;
                    return cfg;
                })
                .UseBinary(JsReportBinary.GetBinary()).AsUtility().Create());

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 4;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                //options.Cookie.HttpOnly = true;
               // options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                //options.LoginPath = "/Account/Login";
                options.LoginPath = "/Home/Index";

                options.LogoutPath = "/Account/LogOut";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                //options.SlidingExpiration = true;
            });
            services.AddOptions();

                services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
                services.AddTransient<IEmailService, EmailService>();
                services.AddTransient<IMassEmailService, MassEmailService>();
            
            if (Environment.EnvironmentName == "LocalProduction")
            {
                services.AddTransient<IDbSyncer, DbSyncer>();
            }


            services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.Configure<UIOption>(Configuration.GetSection("UIOption"));


            //services.Configure<DataProtectionTokenProviderOptions>(options =>
            //options.TokenLifespan = TimeSpan.FromSeconds(30));

            //  services.AddSingleton<HtmlEncoder>(
            //         HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
            //          UnicodeRanges.CyrillicSupplement }));

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           // Console.WriteLine ("Configuration[\"ASPNETCORE_ENVIRONMENT\"] is {env}", Configuration["ASPNETCORE_ENVIRONMENT"]);
            Console.WriteLine("WebHostEnvironment.EnvironmentName is {0}", env.EnvironmentName);
            if (  env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseRouting();
            app.UseAuthentication();
            

          
 
            app.UseAuthorization();
            app.UseResponseCaching();
            CultureInfo[] supportedCultures = new[]
{
                new CultureInfo("en-US"),
                 new CultureInfo("uk-UA"),
                new CultureInfo("ru-RU")
              
            };
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });
            /*
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\media")),

                RequestPath = new PathString("/media")
            });
            */
            UIOption uioption=Configuration.GetSection("UIOption").Get<UIOption>();
            var cultureInfo = new CultureInfo(uioption.DefaultCulture);
            cultureInfo.NumberFormat.CurrencySymbol = uioption.CurrencySymbol;

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(uioption.DefaultRequestCulture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider()
            }

            });

            app.UseMiddleware<RequestMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });


        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /*
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
           // loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
               // app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = "/Static"
            });
            app.UseSession();
            app.UseAuthentication();

            app.UseRouting();

           // app.UseCors();

            //app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
               // endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
*/
    }
}
