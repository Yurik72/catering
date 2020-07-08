using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.Data;
using CateringPro.Core;
using CateringPro.ViewModels;
using Microsoft.Extensions.Configuration;

namespace CateringPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserGroupsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUserGroupsRepository _usrGrRepo;
        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        private int pageRecords = 20;
        public UserGroupsController(AppDbContext context, IUserGroupsRepository usrGrRepo, ILogger<CompanyUser> logger, IConfiguration Configuration)
        {
            _context = context;
            _usrGrRepo = usrGrRepo;
            _logger = logger;
            _configuration = Configuration;
            int.TryParse(_configuration["SQL:PageRecords"], out pageRecords);

        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _usrGrRepo.GetAllAsync());
        }

        public async Task<IActionResult> ListItems([Bind("SearchCriteria,SortField,SortOrder,Page")]  QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {
            //QueryModel querymodel=new QueryModel() { }
            ViewData["QueryModel"] = querymodel;

            var query = (IQueryable<UserGroups>)_context.UserGroups.WhereCompany(User.GetCompanyID());
            
            if (!string.IsNullOrEmpty(querymodel.SearchCriteria))
            {
                query = query.Where(d => d.Name.Contains(querymodel.SearchCriteria) || d.Description.Contains(querymodel.SearchCriteria));
                

            }
            if (!string.IsNullOrEmpty(querymodel.SortField))
            {
                query = query.OrderByEx(querymodel.SortField, querymodel.SortOrder);
            }
            if (querymodel.Page > 0)
            {
                query = query.Skip(pageRecords * querymodel.Page);
            }
            return PartialView(await query.ToListAsync());

        }
        [HttpPost]
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Code,Name,Price,Description,CategoriesId")] UserGroups usrGr)
        {
            if (id != usrGr.Id)
            {
                return NotFound();
            }
            return await this.UpdateCompanyDataAsync(usrGr, _context, _logger);

        }


        public async Task<IActionResult> EditModal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usrGr = await _context.UserGroups.FindAsync(id);
            if (usrGr == null)
            {
                return NotFound();
            }
            
            return PartialView(usrGr);
        }
        public IActionResult CreateModal()
        {

            var usrGr = new UserGroups();
            if (usrGr == null)
            {
                return NotFound();
            }
            
            return PartialView("EditModal", usrGr);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userGroups = await _usrGrRepo.GetByIdAsync(id);

            if (userGroups == null)
            {
                return NotFound();
            }

            return View(userGroups);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] UserGroups userGroups)
        {
            if (ModelState.IsValid)
            {
                _usrGrRepo.Add(userGroups);
                await _usrGrRepo.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(userGroups);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userGroups = await _usrGrRepo.GetByIdAsync(id);

            if (userGroups == null)
            {
                return NotFound();
            }
            return View(userGroups);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] UserGroups userGroups)
        {
            if (id != userGroups.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _usrGrRepo.Update(userGroups);
                    await _usrGrRepo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                        return NotFound();
                }
                return RedirectToAction("Index");
            }
            return View(userGroups);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userGroups = await _usrGrRepo.GetByIdAsync(id);

            if (userGroups == null)
            {
                return NotFound();
            }

            return View(userGroups);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userGroups = await _usrGrRepo.GetByIdAsync(id);
            _usrGrRepo.Remove(userGroups);
            await _usrGrRepo.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
