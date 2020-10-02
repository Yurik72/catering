using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CateringPro.Data;
using CateringPro.ViewModels;
using CateringPro.Repositories;
using System;
using CateringPro.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using CateringPro.Models;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CateringPro.Controllers
{
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IServiceRepository _servicerepo;
        private readonly ICompanyUserRepository _companyuserreporepo;
        private readonly IDbSyncer _syncer;
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly SignInManager<CompanyUser> _signInManager;
        public ServiceController(AppDbContext context, IServiceRepository servicerepo, ICompanyUserRepository companyuserreporepo, IWebHostEnvironment hostingEnv, SignInManager<CompanyUser> signInManager, IDbSyncer syncer = default)
        {
            _context = context;
            _servicerepo = servicerepo;
            _companyuserreporepo = companyuserreporepo;
            _syncer = syncer;
            _hostingEnv = hostingEnv;
            _signInManager = signInManager;
        }

        // GET: Service
        [Authorize(Roles = "Admin, UserAdmin, ServiceAdmin")]
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
            return Json(new { isSuccess = true, CardTag = token, cmd = "generate" });
        }
        public async Task<JsonResult> GenUserCardTokenConfirm(string userId, string token)
        {

            var success = _companyuserreporepo.SaveUserCardTokenAsync(userId, token);
            return await Task.FromResult(Json(new { isSuccess = success, CardTag = token, cmd = "save" }));
        }
        public async Task<IActionResult> UserCardDetails(string token)
        {

            var card = await _servicerepo.GetUserCardAsync(token);
            //    if (card == null)
            //        return NotFound();

            return PartialView(card);
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
            if (!_signInManager.IsSignedIn(User))
            {
                var fail = ServiceResponse.GetFailResult();
                fail.ErrorMessage = "Для продовження видачі, необхідно повторно зайти в систему з відповідними правами доступу";
                return Json(fail);
            }
            var response = await _servicerepo.ProcessRequestAsync(request);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> GetAvailableCategories(DateTime daydate)
        {

            daydate = DateTime.Now;
            if (daydate.Ticks == 0)
            {
                daydate = DateTime.Now;
            }
            var response = await _servicerepo.GetAvailableCategories(daydate);
            return Json(response);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> GetOrdersSnapshot(int? companyid, DateTime? daydate)
        {
            return Json(await _servicerepo.GetOrdersSnapshot(companyid, daydate));
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> SyncDB()
        {
            if (_hostingEnv.EnvironmentName != "LocalProduction")
                return Json(new { State = "Not Allowed", Output = "Not in Local Mode" });
            await _syncer.SyncDb();
            return Json(new { State = "OK", Output = _syncer.GetOutput() }); ;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> LocalModeInit(int? companyId)
        {
            if (_hostingEnv.EnvironmentName != "LocalProduction")
                return Json(new { State = "Not Allowed", Output = "Not in Local Mode" });
            if (!companyId.HasValue)
                companyId = _syncer.GetDefaultCompanyId();
            await _syncer.InitialSyncByDBContext(companyId.Value, DateTime.Today.ResetHMS()); ;
            return Json(new { State = "OK", Output = _syncer.GetOutput() }); ;

        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> LocalModeOrders(int? companyId, DateTime? dayDate)
        {
            if (_hostingEnv.EnvironmentName != "LocalProduction")
                return Json(new { State = "Not Allowed", Output = "Not in Local Mode" });
            DateTime day = dayDate.HasValue ? dayDate.Value : DateTime.Now;
            if (!companyId.HasValue)
                companyId = _syncer.GetDefaultCompanyId();
            await _syncer.SyncOrders(companyId.Value, day.ResetHMS()); ;
            return Json(new { State = "OK", Output = _syncer.GetOutput() }); ;

        }

        public async Task<IActionResult> UploadDelivery(List<UserDayDish> data)
        {
            if (await _servicerepo.UploadDelivery(data))
                return Json(new { State = "OK" });
            return BadRequest();
        }

        public async Task<IActionResult> ServiceHistory(string request)
        {
            ServiceRequest servrequest = new ServiceRequest();
            try
            {
                if (!string.IsNullOrEmpty(request))
                    servrequest = JsonConvert.DeserializeObject<ServiceRequest>(request);
            }
            catch {
            }
            return View(servrequest);
        }


    }
}