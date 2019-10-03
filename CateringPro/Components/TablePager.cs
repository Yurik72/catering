using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.Core;
using System.Linq.Expressions;
using CateringPro.ViewModels;

namespace CateringPro.Components
{
    public class TablePager : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string field, QueryModel queryModel)
        {

            //  daydate = DateTime.Now;
            ViewData["field"] = field;
            return await Task.FromResult((IViewComponentResult)View("Default", queryModel));
        }
    }
}
