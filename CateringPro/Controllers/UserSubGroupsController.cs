using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.Core;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Data.Common;

namespace CateringPro.Controllers
{
    [Authorize]
    public class UserSubGroupsController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ICompanyUserRepository _companyuser_repo;
        public UserSubGroupsController(AppDbContext context, ICompanyUserRepository companyuser_repo)
        {
            _context = context;
            _companyuser_repo = companyuser_repo;
        }

        // GET: UserSubGroups
        public async Task<IActionResult> Index()
        {
            //var appDbContext = _context.UserSubGroups.Include(u => u.Company).Include(u => u.Parent);

            return View(await _companyuser_repo.GetSubGroupTree(User.GetCompanyID()));
        }

        [HttpPost]
        public JsonResult Create(string name,int parentid)
        {
            var subgr = new UserSubGroup() { Name = name, CompanyId = User.GetCompanyID() };
            _context.Add(subgr);
            _context.SaveChanges();
            //return RedirectToAction("Index");
            return Json(subgr);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var subgr = _context.UserSubGroups.FirstOrDefault(u => u.Id == id);
            if (subgr == null)
                return new JsonHttpStatusResult(new { }, HttpStatusCode.InternalServerError);
            try
            {
                _context.Remove(subgr);
                _context.SaveChanges();
            }
            catch(DbUpdateException dbex)
            {
                return new JsonHttpStatusResult(new { }, HttpStatusCode.FailedDependency);
            }
            catch(Exception ex)
            {
                return new JsonHttpStatusResult(new { }, HttpStatusCode.InternalServerError);
            }
            //return RedirectToAction("Index");
            return Json(subgr);
        }
        [HttpPost]
        public JsonResult Change(int id,string name,int parentid)
        {
            try
            {
                var subgr = _context.UserSubGroups.FirstOrDefault(u => u.Id == id);
                if (subgr == null || id==parentid)
                    return new JsonHttpStatusResult(new { }, HttpStatusCode.InternalServerError);
                subgr.Name = name;
                if (parentid > 0)
                    subgr.ParentId = parentid;
                _context.Update(subgr);
                _context.SaveChanges();
                return Json(subgr);
            }
            catch 
            {
                return new JsonHttpStatusResult(new { }, HttpStatusCode.InternalServerError);
            }
        }
    }
}
