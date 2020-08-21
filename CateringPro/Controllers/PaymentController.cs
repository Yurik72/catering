using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
            var inc = _context.UserFinIncomes.FirstOrDefault(u => u.Id == User.GetUserId());
            await _fin.SendPaymentConfirmationEmailAsync(inc);
             //from site
            /*
            var sign = "D+5NcfCR1+QiljL2RQ6SqYR6SsA=";
            var data ="eyJwYXltZW50X2lkIjoxNDAxODM3MDg1LCJhY3Rpb24iOiJwYXkiLCJzdGF0dXMiOiJzYW5kYm94IiwidmVyc2lvbiI6MywidHlwZSI6ImJ1eSIsInBheXR5cGUiOiJwcml2YXQyNCIsInB1YmxpY19rZXkiOiJzYW5kYm94X2k0MjYyMDg5MjI2NCIsImFjcV9pZCI6NDE0OTYzLCJvcmRlcl9pZCI6IjI3ZmI0NTdmLThiNGYtNGE2Ni05NmNlLTVlOThhZTJmMWQ5MSNmMTkyZTMxNC00ZjQ4LTRkOTQtOTBjNS1lNGYzNTRiNzA3YjUjOCIsImxpcXBheV9vcmRlcl9pZCI6IkVSV1UyUkpMMTU5ODAxMDAwNjQ1OTQyMyIsImRlc2NyaXB0aW9uIjoiQWRkIGJhbGFuY2Uga2FiYWNob2suZ3JvdXAgYWRtaW4iLCJzZW5kZXJfcGhvbmUiOiIzODA1MDMxMjEwNzUiLCJzZW5kZXJfZmlyc3RfbmFtZSI6Ikl1cmlpIiwic2VuZGVyX2xhc3RfbmFtZSI6IktvdmFsZW5rbyIsInNlbmRlcl9jYXJkX21hc2syIjoiNTM2MzU0Kjc1Iiwic2VuZGVyX2NhcmRfYmFuayI6InBiIiwic2VuZGVyX2NhcmRfdHlwZSI6Im1jIiwic2VuZGVyX2NhcmRfY291bnRyeSI6ODA0LCJhbW91bnQiOjEuMCwiY3VycmVuY3kiOiJVQUgiLCJzZW5kZXJfY29tbWlzc2lvbiI6MC4wLCJyZWNlaXZlcl9jb21taXNzaW9uIjowLjAzLCJhZ2VudF9jb21taXNzaW9uIjowLjAsImFtb3VudF9kZWJpdCI6MS4wLCJhbW91bnRfY3JlZGl0IjoxLjAsImNvbW1pc3Npb25fZGViaXQiOjAuMCwiY29tbWlzc2lvbl9jcmVkaXQiOjAuMDMsImN1cnJlbmN5X2RlYml0IjoiVUFIIiwiY3VycmVuY3lfY3JlZGl0IjoiVUFIIiwic2VuZGVyX2JvbnVzIjowLjAsImFtb3VudF9ib251cyI6MC4wLCJtcGlfZWNpIjoiNyIsImlzXzNkcyI6ZmFsc2UsImxhbmd1YWdlIjoicnUiLCJwcm9kdWN0X2NhdGVnb3J5IjoiS0hhcmNodXZhbm5qYSIsInByb2R1Y3RfbmFtZSI6IktIYXJjaHV2YW5uamEiLCJwcm9kdWN0X2Rlc2NyaXB0aW9uIjoiUG9wb3ZuZW5uamFfYmFsYW5zdV96YV9raGFyY2h1dmFubmphX2FkbWluIiwiY3JlYXRlX2RhdGUiOjE1OTgwMTAwMDY0NjEsImVuZF9kYXRlIjoxNTk4MDEwMDA2NDc4LCJ0cmFuc2FjdGlvbl9pZCI6MTQwMTgzNzA4NX0=";
            Dictionary<string, string> request_data_dictionary = null;
            string decoded = "";
            bool valid = _fin.RegisterLiqPayResponse("", data, sign, out request_data_dictionary, out decoded);
            if (!valid)
                return View("Error");
            if (_fin.SaveResponse(User.GetUserId(), true, request_data_dictionary, decoded))
                return View("Error");
             */
            /*  FOR TESTING REAL answer from liqpay
            var data="eyJwYXltZW50X2lkIjoxNDAxNzgxNzgyLCJhY3Rpb24iOiJwYXkiLCJzdGF0dXMiOiJzYW5kYm94IiwidmVyc2lvbiI6MywidHlwZSI6ImJ1eSIsInBheXR5cGUiOiJwcml2YXQyNCIsInB1YmxpY19rZXkiOiJzYW5kYm94X2k0MjYyMDg5MjI2NCIsImFjcV9pZCI6NDE0OTYzLCJvcmRlcl9pZCI6IjI3ZmI0NTdmLThiNGYtNGE2Ni05NmNlLTVlOThhZTJmMWQ5MSNmMzlkMmJiZi1kYzQ1LTRiYjktYjk3Yi1hYTExZTA0NjhkMmQjMSIsImxpcXBheV9vcmRlcl9pZCI6IkZKTFVSNjlKMTU5ODAwNjUxMjIwODI2MCIsImRlc2NyaXB0aW9uIjoiQWRkIGJhbGFuY2Uga2FiYWNob2suZ3JvdXAgSlVyaXlfS292YWxlbmtvIiwic2VuZGVyX3Bob25lIjoiMzgwNTAzMTIxMDc1Iiwic2VuZGVyX2ZpcnN0X25hbWUiOiJJdXJpaSIsInNlbmRlcl9sYXN0X25hbWUiOiJLb3ZhbGVua28iLCJzZW5kZXJfY2FyZF9tYXNrMiI6IjUzNjM1NCo3NSIsInNlbmRlcl9jYXJkX2JhbmsiOiJwYiIsInNlbmRlcl9jYXJkX3R5cGUiOiJtYyIsInNlbmRlcl9jYXJkX2NvdW50cnkiOjgwNCwiYW1vdW50IjoxLjAsImN1cnJlbmN5IjoiVUFIIiwic2VuZGVyX2NvbW1pc3Npb24iOjAuMCwicmVjZWl2ZXJfY29tbWlzc2lvbiI6MC4wMywiYWdlbnRfY29tbWlzc2lvbiI6MC4wLCJhbW91bnRfZGViaXQiOjEuMCwiYW1vdW50X2NyZWRpdCI6MS4wLCJjb21taXNzaW9uX2RlYml0IjowLjAsImNvbW1pc3Npb25fY3JlZGl0IjowLjAzLCJjdXJyZW5jeV9kZWJpdCI6IlVBSCIsImN1cnJlbmN5X2NyZWRpdCI6IlVBSCIsInNlbmRlcl9ib251cyI6MC4wLCJhbW91bnRfYm9udXMiOjAuMCwibXBpX2VjaSI6IjciLCJpc18zZHMiOmZhbHNlLCJsYW5ndWFnZSI6InJ1IiwicHJvZHVjdF9jYXRlZ29yeSI6IktIYXJjaHV2YW5uamEiLCJwcm9kdWN0X25hbWUiOiJLSGFyY2h1dmFubmphIiwicHJvZHVjdF9kZXNjcmlwdGlvbiI6IlBvcG92bmVubmphX2JhbGFuc3VfemFfa2hhcmNodXZhbm5qYV9KVXJpeV9Lb3ZhbGVua28iLCJjcmVhdGVfZGF0ZSI6MTU5ODAwNjUxMjIxMCwiZW5kX2RhdGUiOjE1OTgwMDY1MTIyMjYsInRyYW5zYWN0aW9uX2lkIjoxNDAxNzgxNzgyfQ==";
            var sign = "vI2qaLu9uipOUALnAqsmIkjndac=";
            Dictionary<string, string> request_data_dictionary = null;
            string decoded = "";
            bool valid = _fin.RegisterLiqPayResponse("", data, sign, out request_data_dictionary, out decoded);
            if (!valid)
                return View("Error");
            if (_fin.SaveResponse(User.GetUserId(), true, request_data_dictionary, decoded))
                return View("Error");
             */
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
                {
                    _logger.LogWarning("RegisterLiqPayResponse returns false");
                    return View("Error");
                }
                if (_fin.SaveResponse("", true, request_data_dictionary, decoded))
                    return View("Error");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment callback error ");
            }
            return Ok();
        }
    }
}
