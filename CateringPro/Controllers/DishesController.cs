﻿using CateringPro.Core;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Net;

namespace CateringPro.Controllers
{
    [Authorize(Roles = "Admin,CompanyAdmin,KitchenAdmin")]
    public class DishesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DishesController> _logger;
        private IConfiguration _configuration;
        private readonly IDishesRepository _dishesRepo;
        private readonly SharedViewLocalizer _localizer;
        private int pageRecords = 20;

        public DishesController(AppDbContext context,
            IDishesRepository dishesRepo,
            ILogger<DishesController> logger,
            IConfiguration Configuration,
            SharedViewLocalizer localizer)
        {
            _context = context;
            _logger = logger;
            _configuration = Configuration;
            _dishesRepo = dishesRepo;
            _localizer = localizer;
            int.TryParse(_configuration["SQL:PageRecords"], out pageRecords);
        }

        // GET: Dishes
        public IActionResult Index()
        {
            ViewData["QueryModel"] = new QueryModel() { SortField = "Name" };
            return View(new List<Dish>());// await _context.Dishes.WhereCompany(User.GetCompanyID()).Include(d=>d.DishIngredients).Include(d=>d.DishIngredients).ThenInclude(di=>di.Ingredient).ToListAsync());
        }
        public async Task<IActionResult> ListItems([Bind("SearchCriteria,SortField,SortOrder,Page,RelationFilter")] QueryModel querymodel)
        {
            //_logger.LogWarning("Dish Controllers  - ListItems User.GetCompanyID() {0} ", User.GetCompanyID());
            //_logger.LogWarning("ListItems pageRecords {0} ", pageRecords);
          
            ViewData["QueryModel"] = querymodel;
            ViewData["CategoriesId"] = new SelectList(_context.Categories/*.WhereCompany(User.GetCompanyID())*/.ToList(), "Id", "Name", querymodel.RelationFilter);
            //var query = (IQueryable<Dish>)_context.Dishes/*.WhereCompany(User.GetCompanyID())*/.Include(d=>d.Category).Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient);
            var query = this.GetQueryList(_context.Dishes.Include(d => d.Category).Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient),
                    querymodel,
                        d => string.IsNullOrEmpty(querymodel.SearchCriteria) || d.Name.Contains(querymodel.SearchCriteria) || d.Description.Contains(querymodel.SearchCriteria),
                     pageRecords);
            if (querymodel.RelationFilter > 0)
            {
                query = query.Where(d => d.CategoriesId == querymodel.RelationFilter);
            }

