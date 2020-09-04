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
        public string Schedule => "*/10 * * * *"; //every 10 minutes
#endif
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EMailSenderTask => ExecuteAsync");
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
                    var companies = await context.Companies.ToListAsync();
                   
                    foreach (var comp in companies)
                    {
                        if (!context.IsHttpContext())
                        {
                            context.SetCompanyID(comp.Id);
                        }
                        foreach (var em in await context.MassEmail.WhereCompany(comp.Id).AsNoTracking().ToListAsync())
                        {
                            MassMailWrapper wrap = new MassMailWrapper(em);
                            if (wrap.InvalidSchedule)
                            {
                                _logger.LogWarning("Mass mail {0} has wrong schedule definition.", em.Id);
                                continue;
                            }
                            if (!wrap.ShouldSend)
                            {
                                continue;
                            }
                            IMassEmailService meservice = serviceScope.ServiceProvider.GetRequiredService<IMassEmailService>();
                            wrap.Increment();
                            //if (await meservice.SendMassEmailAsync(comp.Id, em, wrap.NextRunTime))
                            //{

                            //}
                            var b = meservice.SendMassEmailAsync(comp.Id, em, wrap.NextRunTime).Result;
                            

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
            if (src.NextSend.Year < 2020)
                NextRunTime = Schedule.GetNextOccurrence(DateTime.Now);
            //Increment();
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