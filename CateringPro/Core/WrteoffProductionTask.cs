using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CateringPro.Data;
using CateringPro.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CateringPro.Core
{

    public class WriteOffProductionTask : IScheduledTask
    {
        

        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        public WriteOffProductionTask( ILogger<CompanyUser> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {

            _logger = logger;
            _configuration = configuration;
        }
        public string Schedule => "* */6 * * *";
        
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {

        }
    }
    
    
}