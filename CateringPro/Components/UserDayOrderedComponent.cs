﻿using Microsoft.AspNetCore.Mvc;
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
    public class UserDayOrderedComponent: ViewComponent
    {
        private readonly IUserDayDishesRepository _udaydishrepo;
        private readonly UserManager<CompanyUser> _userManager;
        public UserDayOrderedComponent( IUserDayDishesRepository udaydishrepo, UserManager<CompanyUser> userManager)
        {
            _udaydishrepo = udaydishrepo;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(DateTime daydate)
        {

            //  daydate = DateTime.Now;
            //var cid = this.User.GetCompanyID();
            //return View(_daydishrepo.DishesPerDay(daydate).ToList());
            ViewData["AllowEdit"] = (_udaydishrepo.IsAllowDayEdit(daydate, this.User.GetCompanyID())&& _udaydishrepo.GetConfrimedAdmin(this.User.GetUserId()));
            //return await Task.FromResult((IViewComponentResult)View("Default", _udaydishrepo.ComplexPerDay(daydate, this.User.GetUserId(), this.User.GetCompanyID()))); //to do
            //return await Task.FromResult((IViewComponentResult)View("OneDayComplex", _udaydishrepo.AvaibleComplexDay(daydate, this.User.GetUserId(), this.User.GetCompanyID())));
             return await Task.FromResult((IViewComponentResult)View("OrderedComplexs", _udaydishrepo.OrderedComplexDay(daydate, this.User.GetUserId(), this.User.GetCompanyID())));
        }
    }
}