            return PartialView(await query.ToListAsync());

        }
        public async Task<IActionResult> SearchView([Bind("SearchCriteria,SortField,SortOrder,Page,RelationFilter")] QueryModel querymodel)
        {

            var query = (IQueryable<Dish>)_context.Dishes.Include(d => d.Category).Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient);

            if (querymodel != null && !string.IsNullOrEmpty(querymodel.SearchCriteria))
                query = query.Where(d => d.Name.Contains(querymodel.SearchCriteria) || d.Description.Contains(querymodel.SearchCriteria));
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
        //GET: Dishes/Info/5
        //info about one dish for line 
        public async Task<IActionResult> Info(int? id)
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

            return PartialView("DishInLine", dish);
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
            return await Task.FromResult(NotFound());
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Price,Description,CategoriesId")] Dish dish)
        {

            return await Task.FromResult(NotFound());
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Code,Name,Price,Description,CategoriesId,PictureId,ReadyWeight,CookingTechnologie")] Dish dish, /*List<string> IngredientsIds,*/ List<DishIngredients> proportion)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }
            if (dish.Code == null) dish.Code = "";
            if (dish.Description == null) dish.Description = "";
            if (dish.CookingTechnologie == null) dish.CookingTechnologie = "";
            var dish_orig = await _context.Dishes.Include(d => d.DishIngredients).ThenInclude(d => d.Ingredient).AsNoTracking().Where(d => d.Id == id).FirstOrDefaultAsync();
            //_context.Entry(dish_orig).Collection(d => d.DishIngredients).Query().Include(d => d.Ingredient).Load();
            if (Request.Form.Files.Count > 0)
            {
                Pictures pict = _context.Pictures.SingleOrDefault(p => p.Id == dish.PictureId);
                if (pict == null || true) //to do always new
                {
                    pict = new Pictures();
                }
                var file = Request.Form.Files[0];
                using (var stream = Request.Form.Files[0].OpenReadStream())
                {
                    byte[] imgdata = new byte[stream.Length];
                    stream.Read(imgdata, 0, (int)stream.Length);
                    pict.PictureData = imgdata;
                    
                        PicturesController.CompressPicture(pict, 350, 350);
                   
                    //pict.PictureData = imgdata;
                }
                _context.Add(pict);
                await _context.SaveChangesAsync();
                dish.PictureId = pict.Id;
            }


            ViewData["CategoriesId"] = new SelectList(_context.Categories.WhereCompany(User.GetCompanyID()).ToList(), "Id", "Name", dish.CategoriesId);
            var res = await this.UpdateDBCompanyDataAsyncEx(dish, _logger,
                e => { return _dishesRepo.UpdateDishEntity(e, proportion, User.GetCompanyID()); });
            if (!ModelState.IsValid)
            {
                if (ModelState["Name"].Errors.Count > 0)
                {
                    ModelState["Name"].Errors.Clear();
                    ModelState["Name"].Errors.Add(_localizer["Incorrect data"]);
                }
                if (ModelState["Code"].Errors.Count > 0)
                {
                    ModelState["Code"].Errors.Clear();
                    ModelState["Code"].Errors.Add(_localizer["Incorrect data"]);
                }
                if (ModelState["Price"].Errors.Count > 0)
                {
                    ModelState["Price"].Errors.Clear();
                    ModelState["Price"].Errors.Add(_localizer["Incorrect data"]);
                }
                if (ModelState["ReadyWeight"].Errors.Count > 0)
                {
                    ModelState["ReadyWeight"].Errors.Clear();
                    ModelState["ReadyWeight"].Errors.Add(_localizer["Incorrect data"]);
                }
                if (ModelState["Description"].Errors.Count > 0)
                {
                    ModelState["Description"].Errors.Clear();
                    ModelState["Description"].Errors.Add(_localizer["Incorrect data"]);
                }


                dish.DishIngredients = dish_orig.DishIngredients;
                return PartialView(dish);
            }
          
            return res;
        }


        public IActionResult NewIngredientDishesLine(int Index, int IngredientId, string IngredientName,string IngredientMeasureUnit)
        {
            //string measure = _context.Ingredients.Where(ing => ing.Id == IngredientId).FirstOrDefault().MeasureUnit;
            return PartialView("IngredientDishesLine", new DishIngredientsProportionViewModel()
            {
                IngredientId = IngredientId,
                LineIndex = Index,
                MeasureUnit = IngredientMeasureUnit,
                Name = IngredientName
            });
        }
        public async Task<IActionResult> EditModal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null || dish.CompanyId != User.GetCompanyID())
            {
                return NotFound();
            }
            _context.Entry(dish).Collection(d => d.DishIngredients).Query().Include(d => d.Ingredient).Load();

            ViewData["CategoriesId"] = new SelectList(_context.Categories.WhereCompany(User.GetCompanyID()).ToList(), "Id", "Name", dish.CategoriesId);
            return PartialView(dish);
        }

        public async Task<IActionResult> EditIngredients(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DishIngredientsViewModel ing = new DishIngredientsViewModel();
            ing.IngredientsIds = await _context.DishIngredients.WhereCompany(User.GetCompanyID()).Where(d => d.DishId == id).Select(d => d.IngredientId.ToString()).ToListAsync();
            ing.Ingredients = new MultiSelectList(await _context.Ingredients.WhereCompany(User.GetCompanyID()).OrderBy(di => di.Name)
                .Select(di => new { Value = di.Id, Text = di.Name }).ToListAsync(), "Value", "Text");
            return PartialView(ing);
        }
        public async Task<IActionResult> EditIngredientsProportion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var model = await _context.DishIngredients.Include(d => d.Ingredient)/*.WhereCompany(User.GetCompanyID())*/.
                Where(d => d.DishId == id).Select(d => new DishIngredientsProportionViewModel()
                {
                    IngredientId = d.Ingredient.Id,
                    Name = d.Ingredient.Name,
                    MeasureUnit = d.Ingredient.MeasureUnit,
                    Proportion = d.Proportion,
                    ProportionNetto=d.ProportionNetto
                    

                }
                )
                .ToListAsync();
            return PartialView(model);
        }
        [HttpGet]
        public ActionResult Search(string term, bool isShort = true)
        {
            var result = _context.Dishes.Where(d => d.CompanyId == User.GetCompanyID()).Where(d => d.Name.Contains(term));
            if (isShort)
            {
                return Ok(result.Select(d => new { Id = d.Id, Name = d.Name, Price = d.Price }));
            }

            return Ok(result);


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
            return PartialView("EditModal", dish);
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

            return PartialView(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var dish = await _context.Dishes.FindAsync(id);
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbex)
            {
                _logger.LogError(dbex, "Delete confirmed error");
                return StatusCode((int)HttpStatusCode.FailedDependency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteConfirmed");
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
    }
}
