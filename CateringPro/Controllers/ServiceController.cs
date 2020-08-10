using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.ViewModels;
using CateringPro.Repositories;
using Microsoft.AspNetCore.Identity;
using CateringPro.Core;

namespace CateringPro.Controllers
{
    public class ServiceController : Controller
    {
        private readonly UserManager<CompanyUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IServiceRepository _servicerepo;
        public ServiceController(AppDbContext context, IServiceRepository servicerepo, UserManager<CompanyUser> userManager)
        {
            _context = context;
            _servicerepo = servicerepo;
            _userManager = userManager;
        }

        // GET: Service
        public async Task<IActionResult> Index()
        {
            //var appDbContext = _context.Dishes.Include(d => d.Category).Include(d => d.Company);
            return View();
        }
        public async Task<IActionResult> NfcCards()
        {
            var query = _userManager.Users; ;
            if (!User.IsInRole(Core.UserExtension.UserRole_Admin))
                query = query.Where(u => u.CompanyId == User.GetCompanyID());
            UserNfcCardViewModel nfcUsers = new UserNfcCardViewModel() { Users = query.ToList() };
            return View(nfcUsers);
        }
        public async Task<IActionResult> Test()
        {
            //var appDbContext = _context.Dishes.Include(d => d.Category).Include(d => d.Company);
            return View();
        }
        public async Task<IActionResult> Cards()
        {
            //var appDbContext = _context.Dishes.Include(d => d.Category).Include(d => d.Company);
            return View();
        }
        public async Task<IActionResult> CardsList(QueryModel querymodel)
        {
            //var appDbContext = _context.Dishes.Include(d => d.Category).Include(d => d.Company);
            return PartialView(await _servicerepo.GetUserCardsAsync(querymodel));
        }

        [HttpPost]
        public async Task<JsonResult> Status(ServiceRequest request)
        {
            var response = new ServiceResponse();
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> RequestForDelivery(ServiceRequest request)
        {
            var response = await _servicerepo.ProcessRequestAsync(request);
            return Json(response);
        }
    }
}
