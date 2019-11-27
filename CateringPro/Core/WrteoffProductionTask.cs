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

    public class WriteOffProductionTask : IScheduledTask
    {
        

        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        private IServiceProvider _serviceProvider;
        public WriteOffProductionTask( ILogger<CompanyUser> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {

            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

#if DEBUG
        public string Schedule => "*/5 * * * *"; //every 5 minutes
#else
        public string Schedule => "0 23 * * *"; //every day  at 23:00
#endif
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start job to Write Off production day");
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
                        IStockRepository stockrepo = serviceScope.ServiceProvider.GetRequiredService<IStockRepository>();
                        if (stockrepo == null)
                        {
                            _logger.LogError("Can't  obtain Stock repository object");
                            return;

                        }
                        await stockrepo.WriteOffProductionAsync(DateTime.Now, cmp.Id);

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Write of production error");
            }
            
        }
    }
    
    
}