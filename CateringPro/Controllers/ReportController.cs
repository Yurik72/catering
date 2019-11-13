using System;
using Microsoft.AspNetCore.Mvc;

using jsreport.AspNetCore;
using jsreport.Types;
using System.Threading.Tasks;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.Core;

namespace CateringPro.Controllers
{
    public class ReportController : Controller
    {
        public IJsReportMVCService JsReportMVCService { get; }
        private readonly IReportRepository _reportrepo;
        public ReportController(IJsReportMVCService jsReportMVCService, IReportRepository rr)
        {
            JsReportMVCService = jsReportMVCService;
            _reportrepo = rr;
        }

        public IActionResult Index()
        {
            return View();
        }

        
       // [MiddlewareFilter(typeof(JsReportPipeline))]
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
            if (format == "xlsx")
            {
                HttpContext.JsReportFeature()
               .Configure(req => req.Options.Preview = true)
               .Recipe(Recipe.HtmlToXlsx)
               .Configure((r) => r.Template.HtmlToXlsx = new HtmlToXlsx() { HtmlEngine = "chrome" });
            }
            else
            {
               // HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
            }
            return View(await _reportrepo.CompanyProductionForecast(datefrom,dateto, User.GetCompanyID()));
        }
        public  IActionResult CompanyDayProduction(DateTime datefrom, DateTime dateto, string format)
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
            if (format == "xlsx")
            {
                HttpContext.JsReportFeature()
               .Configure(req => req.Options.Preview = true)
               .Recipe(Recipe.HtmlToXlsx)
               .Configure((r) => r.Template.HtmlToXlsx = new HtmlToXlsx() { HtmlEngine = "chrome" });
            }
            else
            {
                // HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
            }
            return View( _reportrepo.CompanyDayProduction(datefrom, dateto, User.GetCompanyID()));
        }
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
            if (format == "xlsx")
            {
                HttpContext.JsReportFeature()
               .Configure(req => req.Options.Preview = true)
               .Recipe(Recipe.HtmlToXlsx)
               .Configure((r) => r.Template.HtmlToXlsx = new HtmlToXlsx() { HtmlEngine = "chrome" });
            }
            else
            {
                // HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
            }
            return View(_reportrepo.CompanyMenu(datefrom, dateto, User.GetCompanyID()));
        }



    }
}