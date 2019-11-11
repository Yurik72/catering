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
        public async Task<IActionResult> CompanyProductionForecast(DateTime daydate, string format)
        {
            daydate = DateTime.Now;
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
            return View(await _reportrepo.CompanyProductionForecast(daydate, User.GetCompanyID()));
        }
 
     
       
     

     

      
    }
}