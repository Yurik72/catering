using System;
using Microsoft.AspNetCore.Mvc;

//using jsreport.AspNetCore;
//using jsreport.Types;
using System.Threading.Tasks;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.Core;

namespace CateringPro.Controllers
{
    public class InvoiceController : Controller
    {
        /*
       // public IJsReportMVCService JsReportMVCService { get; }
        private readonly IInvoiceRepository _invoicerepo;
        public InvoiceController(IJsReportMVCService jsReportMVCService, IInvoiceRepository ir)
        {
            JsReportMVCService = jsReportMVCService;
            _invoicerepo = ir;
        }

        public IActionResult Index()
        {
            return View();
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult Invoice(DateTime daydate, string userid,string format)
        {
            if (format == "xlsx")
            {
                HttpContext.JsReportFeature()
               .Configure(req => req.Options.Preview = true)
               .Recipe(Recipe.HtmlToXlsx)
               .Configure((r) => r.Template.HtmlToXlsx = new HtmlToXlsx() { HtmlEngine = "chrome" });
            }
            else
            {
                HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
            }


            return View(_invoicerepo.CustomerInvoice(userid, daydate, User.GetCompanyID()));
        }
        public IActionResult EmailInvoice(DateTime daydate, string userid)
        {


            return View(_invoicerepo.CustomerInvoice(userid, daydate, User.GetCompanyID()));
        }
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult DayProduction(DateTime daydate, string format)
        {
            if (format == "xlsx")
            {
                HttpContext.JsReportFeature()
               .Configure(req => req.Options.Preview = true)
               .Recipe(Recipe.HtmlToXlsx)
               .Configure((r) => r.Template.HtmlToXlsx = new HtmlToXlsx() { HtmlEngine = "chrome" });
            }
            else
            {
                HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
            }
            return View(_invoicerepo.CompanyDayProduction(daydate, User.GetCompanyID()));
        }
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult DayIngredients(DateTime daydate, string format)
        {
            if (format == "xlsx")
            {
                HttpContext.JsReportFeature()
               .Configure(req => req.Options.Preview = true)
               .Recipe(Recipe.HtmlToXlsx)
               .Configure((r) => r.Template.HtmlToXlsx = new HtmlToXlsx() { HtmlEngine = "chrome" });
            }
            else
            {
                HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
            }
            return View(_invoicerepo.CompanyDayIngredients(daydate, User.GetCompanyID()));
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult UserInvoice(DateTime daydate,string userid)
        {
            HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);

            return View(_invoicerepo.CustomerInvoice (userid,daydate,  User.GetCompanyID()));
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult InvoiceDownload()
        {
            HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf)
                .OnAfterRender((r) => HttpContext.Response.Headers["Content-Disposition"] = "attachment; filename=\"myReport.pdf\"");

            return View("Invoice", InvoiceModel.Example());
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> InvoiceWithHeader()
        {
            var header = await JsReportMVCService.RenderViewToStringAsync(HttpContext, RouteData, "Header", new { });

            HttpContext.JsReportFeature()
                .Recipe(Recipe.ChromePdf)
                .Configure((r) => r.Template.Chrome = new Chrome
                {
                    HeaderTemplate = header,
                    DisplayHeaderFooter = true,
                    MarginTop = "1cm",
                    MarginLeft = "1cm",
                    MarginBottom = "1cm",
                    MarginRight = "1cm"
                });

            return View("Invoice", InvoiceModel.Example());
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult Items()
        {
            HttpContext.JsReportFeature()
                .Recipe(Recipe.HtmlToXlsx)
                .Configure((r) => r.Template.HtmlToXlsx = new HtmlToXlsx() { HtmlEngine = "chrome" });

            return View(InvoiceModel.Example());
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult ItemsExcelOnline()
        {
            HttpContext.JsReportFeature()
                .Configure(req => req.Options.Preview = true)
                .Recipe(Recipe.HtmlToXlsx)
                .Configure((r) => r.Template.HtmlToXlsx = new HtmlToXlsx() { HtmlEngine = "chrome" });

            return View("Items", InvoiceModel.Example());
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult InvoiceDebugLogs()
        {
            HttpContext.JsReportFeature()
                .DebugLogsToResponse()
                .Recipe(Recipe.ChromePdf);

            return View("Invoice", InvoiceModel.Example());
        }
        */
    }
}