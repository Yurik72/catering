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

    public class LocalDBSyncerTask : IScheduledTask
    {
        

        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        private IServiceProvider _serviceProvider;
       
        public LocalDBSyncerTask( ILogger<CompanyUser> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {

            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
         
        }
        public bool IsRunning { get; private set; }
#if DEBUG
        public string Schedule => "*/5 * * * *"; //every 5 minutes
#else
        public string Schedule => "0 8 * * *"; //every day  at 08:00
#endif
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start job LocalDBSyncerTask");
            IsRunning = true;
            try
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                   

                        var syncer = serviceScope.ServiceProvider.GetRequiredService<IDbSyncer>();
                        if (syncer == null)
                        {
                            _logger.LogError("Can't  obtain syncer");
                            return;

                        }
                    await syncer.SyncOrdersDays(syncer.GetDefaultCompanyId(), DateTime.Now.ResetHMS());
                    

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Write of production error");
            }
            finally 
            {
                IsRunning = false ;
            }
        }
    }
    
    
}