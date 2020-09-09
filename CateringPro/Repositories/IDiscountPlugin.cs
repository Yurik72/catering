using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IDiscountPlugin
    {
        void LoadConfig(IConfiguration config);
    }
}
