﻿using System;
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

namespace CateringPro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddLogging();
            services.AddIdentity<CompanyUser, CompanyRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                   // .AddDefaultUI()
                    .AddDefaultTokenProviders();
                   


           
            services.AddTransient<IPizzaRepository, PizzaRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IAdminRepository, AdminRepository>();

            services.AddTransient<IDayDishesRepository, DayDishesRepository>();
            services.AddTransient<IUserDayDishesRepository, UserDayDishesRepository>();
            services.AddTransient<IDishesRepository, DishesRepository>();


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sp => ShoppingCart.GetCart(sp));

           // services.AddScoped<IUserClaimsPrincipalFactory<CompanyUser>, UserClaimsPrincipalFactory<CompanyUser, CompanyRole>>();
            services.AddScoped<IUserClaimsPrincipalFactory<CompanyUser>, CustomClaimsPrincipalFactory>();

            //services.AddScoped<IUserClaimsPrincipalFactory<CompanyUser>, UserClaimsPrincipalFactory<CompanyUser, CompanyRole>>();


            services.AddMemoryCache();
            // services.AddDistributedMemoryCache();
           
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
             
            });
            services.AddTransient<SharedViewLocalizer>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider = (t, f) => f.Create(typeof(SharedResources)));

            services.AddJsReport(new LocalReporting().UseBinary(JsReportBinary.GetBinary()).AsUtility().Create());

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

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
            
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthorization();

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru-RU")


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
