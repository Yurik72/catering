﻿using System;
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

namespace CateringPro.Controllers
{
    [Authorize(Roles = "Admin,CompanyAdmin,KitchenAdmin,UserAdmin")]
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
    }
}