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

namespace CateringPro.Components
{
    public class CustomerOrdersComponent : ViewComponent
    {
        private readonly IUserDayDishesRepository _udaydishrepo;
        private readonly UserManager<CompanyUser> _userManager;
        public CustomerOrdersComponent(IUserDayDishesRepository ud,UserManager<CompanyUser> userManager)
        {
            _udaydishrepo = ud;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(DateTime daydate)
        {

            //  daydate = DateTime.Now;
           
            //return View(_daydishrepo.DishesPerDay(daydate).ToList());
            return await Task.FromResult((IViewComponentResult)View("Default", _udaydishrepo.CustomerOrders(daydate, this.User.GetCompanyID()))); //to do
        }
    }
}
