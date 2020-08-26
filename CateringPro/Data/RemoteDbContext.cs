using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Data
{
    public class RemoteDbContext: AppDbContext
    {
        private readonly IConfiguration _config;
        public RemoteDbContext( IConfiguration config) 
        {
            _config = config;
         }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                var serverConnectionString = _config.GetSection("ConnectionStrings").GetSection("RemoteConnection").Value;

                optionsBuilder.UseSqlServer(serverConnectionString);
            }
        }
     
    }
}
