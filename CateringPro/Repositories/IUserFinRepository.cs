﻿using CateringPro.Models;
using CateringPro.ViewModels;
using CateringPro.ViewModels.LiqPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IUserFinRepository
    {
        Task<bool> AddBalanceToAsync(UserFinIncome userincome);
         Task<LiqPayCheckoutFormModel> GenerateLiqPayCheckOut(string userId, decimal amount, int companyId, string result_url, string callback_url);
        Task<UserFinanceViewModel> GetUserFinModelAsync(string userId, int companyId, int mounth=1);
        bool RegisterLiqPayResponse(string userId, string data, string signature, out Dictionary<string, string> dataresult, out string decodedString);
        bool MakeOrderPayment(DateTime daydate, int companyId);
        Task<bool> MakeOrderPaymentAsync(DateTime daydate, int companyId);
        bool SaveResponse(string userId, bool iscallback, Dictionary<string, string> dataresult, string jsonstr);
        Task<PaymentRedirectViewModel> GetRedirectModelAsync(string userId, int companyId,string orderid);
        Task<bool> RegisterWidgetCallbackAsync(string userId, string orderdid, string data);
        Task<UserAddBalanceConfirmModel> GetAddBalanceConfirmModelAsync(UserFinIncome finincome);
        Task<bool> SendPaymentConfirmationEmailAsync(UserFinIncome finincome);
        string GetUserIDfromOrderID(string orderid);
        int GetCompanyIdfromOrderID(string orderid);
    }
}
