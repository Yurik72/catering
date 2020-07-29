using CateringPro.Core;
using CateringPro.Data;
using CateringPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public class UserFinRepository:IUserFinRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        SharedViewLocalizer _localizer;
        public UserFinRepository(AppDbContext context, ILogger<CompanyUser> logger, SharedViewLocalizer localizer)
        {
            _context = context;
            _logger = logger;
            _localizer = localizer;
        }

        public bool MakeOrderPayment(DateTime daydate, int companyId)
        {
            try
            {
                _logger.LogInformation("MakeOrderPayment {0},{1}", daydate, companyId);
                var res = _context.Database.ExecuteSqlInterpolated($"exec MakeOrderPayment {daydate} , {companyId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MakeOrderPayment");
                return false;
            }
            return true;
        }
        public async Task<bool> MakeOrderPaymentAsync(DateTime daydate, int companyId)
        {
            try
            {
                _logger.LogInformation("MakeOrderPayment {0},{1}", daydate, companyId);
                var res = await _context.Database.ExecuteSqlInterpolatedAsync($"exec MakeOrderPayment {daydate} , {companyId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MakeOrderPayment");
                return false;
            }
            return true;
        }
    }
}
