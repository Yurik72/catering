using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace CateringPro.Core
{

    public class CleanUpTask : IScheduledTask
    {
        

        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        private IServiceProvider _serviceProvider;
        public CleanUpTask( ILogger<CompanyUser> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {

            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

#if DEBUG
        public string Schedule => "*/5 * * * *"; //every 5 minutes
#else
        public string Schedule => "0 2 * * *"; //every day  at 02:00
#endif
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Start job for CleanUpTask");
            try
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                    if (context == null)
                    {
                        _logger.LogError("Can't  obtain DB context");
                        return;

                    }

                    foreach (var cmp in await context.Companies.AsNoTracking().ToListAsync())
                    {

                        await context.Database.ExecuteSqlRawAsync($"exec CleanUp {cmp.Id}");

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CleanUpTask error");
            }
            
        }
    }
    
    
}