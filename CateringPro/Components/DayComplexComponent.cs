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
{
    using System.Threading.Tasks;
    public class DayComplexComponent : ViewComponent
    {
        private readonly IDayDishesRepository _daydishrepo;
        public DayComplexComponent(IDayDishesRepository daydishrepo)
        {
            _daydishrepo = daydishrepo;
        }

        public async Task<IViewComponentResult> InvokeAsync(DayMenu day)
        {

            var complexes = _daydishrepo.ComplexDay(day.Date, this.User.GetCompanyID());
            if (day.DishKind != 0)
            {
                complexes= complexes.Where(com => com.DishKindId == day.DishKind);
            }
            //return View(_daydishrepo.DishesPerDay(daydate).ToList());
            return await Task.FromResult((IViewComponentResult)View("Default", complexes));
        }
    }
}
