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
{
    using Microsoft.Extensions.Caching.Memory;
    using System.Threading.Tasks;
    public class UserDayComplexComponent: ViewComponent
    {
        private readonly IUserDayDishesRepository _udaydishrepo;
        private readonly UserManager<CompanyUser> _userManager;
       
        public UserDayComplexComponent( IUserDayDishesRepository udaydishrepo, UserManager<CompanyUser> userManager)
        {
            _udaydishrepo = udaydishrepo;
            _userManager = userManager;
           
        }
        
        public async Task<IViewComponentResult> InvokeAsync(DayMenu day)
        {

            //  daydate = DateTime.Now;
            //var cid = this.User.GetCompanyID();
            //return View(_daydishrepo.DishesPerDay(daydate).ToList());
            DateTime daydate = day.Date;
            ViewData["AllowEdit"] = _udaydishrepo.IsAllowDayEdit(daydate, this.User.GetCompanyID()) && _udaydishrepo.GetConfrimedAdmin(this.User.GetUserId()) && _udaydishrepo.IsBalancePositive(this.User.GetUserId());
            ViewData["AllowAdmin"] = _udaydishrepo.GetConfrimedAdmin(this.User.GetUserId());
            ViewData["PositiveBalance"] = _udaydishrepo.IsBalancePositive(this.User.GetUserId());
            if ((_udaydishrepo.GetCompanyOrderType(this.User.GetCompanyID()) & OrderTypeEnum.OneComplexType) >0)
            {
                var complexes =  _udaydishrepo.AvaibleComplexDay(daydate, this.User.GetUserId(), this.User.GetCompanyID());
                if (day.DishKind != 0)
                {
                    complexes = complexes.Where(com => com.DishKindId == day.DishKind);
                }
                return await Task.FromResult((IViewComponentResult)View("OneDayComplex", complexes));
           }
            else
            {
                return await Task.FromResult((IViewComponentResult)View("Default", _udaydishrepo.ComplexPerDay(daydate, this.User.GetUserId(), this.User.GetCompanyID())));
            }
            // return await Task.FromResult((IViewComponentResult)View("Default", _udaydishrepo.AvaibleComplexDay(daydate, this.User.GetUserId(), this.User.GetCompanyID())));
        }

    }
}
