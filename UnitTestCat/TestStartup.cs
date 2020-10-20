using System;
using System.Collections.Generic;
using System.Text;

namespace CateringPro.Test
{
    using MyTested.AspNetCore.Mvc;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using CateringPro.Models;
    using Microsoft.AspNetCore.Identity;
    using CateringPro.Test.Mocks;
    using CateringPro.Repositories;
    using System.Security.Claims;

    public class TestStartup : Startup
    {
        public const int CompanyId = 5;
        public const string UserId = "testuser";
        public TestStartup(IConfiguration configuration, IWebHostEnvironment env):
            base(configuration, env)
        {

        }
        public void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            // Replace only your own custom services. The ASP.NET Core ones 
            // are already replaced by MyTested.AspNetCore.Mvc. 
            services.ReplaceSingleton<SignInManager<CompanyUser>, SignInManagerMock>();
            services.AddTransient<IUserContext, UserContext>();
            //services.Replace<IService, MockedService>();
        }
    }
    public static class UserContextEx
    {
        public static IEnumerable<Claim> GetClaims()
        {

            return new[] { new Claim("companyid", TestStartup.CompanyId.ToString()),
                           new Claim(System.Security.Claims.ClaimTypes.NameIdentifier, TestStartup.UserId)
                            };
        }
    }
    public class UserContext : IUserContext
    {
        public UserContext()
        {

        }
        public string UserId => TestStartup.UserId;

        public int CompanyId => TestStartup.CompanyId;
    }
}
