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
    public class IngredientsController : Controller
    {
        private readonly AppDbContext _context;
       
        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        private int pageRecords = 20;
        public IngredientsController(AppDbContext context, ILogger<CompanyUser> logger, IConfiguration Configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = Configuration;
            int.TryParse(_configuration["SQL:PageRecords"], out pageRecords);
        }

        // GET: Ingredients
        public IActionResult Index()
        {
            return View(new List<Ingredients>()); //await _context.Ingredients.WhereCompany(User.GetCompanyID()).ToListAsync());
        }
        public async Task<IActionResult> ListItems([Bind("SearchCriteria,SortField,SortOrder,Page")]  QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {
            //QueryModel querymodel=new QueryModel() { }
           // ViewData["QueryModel"] = querymodel;

            ViewData["IngredientCategoriesId"] = new SelectList(_context.IngredientCategories/*.WhereCompany(User.GetCompanyID())*/.ToList(), "Id", "Name", querymodel.RelationFilter);


            var query = this.GetQueryList(_context.Ingredients.Include(i => i.IngredientCategory),
                querymodel,
                d => d.Name.Contains(querymodel.SearchCriteria),
                pageRecords);


            return PartialView(await query.ToListAsync());

        }
        public async Task<IActionResult> SearchView([Bind("SearchCriteria,SortField,SortOrder,Page,RelationFilter")] QueryModel querymodel)
        {

            // ViewData["courseindex"] = course;
            var query = (IQueryable<Ingredients>)_context.Ingredients.Include(i => i.IngredientCategory);

            if (querymodel != null && !string.IsNullOrEmpty(querymodel.SearchCriteria))
                query = query.Where(d => d.Name.Contains(querymodel.SearchCriteria) );
            query = query.Take(pageRecords);
            return PartialView(await query.ToListAsync());

        }
        [HttpPost]
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Code,Name,MeasureUnit,StockDate,StockValue,IngredientCategoriesId")] Ingredients ing)
        {
            if (id != ing.Id)
            {
                return NotFound();
            }
            if (ing.MeasureUnit == null)
                ing.MeasureUnit = string.Empty;
            return await this.UpdateCompanyDataAsync(ing, _context, _logger);

        }


        public async Task<IActionResult> EditModal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ing = await _context.Ingredients.FindAsync(id);
            if (ing == null)
            {
                return NotFound();
            }
            ViewData["IngredientCategoriesId"] = new SelectList(_context.IngredientCategories.WhereCompany(User.GetCompanyID()).ToList(), "Id", "Name", ing.IngredientCategoriesId);
            return PartialView(ing);
        }
        public IActionResult CreateModal()
        {

            var ing = new Ingredients();
            if (ing == null)
            {
                return NotFound();
            }
            ing.StockDate = DateTime.Now;
      
            return PartialView("EditModal", ing);
        }
        // GET: Ingredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredients = await _context.Ingredients
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ingredients == null)
            {
                return NotFound();
            }

            return View(ingredients);
        }

        // GET: Ingredients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Ingredients ingredients)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingredients);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ingredients);
        }

        // GET: Ingredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredients = await _context.Ingredients.SingleOrDefaultAsync(m => m.Id == id);
            if (ingredients == null)
            {
                return NotFound();
            }
            return View(ingredients);
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Ingredients ingredients)
        {
            if (id != ingredients.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingredients);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientsExists(ingredients.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(ingredients);
        }
        [HttpGet]
        public ActionResult Search(string term, bool isShort = true)
        {
            var result = _context.Ingredients.Where(d => d.CompanyId == User.GetCompanyID()).Where(d => d.Name.Contains(term));
            if (isShort)
            {
                return Ok(result.Select(d => new { id = d.Id, name = d.Name }));
            }

            return Ok(result);


        }
        // GET: Ingredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredients = await _context.Ingredients
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ingredients == null)
            {
                return NotFound();
            }

            return PartialView(ingredients);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredients = await _context.Ingredients.SingleOrDefaultAsync(m => m.Id == id);
            _context.Ingredients.Remove(ingredients);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool IngredientsExists(int id)
        {
            return _context.Ingredients.Any(e => e.Id == id);
        }
    }
}
