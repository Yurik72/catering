﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.Core;

namespace CateringPro.ViewComponents
{using System.Threading.Tasks;
    public class DayComplexComponent: ViewComponent
    {
        private readonly IDayDishesRepository _daydishrepo;
        public DayComplexComponent( IDayDishesRepository daydishrepo)
        {
            _daydishrepo = daydishrepo;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(DateTime daydate)
        {

          //  daydate = DateTime.Now;

            //return View(_daydishrepo.DishesPerDay(daydate).ToList());
            return await Task.FromResult((IViewComponentResult)View("Default", _daydishrepo.ComplexDay(daydate,this.User.GetCompanyID())));
        }
    }
}