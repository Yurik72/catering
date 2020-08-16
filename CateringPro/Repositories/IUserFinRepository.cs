using CateringPro.Models;
using CateringPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IUserFinRepository
    {
        Task<bool> AddBalanceToAsync(UserFinIncome userincome);
        Task<UserFinanceViewModel> GetUserFinModelAsync(string userId, int companyId);
        bool MakeOrderPayment(DateTime daydate, int companyId);
        Task<bool> MakeOrderPaymentAsync(DateTime daydate, int companyId);
    }
}
