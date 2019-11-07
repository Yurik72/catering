using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.Core;


namespace CateringPro.ViewComponents
{using System.Threading.Tasks;
    public class UserDayDishComponent: ViewComponent
    {
        private readonly IUserDayDishesRepository _udaydishrepo;
        private readonly UserManager<CompanyUser> _userManager;
        public UserDayDishComponent( IUserDayDishesRepository udaydishrepo, UserManager<CompanyUser> userManager)
        {
            _udaydishrepo = udaydishrepo;
            _userManager = userManager;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(DateTime daydate)
        {

            //  daydate = DateTime.Now;
            //var cid = this.User.GetCompanyID();
            //return View(_daydishrepo.DishesPerDay(daydate).ToList());
            ViewData["AllowEdit"] = _udaydishrepo.IsAllowDayEdit(daydate, this.User.GetCompanyID());
            return await Task.FromResult((IViewComponentResult)View("Default", _udaydishrepo.CategorizedDishesPerDay(daydate, this.User.GetUserId(), this.User.GetCompanyID()))); //to do
        }
    }
}
