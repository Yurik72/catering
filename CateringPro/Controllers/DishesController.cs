using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Core;
using CateringPro.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using CateringPro.ViewModels;

namespace CateringPro.Controllers
{
    public class DishesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        private readonly IDishesRepository _dishesRepo;
        private int pageRecords = 20;

        public DishesController(AppDbContext context, IDishesRepository dishesRepo,ILogger<CompanyUser> logger, IConfiguration Configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = Configuration;
            _dishesRepo = dishesRepo;
            int.TryParse(_configuration["SQL:PageRecords"], out pageRecords);
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
          
            return View(await _context.Dishes.Include(d=>d.DishIngredients).Include(d=>d.DishIngredients).ThenInclude(di=>di.Ingredient).ToListAsync());
        }
        public async Task<IActionResult> ListItems([Bind("SearchCriteria,SortField,SortOrder,Page,RelationFilter")]  QueryModel querymodel)
        {
            _logger.LogWarning("Dish Controllers  - ListItems User.GetCompanyID() {0} ", User.GetCompanyID());
            _logger.LogWarning("ListItems pageRecords {0} ", pageRecords);
            ViewData["QueryModel"] = querymodel;
            ViewData["CategoriesId"] = new SelectList(_context.Categories.ToList(), "Id", "Name", querymodel.RelationFilter);
            var query = (IQueryable<Dish>)_context.Dishes.WhereCompany(User.GetCompanyID()).Include(d=>d.Category).Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient);
            if (querymodel.RelationFilter > 0)
            {
                query = query.Where(d => d.CategoriesId == querymodel.RelationFilter);
            }
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
            query = query.Take(pageRecords);
            return PartialView(await query.ToListAsync());

        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }


        // GET: Dishes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Price,Description,CategoriesId")] Dish dish)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

   //     public async Task<IActionResult> GetDishPicture(int id)
       // {
           // var dish = await _context.Dishes.SingleOrDefaultAsync(d=>d.Id==id && d.CompanyId== this.User.GetCompanyID());
          //  if (dish == null || dish.DishPicture==null || dish.DishPicture.Length == 0)
          //      return File(new byte[0], "image/jpeg"); ;
          //  return File(dish.DishPicture, "image/jpeg");
    //    }
        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Code,Name,Price,Description,CategoriesId,PictureId")] Dish dish, List<string> IngredientsIds)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }
            if (dish.Code == null) dish.Code = "";
            if (dish.Description == null) dish.Description = "";
            if (Request.Form.Files.Count > 0)
            {
                Pictures pict = _context.Pictures.SingleOrDefault(p => p.Id == dish.PictureId);
                if (pict == null || true ) //to do always new
                 {
                    pict = new Pictures();
                }
                var file = Request.Form.Files[0];
                using (var stream = Request.Form.Files[0].OpenReadStream())
                {
                    byte[] imgdata = new byte[stream.Length];
                    stream.Read(imgdata, 0, (int)stream.Length);
                    pict.PictureData = imgdata;
                }
                _context.Add(pict);
                await _context.SaveChangesAsync();
                dish.PictureId = pict.Id;
            }

            ///not work
           // Action<Dish> postSave =async ( d) => {await this.UpdateDishIngredients(d, IngredientsIds); };
            var res=await this.UpdateCompanyDataAsync(dish, _context, _logger);
            await _dishesRepo.UpdateDishIngredients(dish, IngredientsIds,User.GetCompanyID());
            return res;
        }


        public async Task<IActionResult> EditModal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            _context.Entry(dish).Collection(d => d.DishIngredients).Query().Include(d=>d.Ingredient).Load();

            ViewData["CategoriesId"] = new SelectList(_context.Categories.ToList(), "Id", "Name", dish.CategoriesId);
            return PartialView(dish);
        }

        public async Task<IActionResult> EditIngredients(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DishIngredientsViewModel ing = new DishIngredientsViewModel();
            ing.IngredientsIds = await _context.DishIngredients.Where(d => d.DishId == id).Select(d => d.IngredientId.ToString()).ToListAsync();
            ing.Ingredients= new MultiSelectList(await  _context.Ingredients.OrderBy(di => di.Name)
                .Select(di=>new {Value=di.Id,Text=di.Name }).ToListAsync(), "Value", "Text");
            return PartialView(ing);
        }
        public IActionResult CreateModal()
        {

            var dish = new Dish();
            if (dish == null)
            {
                return NotFound();
            }
            dish.Code = _context.Categories.Count().ToString();
            ViewData["CategoriesId"] = new SelectList(_context.Categories.ToList(), "Id", "Name", dish.CategoriesId);
            return PartialView("EditModal",dish);
        }
        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
    }
}
