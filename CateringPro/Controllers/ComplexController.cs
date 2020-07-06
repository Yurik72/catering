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
    public class ComplexController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IComplexRepository _complexRepo;
        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        private int pageRecords = 20;
        public ComplexController(AppDbContext context, IComplexRepository complexRepo, ILogger<CompanyUser> logger, IConfiguration Configuration)
        {
            _context = context;
            _complexRepo = complexRepo;
            _logger = logger;
            _configuration = Configuration;
            int.TryParse(_configuration["SQL:PageRecords"], out pageRecords);

        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Complex.WhereCompany(User.GetCompanyID()).ToListAsync());
        }

        public async Task<IActionResult> ListItems([Bind("SearchCriteria,SortField,SortOrder,Page")]  QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {
            //QueryModel querymodel=new QueryModel() { }
            ViewData["QueryModel"] = querymodel;

            var query = (IQueryable<Complex>)_context.Complex.WhereCompany(User.GetCompanyID());
            
            if (!string.IsNullOrEmpty(querymodel.SearchCriteria))
            {
                query = query.Where(d => d.Name.Contains(querymodel.SearchCriteria) );
                

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
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Name,Price")] Complex cmp, List<string> DishesIds, List<ItemsLine> DishLine)
        {
            if (id != cmp.Id)
            {
                return NotFound();
            }
            var res=await this.UpdateCompanyDataAsync(cmp, _context, _logger);
            await _complexRepo.UpdateComplexDishes(cmp, DishesIds, User.GetCompanyID());
            return res;

        }


        public async Task<IActionResult> EditModal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _context.Complex.FindAsync(id);
            if (cat == null)
            {
                return NotFound();
            }
            
            return PartialView(cat);
        }
        public IActionResult CreateModal()
        {

            var cat = new Complex();
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



     
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var categories = await _categoryRepo.GetByIdAsync(id);

         //   if (categories == null)
           // {
          //      return NotFound();
          //  }

            return View(null);
        }
        public async Task<IActionResult> EditDishes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ComplexDishViewModel ing = new ComplexDishViewModel();
            ing.DishesIds = await _context.DishComplex.WhereCompany(User.GetCompanyID()).Where(d => d.ComplexId == id).Select(d => d.DishId.ToString()).ToListAsync();
            ing.Dishes = new MultiSelectList(await _context.Dishes.WhereCompany(User.GetCompanyID()).OrderBy(di => di.Name)
                .Select(di => new { Value = di.Id, Text = di.Name }).ToListAsync(), "Value", "Text");
            return PartialView(ing);
        }

        public async Task<IActionResult> CreateNewCourseAsync(int complexId, int course)
        {
          
            
            var itemline = new ItemsLine();
            itemline.ComplexId = complexId;
            itemline.DishCourse = course + 1;
            ViewData["courseindex"] = course;
           // itemline.DishesIds = await _context.DishComplex.WhereCompany(User.GetCompanyID()).Where(d => d.ComplexId == complexId).Where(d => d.DishCourse == course).Select(d => d.DishId.ToString()).ToListAsync();
            itemline.DishesIds = await _context.DishComplex.WhereCompany(User.GetCompanyID()).Where(d => d.ComplexId == complexId).Select(d => d.DishId.ToString()).ToListAsync();
            itemline.Dishes = new MultiSelectList(await _context.Dishes.WhereCompany(User.GetCompanyID()).OrderBy(di => di.Name)
                .Select(di => new { Value = di.Id, Text = di.Name }).ToListAsync(), "Value", "Text");

            

            return PartialView("CreateNewCourse", itemline);
        }

    }
}
