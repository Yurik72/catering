using CateringPro.Core;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.ViewModels;
using CateringPro.ViewModels.LiqPay;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public class UserFinRepository : IUserFinRepository
    {
        static private readonly string _private_key;
        static private readonly string _public_key;

        static UserFinRepository()
        {
            _public_key = "sandbox_i42620892264";     // Public Key компанії, який можна знайти в особистому кабінеті на сайті liqpay.ua
            _private_key = "sandbox_Tmlocf82Jq40HNqjlmbgAZr3oM9LG0pc35JLH7IH";    // Private Key компанії, який можна знайти в особистому кабінеті на сайті liqpay.ua
        }
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        SharedViewLocalizer _localizer;
        private readonly UserManager<CompanyUser> _userManager;
        private readonly IEmailService _email;
        public UserFinRepository(AppDbContext context, ILogger<CompanyUser> logger, 
            SharedViewLocalizer localizer, UserManager<CompanyUser> userManager, IEmailService email)
        {
            _context = context;
            _logger = logger;
            _localizer = localizer;
            _userManager = userManager;
            _email = email;
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
        public async Task<UserFinanceViewModel> GetUserFinModelAsync(string userId, int companyId)
        {
            var model = new UserFinanceViewModel() { UserId = userId, CompanyId = companyId };
            model.Finance = await _context.UserFinances.FirstOrDefaultAsync(m => m.Id == userId);
            if (model.Finance == null)
            {
                model.Finance = new UserFinance() { Id = userId, CompanyId = companyId };
                _context.Add(model.Finance);
                await _context.SaveChangesAsync();
            }
            model.Outcomes = await _context.UserFinOutComes.Where(o => o.Id == userId).OrderByDescending(o => o.TransactionDate).Take(20).ToListAsync();
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
            catch (Exception ex)
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
        public async Task<LiqPayCheckoutFormModel> GenerateLiqPayCheckOut(string userId, decimal amount, int companyId, string result_url, string callback_url)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;
            var db_income = new UserFinIncome()
            {
                Id = userId,
                IsProjection = true,
                IncomeType = 3,
                Amount = 0m,
                CompanyId = companyId,
                ProjectionAmount = amount,
                TransactionDate = DateTime.Now,
                OrderId = userId + '#' + Guid.NewGuid().ToString() + "#" + companyId.ToString()

            };
            try
            {
                _context.Add(db_income);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding fin income", ex);
                return null;
            }
          
            var signature_source = new LiqPayCheckout()
            {
                public_key = _public_key,
                version = 3,
                action = "pay",
                amount = db_income.ProjectionAmount,
                currency = "UAH",
                description = "Add balance kabachok.group " + Translit.cyr2lat(user.GetChildUserName()),// "Поповнення балансу",
                order_id = db_income.OrderId,
                sandbox = 1,

                result_url = result_url,
                server_url = callback_url,
                product_category = Translit.cyr2lat("Харчування"),//  "T",  //"Харчування",
                product_description = Translit.cyr2lat("Поповнення балансу за харчування " +user.GetChildUserName()),//"T", //"Поповнення балансу за харчування " +user.GetChildUserName(),
                product_name = Translit.cyr2lat("Харчування")// "T" //"Харчування"
            };
            var json_string = JsonConvert.SerializeObject(signature_source);
            var data_hash = Convert.ToBase64String(Encoding.ASCII.GetBytes(json_string));
            var signature_hash = GetLiqPaySignature(data_hash);

            // Данні для передачі у в'ю
            var model = new LiqPayCheckoutFormModel();
            model.Data = data_hash;
            model.Signature = signature_hash;
            _logger.LogWarning("New liqPay checkout is generated orderid=({0}), amount=({1}), hash=({2})", db_income.OrderId, db_income.Amount, data_hash);
            return model;
        }
        public bool RegisterLiqPayResponse(string userId, string data, string signature, out Dictionary<string, string> dataresult, out string decodedString)
        {
            byte[] request_data = Convert.FromBase64String(data);
            decodedString = Encoding.UTF8.GetString(request_data);
            dataresult = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedString);

            // --- Отримую сигнатуру для перевірки
            var mySignature = GetLiqPaySignature(data);

            // --- Якщо сигнатура серевера не співпадає з сигнатурою відповіді LiqPay - щось пішло не так
            if (mySignature != signature)
            {
                _logger.LogError("IsLiqPayResponseValid detected invalid signature");
                return false;
            }
            return true;
        }
        private string GetUserIDfromOrderID(string orderid)
        {
            var arr = orderid.Split("#");
            return arr[0];
        }
        private int GetCompanyIdfromOrderID(string orderid)
        {
            var arr = orderid.Split("#");
            return int.Parse(arr[2]);
        }
        public async Task<PaymentRedirectViewModel> GetRedirectModelAsync(string userId, int companyId)
        {
            var res = new PaymentRedirectViewModel();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return res;
            res.UserName = user.NameSurname;
            res.ChildName = user.GetChildUserName();
            var finincome = await _context.UserFinIncomes.LastOrDefaultAsync(ui => ui.Id == userId);
            if (finincome == null)
            {
                return res;
            }
            res.Amount = finincome.ProjectionAmount;
            res.IsConfrimed = !finincome.IsProjection;
            return res;
        }
        public bool SaveResponse(string userId, bool iscallback, Dictionary<string, string> dataresult, string jsonstr)
        {
            try
            {
                string orderid = dataresult["order_id"];
                _logger.LogWarning("Processing orderid {0}", orderid);
                string orderuserid = GetUserIDfromOrderID(orderid);
                if (!string.IsNullOrEmpty(userId) && userId != orderuserid)
                {
                    _logger.LogError("Invalid combination userid {0 }in orderid {1} ", userId, orderid);
                }
                int companyid = GetCompanyIdfromOrderID(orderid);
                _context.SetCompanyID(companyid);
                var fin_income = _context.UserFinIncomes.FirstOrDefault(income => income.Id == orderuserid && income.OrderId == orderid);
                if (fin_income == null)
                {
                    _logger.LogError("can't find transaction initiator ");
                    return false;
                }
                if (iscallback)
                {
                    fin_income.ReturnCallBackData = jsonstr;
                }
                else
                {
                    fin_income.ReturnData = jsonstr;
                }
                if (dataresult["status"] == "sandbox" || dataresult["status"] == "success")
                {
                    fin_income.IsProjection = false;
                    decimal amount;
                    int intamount;
                    if(decimal.TryParse(dataresult["amount"],out amount)){
                        fin_income.Amount = amount;
                    }else if(int.TryParse(dataresult["amount"],out intamount)){
                        fin_income.Amount = (decimal)intamount;
                    }
                    else
                    {
                        fin_income.Amount = fin_income.ProjectionAmount;
                    }
                   
                    fin_income.Comments = dataresult["description"];
                }
                    _context.Update(fin_income);
                _context.SaveChanges();
                var b= SendPaymentConfirmationEmailAsync(fin_income).Result;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveResponse", ex);
                return false;
            }
        }
        private async Task<bool> SendPaymentConfirmationEmailAsync(UserFinIncome finincome) 
        {
            var user = await _userManager.FindByIdAsync(finincome.Id);
            if (user == null)
            {
                return false;
            }
            var fin = await _context.UserFinances.FirstOrDefaultAsync(f => f.Id == user.Id);
            if (fin == null)
            {
                return false;
            }
            var msg = $" Спасибо !\r\n Мы получили вашу оплату <b>{finincome.Amount} UAH</b> </br>\r\n Ваш текущий баланс <b>{fin.Balance} UAH </b> </br>\r\n Kabachok group";
            return await _email.SendEmailNoExceptionAsync(user.Email, "Оплата получена", msg);
                
        }
        static public string GetLiqPaySignature(string data)
        {
            return Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(_private_key + data + _private_key)));
        }

        public async Task<bool> RegisterWidgetCallbackAsync(string userId, string orderdid, string data)
        {
            try
            {
                _logger.LogWarning("RegisterWidgetCallbackAsync orderid= {0} userid={1}", orderdid, userId);
                var fin_income = _context.UserFinIncomes.FirstOrDefault(income => income.Id == userId && income.OrderId == orderdid);
                if (fin_income == null)
                {
                    _logger.LogError("Can't find appropriate income records");
                    return false;
                }
                fin_income.ReturnData = data;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("RegisterWidgetCallback", ex);
                return false;
            }
        }
    }
}
