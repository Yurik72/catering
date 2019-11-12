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
    public class ReportParamDateRange: ViewComponent
    {
        public  async Task<IViewComponentResult> InvokeAsync(DateTime from ,DateTime to)
        {
            if (from.Ticks == 0)
                from = DateTime.Today;
            if(to.Ticks==0)
                to = DateTime.Today.AddDays(3);
            return await Task.FromResult((IViewComponentResult)View("Default", 
                new ReportParamDateRangeViewModel() {From=from,To=to}));
        }
    }
}
