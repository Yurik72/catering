using Microsoft.AspNetCore.Mvc;
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
    public class DayDishComponent: ViewComponent
    {
        private readonly IDayDishesRepository _daydishrepo;
        public DayDishComponent( IDayDishesRepository daydishrepo)
        {
            _daydishrepo = daydishrepo;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(DayMenu dayMenu)
        {

          //  daydate = DateTime.Now;

            //return View(_daydishrepo.DishesPerDay(daydate).ToList());
            return await Task.FromResult((IViewComponentResult)View("Default", _daydishrepo.CategorizedDishesPerDay(dayMenu.Date, this.User.GetCompanyID())));
        }
    }
}
