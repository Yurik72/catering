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

namespace CateringPro.Controllers
{
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

        
    }
}
