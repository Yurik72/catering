using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CateringPro.Repositories;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Core;
using Microsoft.Extensions.Logging;

namespace CateringPro.Controllers
{
    [Authorize(Roles = "Admin,CompanyAdmin,KitchenAdmin,UserAdmin")]
    public class CustomerOrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly UserManager<CompanyUser> _userManager;
        private readonly ILogger<CompanyUser> _logger;

        public CustomerOrdersController(AppDbContext context, IInvoiceRepository ir, UserManager<CompanyUser> um, ILogger<CompanyUser> logger)
        {
            _context = context;
            _userManager = um;
            _invoiceRepository = ir;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View(DateTime.Now);
        }
        public IActionResult CustomerOrderDayComponent(DateTime daydate)
        {
            object paramets = new { daydate = daydate };
            return ViewComponent("CustomerOrdersComponent", paramets);
        }
    }
}