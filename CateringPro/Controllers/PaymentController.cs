using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CateringPro.Core;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.ViewModels.LiqPay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CateringPro.Controllers
{
 [Authorize]
    public class PaymentController : Controller
    {
        private readonly IUserFinRepository _fin;
        private readonly SharedViewLocalizer _localizer;
        private readonly ILogger<CompanyUser> _logger;
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context,
                         ILogger<CompanyUser> logger, ICompanyUserRepository companyuser_repo,
                         IUserFinRepository fin,
                         SharedViewLocalizer localizer)
        {
            _context = context;

            _logger = logger;

            _fin = fin;
            _localizer = localizer;
        }
        public async Task<ActionResult> Index(decimal? amount)
        {
            decimal orderamount = 500;
            if (amount.HasValue)
                orderamount = amount.Value;

            orderamount = 1m;
            var model = await _fin.GenerateLiqPayCheckOut(User.GetUserId(), orderamount, User.GetCompanyID(), GetUrl("/Payment/Redirect"), GetUrl("/Payment/Callback"));
             if(model==null)
                 return View("Error");
            return View(model);
        }
        private string GetUrl(string action)
        {
            return HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value + action;
        }
        /// <summary>
        /// На цю сторінку LiqPay відправляє результат оплати. Вона вказана в data.result_url
        /// </summary>
        /// <returns></returns>
        
        public async Task<ActionResult> Redirect()
        {

            return View(await _fin.GetRedirectModelAsync(User.GetUserId(), User.GetCompanyID()));
            // --- Перетворюю відповідь LiqPay в Dictionary<string, string> для зручності:
            // var request_dictionary = Request.Form.ToDictionary(key => key, key => Request.Form[key]);
            /*
            var req_signature = Request.Form["signature"];
            var req_data = Request.Form["data"];
            Dictionary<string, string> request_data_dictionary = null;
            string decoded="";
            bool valid = _fin.RegisterLiqPayResponse(User.GetUserId(),req_data, req_signature,out  request_data_dictionary,out decoded);
            if(!valid)
                  return View("Error");
            if( _fin.SaveResponse(User.GetUserId(), false, request_data_dictionary, decoded))
                return View("Error");
            */

            /*
            // --- Розшифровую параметр data відповіді LiqPay та перетворюю в Dictionary<string, string> для зручності:
            byte[] request_data = Convert.FromBase64String(req_data);
            string decodedString = Encoding.UTF8.GetString(request_data);
            var request_data_dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedString);

            // --- Отримую сигнатуру для перевірки
            var mySignature = LiqPayHelper.GetLiqPaySignature(req_data);

            // --- Якщо сигнатура серевера не співпадає з сигнатурою відповіді LiqPay - щось пішло не так
            if (mySignature != req_signature)
                return View("Error");

            // --- Якщо статус відповіді "Тест" або "Успіх" - все добре
            if (request_data_dictionary["status"] == "sandbox" || request_data_dictionary["status"] == "success")
            {
                // Тут можна оновити статус замовлення та зробити всі необхідні речі. Id замовлення можна взяти тут: request_data_dictionary[order_id]
                // ...

                return View("Success");
            }
            */
            return View("Success");
        }
        [HttpPost]
        public async Task<JsonResult> RegisterWidgetCallback(string orderid,string data)
        {
            var res=await _fin.RegisterWidgetCallbackAsync(User.GetUserId(), orderid, data);
            return Json(new { status = res });
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Callback()
        {
            try
            {
                var req_signature = Request.Form["signature"];
                var req_data = Request.Form["data"];
                _logger.LogWarning("Getting payment callback signature=({0}), data=({1})", req_signature, req_data);
                Dictionary<string, string> request_data_dictionary = null;
                string decoded = "";
                bool valid = _fin.RegisterLiqPayResponse("", req_data, req_signature, out request_data_dictionary, out decoded);
                if (!valid)
                    return View("Error");
                if (_fin.SaveResponse(User.GetUserId(), false, request_data_dictionary, decoded))
                    return View("Error");

            }
            catch (Exception ex)
            {
                _logger.LogError("Payment callback error", ex);
            }
            return Ok();
        }
    }
}
