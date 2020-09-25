using System;
using Microsoft.AspNetCore.Mvc;

using jsreport.AspNetCore;
using jsreport.Types;
using System.Threading.Tasks;
using CateringPro.Models;
using CateringPro.Repositories;
using Microsoft.AspNetCore.Authorization;
using CateringPro.Core;
using CateringPro.ViewModels;
using System.Linq;
using System.Collections;
using System.IO;
using System.Text;
using CateringPro.Data;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Identity;

namespace CateringPro.Controllers
{
    [Authorize(Roles = "Admin,CompanyAdmin,KitchenAdmin,UserAdmin,SubGroupReportAdmin")]
    public class ReportController : Controller
    {
        public IJsReportMVCService JsReportMVCService { get; }
        private readonly IReportRepository _reportrepo;
        private readonly ICompanyUserRepository _companyRep;
        private IStockRepository _stockrepo;
        private readonly AppDbContext _context;
        private readonly ILogger<ReportController> _logger;

        public ReportController(AppDbContext context,IJsReportMVCService jsReportMVCService, IReportRepository rr, IStockRepository stockrepo, ILogger<ReportController> logger, ICompanyUserRepository comRep)
        {
            JsReportMVCService = jsReportMVCService;
            _reportrepo = rr;
            _stockrepo = stockrepo;
            _context = context;
            _logger = logger;
            _companyRep = comRep;

        }

        public IActionResult Index()
        {
            return View();
        }


