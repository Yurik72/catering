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
using System.Data.Entity.Core.Mapping;
using Newtonsoft.Json;

namespace CateringPro.Controllers
{
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IServiceRepository _servicerepo;
        private readonly ICompanyUserRepository _companyuserreporepo;
        public ServiceController(AppDbContext context, IServiceRepository servicerepo, ICompanyUserRepository companyuserreporepo)
        {
            _context = context;
            _servicerepo = servicerepo;
            _companyuserreporepo = companyuserreporepo;
        }

        // GET: Service
        public async Task<IActionResult> Index()
        {
            //var appDbContext = _context.Dishes.Include(d => d.Category).Include(d => d.Company);
            return View();
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
        public async Task<IActionResult> CardsList([Bind("SearchCriteria,SortField,SortOrder,Page,RelationFilter")] QueryModel querymodel)
        {
            //var appDbContext = _context.Dishes.Include(d => d.Category).Include(d => d.Company);
            return PartialView(await _servicerepo.GetUserCardsAsync(querymodel));
        }
        public async Task<JsonResult> GenUserCardToken(string userId)
        {
            var token = _companyuserreporepo.GenerateNewCardToken(userId, "", false);
            return Json(new { isSuccess = true, CardTag = token,cmd="generate" });
        }
        public async Task<JsonResult> GenUserCardTokenConfirm(string userId,string token)
        {

            var success = _companyuserreporepo.SaveUserCardTokenAsync(userId, token);
            return await Task.FromResult(Json(new { isSuccess = success, CardTag = token, cmd = "save" }));
        }
        [HttpPost]
        public async Task<JsonResult> Status(ServiceRequest request)
        {
            var response = new ServiceResponse();
            return await Task.FromResult(Json(response));
        }
        [HttpPost]
        public async Task<JsonResult> RequestForDelivery(ServiceRequest request)
        {
            var response = await _servicerepo.ProcessRequestAsync(request);
            return Json(response);
        }
    }
}
