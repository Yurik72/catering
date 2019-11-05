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
    // uses https://theysaidso.com/api/
    public class QuoteOfTheDayTask : IScheduledTask
    {
        

        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        public QuoteOfTheDayTask( ILogger<CompanyUser> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {

            _logger = logger;
            _configuration = configuration;
        }
        public string Schedule => "* */6 * * *";
        
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var httpClient = new HttpClient();

            var quoteJson = JObject.Parse(await httpClient.GetStringAsync("http://quotes.rest/qod.json"));

            QuoteOfTheDay.Current = JsonConvert.DeserializeObject<QuoteOfTheDay>(quoteJson["contents"]["quotes"][0].ToString());
        }
    }
    
    public class QuoteOfTheDay
    {
        public static QuoteOfTheDay Current { get; set; }

        static QuoteOfTheDay()
        {
            Current = new QuoteOfTheDay { Quote = "No quote", Author = "Maarten" };
        }
        
        public string Quote { get; set; }
        public string Author { get; set; }
    }
}