using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CateringPro.Data;
using System.IO;
using Microsoft.Extensions.Configuration;
using CateringPro.Core;
using Microsoft.Extensions.Hosting;

namespace CateringPro
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHostEnvironment env = null;

            var host = CreateHostBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                 env = hostingContext.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                      optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
            
            })
            .ConfigureLogging((hostingContext, logging) =>{
                // Requires `using Microsoft.Extensions.Logging;`
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
             logging.AddConsole();
             logging.AddDebug();
             logging.AddEventSourceLogger();
             logging.AddFile();
            })
           .Build();
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    DbInitializer.Initialize(context, services, env);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  //webBuilder.UseKestrel(serverOptions =>
                  // {
                  // Set properties and call methods on options
                  // })
                  //webBuilder.UseIISIntegration()
                  webBuilder.UseStartup<Startup>();
              });
 

    }
}
