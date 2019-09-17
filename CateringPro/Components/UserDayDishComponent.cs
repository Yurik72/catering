using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Repositories;



namespace CateringPro.ViewComponents
{using System.Threading.Tasks;
    public class UserDayDishComponent: ViewComponent
    {
        private readonly IUserDayDishesRepository _udaydishrepo;
        private readonly UserManager<IdentityUser> _userManager;
        public UserDayDishComponent( IUserDayDishesRepository udaydishrepo, UserManager<IdentityUser> userManager)
        {
            _udaydishrepo = udaydishrepo;
            _userManager = userManager;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(DateTime daydate)
        {

          //  daydate = DateTime.Now;

            //return View(_daydishrepo.DishesPerDay(daydate).ToList());
            return await Task.FromResult((IViewComponentResult)View("Default", _udaydishrepo.CategorizedDishesPerDay(daydate, _userManager.GetUserId(HttpContext.User)))); //to do
        }
    }
}
