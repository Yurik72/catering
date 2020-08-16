using CateringPro.Core;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<CompanyUser> _userManager;
        public UserFinRepository(AppDbContext context, ILogger<CompanyUser> logger, SharedViewLocalizer localizer, UserManager<CompanyUser> userManager)
        {
            _context = context;
            _logger = logger;
            _localizer = localizer;
            _userManager = userManager;
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
        public async Task<UserFinanceViewModel> GetUserFinModelAsync(string userId,int companyId)
        {
            var model=new UserFinanceViewModel() { UserId = userId, CompanyId = companyId };
            model.Finance =await  _context.UserFinances.FirstOrDefaultAsync(m => m.Id == userId);
            if (model.Finance == null)
            {
                model.Finance = new UserFinance() { Id = userId, CompanyId = companyId };
                _context.Add(model.Finance);
                await _context.SaveChangesAsync();
            }
             model.Outcomes  = await _context.UserFinOutComes.Where(o => o.Id == userId).OrderByDescending(o => o.TransactionDate).Take(20).ToListAsync();
            model.Incomes = await _context.UserFinIncomes.Where(o => o.Id == userId).OrderByDescending(o => o.TransactionDate).Take(20).ToListAsync();
            return model;
        }

        public async Task<bool> AddBalanceToAsync(UserFinIncome userincome)
        {
            var user = await _userManager.FindByIdAsync(userincome.Id);
            if (user == null)
                return false;
            try
            {
                _context.Add(userincome);
                await _context.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "AddBalanceTo");
            }
            /*
            var user = _userManager.FindByIdAsync(finIncome.Id).Result;
            if (user != null)
            {
                if (finIncome.Amount != null)
                {
                    finIncome.Id = user.Id;
                    finIncome.TransactionDate = DateTime.Now;
                    finIncome.IncomeType = 1;
                    finIncome.TransactionData = null;
                    finIncome.CompanyId = user.CompanyId;
                    _context.UserFinIncomes.Add(finIncome);
                }
            }
            */
            return true;
        }
    }
}
