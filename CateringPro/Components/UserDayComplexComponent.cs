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
            ViewData["AllowEdit"] = _udaydishrepo.IsAllowDayEdit(daydate, this.User.GetCompanyID()) && _udaydishrepo.GetConfrimedAdmin(this.User.GetUserId()) 
                /*&& _udaydishrepo.IsBalancePositive(this.User.GetUserId())*/;
            ViewData["AllowAdmin"] = _udaydishrepo.GetConfrimedAdmin(this.User.GetUserId());
            //to do check balance
            //ViewData["PositiveBalance"] = _udaydishrepo.IsBalancePositive(this.User.GetUserId());
            IQueryable<UserDayComplexViewModel> complexes;
            string viewName = "Default";
            if ((_udaydishrepo.GetCompanyOrderType(this.User.GetCompanyID()) & OrderTypeEnum.OneComplexType) > 0)
            {
                complexes = _udaydishrepo.AvaibleComplexDay(daydate, this.User.GetUserId(), this.User.GetCompanyID());
                viewName = "OneDayComplex";
            }
            else
            {
                complexes = _udaydishrepo.AllComplexDay(daydate, this.User.GetUserId(), this.User.GetCompanyID());
            }
            if (day.DishKind != 0)
            {
                complexes = complexes.Where(com => com.DishKindId == day.DishKind);
            }
            if (day.Category != 0)
            {
                complexes = complexes.Where(com => com.ComplexCategoryId == day.Category);
            }
            if (day.Categories != null && day.Categories.Count > 0)
            {
                complexes = complexes.Where(com => day.Categories.Contains(com.ComplexCategoryId));
            }
            complexes = complexes.OrderBy(com => com.ComplexCategoryCode);
             return await Task.FromResult((IViewComponentResult)View(viewName, complexes));
           
                //return await Task.FromResult((IViewComponentResult)View("Default", _udaydishrepo.ComplexPerDay(daydate, this.User.GetUserId(), this.User.GetCompanyID())));
            // return await Task.FromResult((IViewComponentResult)View("Default", _udaydishrepo.AvaibleComplexDay(daydate, this.User.GetUserId(), this.User.GetCompanyID())));
        }

    }
}
