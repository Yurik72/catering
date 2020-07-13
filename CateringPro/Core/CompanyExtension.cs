using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Core
{
    public static class CompanyExtension
    {
        
        public static IEnumerable<string> EMails(this IEnumerable<CompanyUser> src)
        {
            return src.Select(u => u.Email);
        }
    }
}
