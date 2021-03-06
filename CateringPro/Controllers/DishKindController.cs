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
using System.Net;

namespace CateringPro.Controllers
{
    [Authorize(Roles = "Admin,CompanyAdmin,KitchenAdmin")]
    public class DishKindController : GeneralController<DishKind>
    {
        public DishKindController(AppDbContext context, IGenericModelRepository<DishKind> repo, ILogger<DishKind> logger, IConfiguration Configuration)
             : base(context, repo, logger, Configuration)
        {

        }




    }
    /*
    public class DishKindController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        private int pageRecords = 20;
        public DishKindController(AppDbContext context, ILogger<CompanyUser> logger, IConfiguration Configuration)
        {
            _context = context;
           
            _logger = logger;
            _configuration = Configuration;
            int.TryParse(_configuration["SQL:PageRecords"], out pageRecords);

        }

        // GET: Categories
        public IActionResult Index()
        {
            return View(new List<DishKind>());
        }

        public async Task<IActionResult> ListItems([Bind("SearchCriteria,SortField,SortOrder,Page")]  QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {
            //QueryModel querymodel=new QueryModel() { }

            var query = this.GetQueryList(_context.DishesKind,
                querymodel,
                d => d.Name.Contains(querymodel.SearchCriteria) || d.Description.Contains(querymodel.SearchCriteria),
                pageRecords);

            return PartialView(await query.ToListAsync());

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Code,Name,Price,Description,CategoriesId")] DishKind cat)
        {
            if (id != cat.Id)
            {
                return NotFound();
            }
            return await this.UpdateCompanyDataAsync(cat, _context, _logger);

        }


        public async Task<IActionResult> EditModal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _context.DishesKind.FindAsync(id);
            if (cat == null)
            {
                return NotFound();
            }
            
            return PartialView(cat);
        }
        public IActionResult CreateModal()
        {

            var cat = new DishKind();
            if (cat == null)
            {
                return NotFound();
            }
            
            return PartialView("EditModal", cat);
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
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] DishKind dishkind)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishkind);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dishkind);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishkind  = await _context.DishesKind.FirstOrDefaultAsync(d=>d.Id==id);

            if (dishkind == null)
            {
                return NotFound();
            }
            return View(dishkind);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] DishKind dishkind)
        {
            if (id != dishkind.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                     _context.Update(dishkind);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                        return NotFound();
                }
                return RedirectToAction("Index");
            }
            return View(dishkind);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishkind = await _context.DishesKind.FirstOrDefaultAsync(d => d.Id == id);

            if (dishkind == null)
            {
                return NotFound();
            }

            return PartialView(dishkind);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishkind = await _context.DishesKind.FirstOrDefaultAsync(d => d.Id == id);

            if (dishkind == null)
            {
                return NotFound();
            }

            try
            {
                _context.Remove(dishkind);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbex)
            {
                return StatusCode((int)HttpStatusCode.FailedDependency);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return RedirectToAction("Index");
        }


    }
    */
}
