using CateringPro.Data;
using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace CateringPro.Core
{
    public static class DBCache
    {
        public static async Task<List<Company>> GetCachedCompaniesAsync(this IMemoryCache cache,AppDbContext context)
        {
            var res=await cache.GetOrCreateAsync("CompanyList", async entry =>
            {

                entry.SetPriority(CacheItemPriority.Normal);
               // entry.AddExpirationToken(changeToken);
                entry.SetSlidingExpiration(TimeSpan.FromMinutes(10));
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(100);

                return await context.Companies.AsNoTracking().ToListAsync();
            });
            return  res;
        }
        public static async Task<Company> GetCachedCompanyAsync(this IMemoryCache cache, AppDbContext context,int companyid)
        {
            return (await cache.GetCachedCompaniesAsync(context)).FirstOrDefault(c => c.Id == companyid);
        }
    }
}
