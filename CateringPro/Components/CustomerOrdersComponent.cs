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
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly UserManager<CompanyUser> _userManager;
        public CustomerOrdersComponent(IInvoiceRepository ir, UserManager<CompanyUser> userManager)
        {
            _invoiceRepository = ir;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(DateTime daydate)
        {

            //  daydate = DateTime.Now;
           
            //return View(_daydishrepo.DishesPerDay(daydate).ToList());
            return await Task.FromResult((IViewComponentResult)View("Default", _invoiceRepository.CustomerOrders(daydate, this.User.GetCompanyID()))); //to do
        }
    }
}
