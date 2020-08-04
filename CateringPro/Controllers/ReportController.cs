using System;
using Microsoft.AspNetCore.Mvc;

using jsreport.AspNetCore;
using jsreport.Types;
using System.Threading.Tasks;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.Core;
using CateringPro.ViewModels;
using System.Linq;
using System.Collections;

namespace CateringPro.Controllers
{
    public class ReportController : Controller
    {
        public IJsReportMVCService JsReportMVCService { get; }
        private readonly IReportRepository _reportrepo;
        private IStockRepository _stockrepo;
        public ReportController(IJsReportMVCService jsReportMVCService, IReportRepository rr, IStockRepository stockrepo)
        {
            JsReportMVCService = jsReportMVCService;
            _reportrepo = rr;
            _stockrepo = stockrepo;
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
       // [MiddlewareFilter(typeof(JsReportPipeline))]
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
            //test
            var rs = new jsreport.Local.LocalReporting().UseBinary(jsreport.Binary.JsReportBinary.GetBinary()).AsUtility().Create();
            var report =  rs.RenderAsync(new RenderRequest()
            {
                Template = new Template()
                {
                    Recipe = Recipe.ChromePdf,
                    Engine = Engine.None,
                    Content = "Hello from pdf",
                }
            }).Result;
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

            return View(_reportrepo.DishSpecification(datefrom, dateto, User.GetCompanyID()));
        }
        public async Task<IActionResult> ConsigmentStockAsync(DateTime datefrom, DateTime dateto, string format)
        {
            //if (datefrom.Ticks == 0)
            //{
            //    datefrom = DateTime.Today;
            //}
            //if (dateto.Ticks == 0)
            //{
            //    dateto = DateTime.Today.AddDays(3);
            //}
            //datefrom = datefrom.ResetHMS();
            //dateto = dateto.ResetHMS();
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
            //test
            /*
            var rs = new jsreport.Local.LocalReporting().UseBinary(jsreport.Binary.JsReportBinary.GetBinary()).AsUtility().Create();
            var report = rs.RenderAsync(new RenderRequest()
            {
                Template = new Template()
                {
                    Recipe = Recipe.ChromePdf,
                    Engine = Engine.None,
                    Content = "Hello from pdf",
                }
            }).Result;
            */
            var res = await _stockrepo.ConsignmentStock(new QueryModel(), User.GetCompanyID());
            
            var resGroup = res.GroupBy(it => it.IngredientCategoryId).SelectMany(c => c).ToList();
           
            return View(resGroup);
        }


    }
}