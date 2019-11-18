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

    public class EMailSenderTask : IScheduledTask
    {
        

        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        private IServiceProvider _serviceProvider;
        public EMailSenderTask( ILogger<CompanyUser> logger, IConfiguration configuration, IServiceProvider serviceProvider)
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

                    foreach (var em in await context.MassEmail.AsNoTracking().ToListAsync())
                    {
                        MassMailWrapper wrap = new MassMailWrapper(em);
                        if (wrap.InvalidSchedule)
                        {
                            _logger.LogWarning("Mass mail {0} has wrong schedule definition.",em.Id);
                            continue;
                        }
                        if (!wrap.ShouldSend)
                        {
                            continue;
                        }
                        IMassEmailService meservice = serviceScope.ServiceProvider.GetRequiredService<IMassEmailService>();
                        wrap.Increment();
                        if (await meservice.SendMassEmailAsync(em, wrap.NextRunTime))
                        {
                            
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EMailSenderTask error");
            }
            
        }
    }
    
    public  class MassMailWrapper
    {
        public MassMailWrapper(MassEmail src)
        {
            this.Source = src;
            try
            {
                this.Schedule = CrontabSchedule.Parse(src.Schedule);
            }
            catch(Exception ex)
            {
                return;
            }
            this.LastRunTime = this.NextRunTime = src.NextSend;
        }
        public bool ShouldSend
        {
            get => NextRunTime < DateTime.Now;
        }
        public void Increment()
        {
            LastRunTime = NextRunTime=DateTime.Now;
            NextRunTime = Schedule.GetNextOccurrence(NextRunTime);
            //this.Source.NextSend = NextRunTime;
        }
        public bool InvalidSchedule { get => this.Schedule == null; }
        public CrontabSchedule Schedule { get; set; }

        public MassEmail Source { get; set; }
        public DateTime LastRunTime { get; private set; }
        public DateTime NextRunTime { get; private set; }
        public bool ShouldRun(DateTime currentTime)
        {
            return NextRunTime < currentTime && LastRunTime != NextRunTime;
        }
    }
}