         [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> CompanyProductionForecast(DateTime datefrom, DateTime dateto, string format)
        {
            if (datefrom.Ticks == 0)
            {
                datefrom = DateTime.Today;
            }
            if (dateto.Ticks == 0)
            {
                dateto = DateTime.Today.AddDays(3);
            }
            datefrom = datefrom.ResetHMS();
            dateto = dateto.ResetHMS();
            ViewData["datefrom"] = datefrom;
            ViewData["dateto"] = dateto;
            SelectFormat(format);

            return View(await _reportrepo.CompanyProductionForecast(datefrom, dateto, User.GetCompanyID()));
        }
         [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult CompanyDayProduction(DateTime datefrom, DateTime dateto, string format)
        {
            if (datefrom.Ticks == 0)
            {
                datefrom = DateTime.Today;
            }
            if (dateto.Ticks == 0)
            {
                dateto = DateTime.Today.AddDays(3);
            }
            datefrom = datefrom.ResetHMS();
            dateto = dateto.ResetHMS();
            ViewData["datefrom"] = datefrom;
            ViewData["dateto"] = dateto;
            SelectFormat(format);
            return View(_reportrepo.CompanyDayProduction(datefrom, dateto, User.GetCompanyID()));
        }
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult CompanyDayProductionWithoutIngredients(DateTime datefrom, DateTime dateto, string format)
        {
            if (datefrom.Ticks == 0)
            {
                datefrom = DateTime.Today;
            }
            if (dateto.Ticks == 0)
            {
                dateto = DateTime.Today.AddDays(3);
            }
            datefrom = datefrom.ResetHMS();
            dateto = dateto.ResetHMS();
            ViewData["datefrom"] = datefrom;
            ViewData["dateto"] = dateto;
            SelectFormat(format);
            return View(_reportrepo.CompanyDayProductionWithoutIngredients(datefrom, dateto, User.GetCompanyID()));
        }
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult CompanyMenu(DateTime datefrom, DateTime dateto, string format)
        {
            if (datefrom.Ticks == 0)
            {
                datefrom = DateTime.Today;
            }
            if (dateto.Ticks == 0)
            {
                dateto = DateTime.Today.AddDays(3);
            }
            datefrom = datefrom.ResetHMS();
            dateto = dateto.ResetHMS();
            SelectFormat(format);
            ViewData["datefrom"] = datefrom;
            ViewData["dateto"] = dateto;
            return View(_reportrepo.CompanyMenu(datefrom, dateto, User.GetCompanyID()));
        }
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult DishSpecification(DateTime datefrom, DateTime dateto, string format)
        {
            if (datefrom.Ticks == 0)
            {
                datefrom = DateTime.Today;
            }
            if (dateto.Ticks == 0)
            {
                dateto = DateTime.Today.AddDays(3);
            }
            datefrom = datefrom.ResetHMS();
            dateto = dateto.ResetHMS();
            ViewData["datefrom"] = datefrom;
            ViewData["dateto"] = dateto;
            SelectFormat(format);

            return View(_reportrepo.DishSpecification(datefrom, dateto, User.GetCompanyID()));
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> ConsigmentStockAsync(DateTime datefrom, DateTime dateto, string format)
        {
            SelectFormat(format);
            var res = await _stockrepo.ConsignmentStock(new QueryModel(), User.GetCompanyID());

            var resGroup = res.GroupBy(it => it.IngredientCategoryId).SelectMany(c => c).ToList();
            //HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf)
            //    .OnAfterRender((r) => HttpContext.Response.Headers["Content-Disposition"] = "attachment; filename=\"ConsigmentStock.pdf\"");


            return View(resGroup);
        }
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> UsersOrdersReport(DateTime datefrom, DateTime dateto, string format)
        {
            SelectFormat(format);
            //var groups = _companyRep.UserPermittedSubGroups(User.GetUserId(), User.GetCompanyID()).Result.ToArray();
            var groups = _companyRep.GetUserSubGroups(User.GetUserId(), User.GetCompanyID()).ToArray();
            var res = _reportrepo.UserDayReport(groups, datefrom, dateto, User.GetCompanyID());

            return View(res);
        }
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public void SelectFormat(string format)
        {
            switch (format)
            {
                case "pdf":
                    HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf)
               .Configure((r) => r.Template.Chrome = new Chrome
               {
                   MarginTop = "1cm",
                   MarginLeft = "1cm",
                   MarginBottom = "1cm",
                   MarginRight = "1cm"
               });
                    break;
                case "xlsx":
                    HttpContext.JsReportFeature()
                .Recipe(Recipe.HtmlToXlsx)
                .Configure((r) => r.Template.HtmlToXlsx = new HtmlToXlsx() { HtmlEngine = "chrome" });
                    break;
                default:
                    
                    HttpContext.JsReportFeature().Recipe(Recipe.Html).Configure((r) => 
                    r.Template.Chrome = new Chrome { 
                    });
                    break;
            }

        }
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> OrderPeriodDetailReportWithGroup(DateTime? datefrom, DateTime? dateto, string format, int? userSubGroupId)
        {
            SelectFormat(format);
            datefrom = datefrom.SetDefaultIfNotSet(DateTime.Now);
            dateto= dateto.SetDefaultIfNotSet(DateTime.Now);
            userSubGroupId = _companyRep.GetUserSubGroupId(User.GetUserId());
            var model =await  _reportrepo.GetOrderPeriodDetailReportWithGroup(datefrom.Value, dateto.Value, User.GetCompanyID(), userSubGroupId);
            return PartialView(model);
        }
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> UserFinancePeriodReportWithGroup(DateTime? datefrom, DateTime? dateto, string format, int? userSubGroupId)
        {
            SelectFormat(format);
            datefrom = datefrom.SetDefaultIfNotSet(DateTime.Now);
            dateto = dateto.SetDefaultIfNotSet(DateTime.Now);
            userSubGroupId = _companyRep.GetUserSubGroupId(User.GetUserId());
            var toplevelsubgroup = _companyRep.GetTopLevelSubGroup();
            if(User.IsInRole(Core.UserExtension.UserRole_Admin) 
                || User.IsInRole(Core.UserExtension.UserRole_UserAdmin)
                )
            {
                userSubGroupId = toplevelsubgroup;
            }

            ViewData["datefrom"] = datefrom;
            ViewData["dateto"] = dateto;
            ViewData["CompanyModel"] = _reportrepo.GetOwnCompany(User.GetCompanyID());
            if (userSubGroupId.HasValue)
                ViewData["UserSubGroupName"] = _companyRep.GetUserSubGroupName(userSubGroupId.Value);
            else
                ViewData["UserSubGroupName"] = string.Empty;

            var model = await _reportrepo.GetUserFinancePeriodReportWithGroup(datefrom.Value, dateto.Value, User.GetCompanyID(), userSubGroupId);
            return PartialView(model);
        }
        public async Task<FileResult> DeliveryReport(DateTime dayDate)
        {
            //Response.ContentType = "text/csv";
            return await CsvReportFromSQL($"exec DeliveryReport '{dayDate.ShortSqlDate()}' , {User.GetCompanyID()}",$"deliveryreport{dayDate.ShortSqlDate()}");

        
        }
        public async Task<FileResult> DeliveryReportSummary(DateTime dayDate)
        {
            //Response.ContentType = "text/csv";
            return await CsvReportFromSQL($"exec DeliveryReportSummary '{dayDate.ShortSqlDate()}' , {User.GetCompanyID()}", $"deliveryreportsummary{dayDate.ShortSqlDate()}");


        }


        [AllowAnonymous]
        public async Task<IActionResult> OrderPeriodDetailReport(DateTime? dateFrom, DateTime? dateTo, int? companyId)
        {
            if(string.IsNullOrEmpty( Request.Headers["Authorization"]))
                 return StatusCode((int)HttpStatusCode.Unauthorized);
            var autorize=await _companyRep.ValidateBasicAuthAsync(Request.Headers["Authorization"]);
            if (!autorize)
                return StatusCode((int)HttpStatusCode.Unauthorized);
            Response.Headers["ContentType"] = "application/json";
            return Content(await _reportrepo.OrderPeriodDetailReportAsync(dateFrom, dateTo, companyId));
        }
        private async Task<bool> IsBaseAutorizedAsync()
        {
            if (string.IsNullOrEmpty(Request.Headers["Authorization"]))
                return false;
            var autorize = await _companyRep.ValidateBasicAuthAsync(Request.Headers["Authorization"]);
            return autorize;

           
        }
            // [AllowAnonymous]
          public async Task<IActionResult> SQLRawJsonData(string sql)
        {
            // to do unsecure check injection
            return Content("");
            var jsonstring= await _context.Database.JsonWriter(sql).ToStringAsync();
            Response.Headers["ContentType"]="application/json";
            return Content(jsonstring);
        }
        private async Task<FileResult> CsvReportFromSQL(string sql,string filename)
        {
            Response.Headers.Add("Content-disposition", $"attachment;filename={filename}.csv");
            using (var ms = new MemoryStream())
            {
                try
                {
                    using (var sw = new StreamWriter(ms, new UTF8Encoding(true)))
                    {
                        await _context.Database.CSVWriter(sql).ToStreamAsync(sw);
                        FileContentResult fs = new FileContentResult(ms.GetBuffer(), new MediaTypeHeaderValue("text/csv"));
                        return fs;
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"CsvReportFromSQL sql {sql}");
                    return null;
                }

            }
        }
        //excel
        public async Task<FileResult> OrderFinDetails(DateTime? dateFrom, DateTime? dateTo, int? companyId)
        {
            int companyid = User.GetCompanyID();
            if (companyId.HasValue)
                companyid = companyId.Value;
            return await _reportrepo.ExcelReport("OrderFinDetails", dateFrom, dateTo, companyid);
         }
    }
}
