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
    [Authorize(Roles = "Admin,CompanyAdmin,KitchenAdmin")]
    public class StockController : Controller
    {
        private readonly AppDbContext _context;
       
        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        private IStockRepository _stockrepo;
        private int pageRecords = 20;
        public StockController(AppDbContext context, ILogger<CompanyUser> logger, IConfiguration Configuration,IStockRepository stockrepo)
        {
            _context = context;
            _logger = logger;
            _configuration = Configuration;
            _stockrepo = stockrepo;
            int.TryParse(_configuration["SQL:PageRecords"], out pageRecords);
        }

        // GET: Ingredients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ingredients.ToListAsync());
        }
        public async Task<IActionResult> ListItems([Bind("SearchCriteria,SortField,SortOrder,Page")]  QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {
            //QueryModel querymodel=new QueryModel() { }
            ViewData["QueryModel"] = querymodel;

            var query = (IQueryable<Ingredients>)_context.Ingredients.WhereCompany(User.GetCompanyID());

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
            if (pageRecords > 0)
                query = query.Take(pageRecords);
            return PartialView(await query.ToListAsync());

        }

        [HttpPost]
        public async Task<IActionResult> SaveStock(int id ,decimal stockvalue)
        {
            var ing = await _context.Ingredients.FindAsync(id);
            if(ing==null || ing.CompanyId != User.GetCompanyID())
            {
                return Json(new { res = "FAIL" });
            }
            ing.StockValue = stockvalue;
            ing.StockDate = DateTime.Now;
            _context.Update(ing);
            await _context.SaveChangesAsync();
            return Json(new { res = "OK" });

        }
        [HttpPost]
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Code,Name,Price,Description,CategoriesId")] Ingredients ing)
        {
            if (id != ing.Id)
            {
                return NotFound();
            }
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

            return PartialView(ing);
        }
        public async Task<IActionResult> ConsignmentStock()
        {
            //QueryModel querymodel=new QueryModel() { }


            var query = _stockrepo.ConsignmentStock(User.GetCompanyID());

            return View(new List<ConsignmentStockViewModel>());
            return View(await query.ToListAsync());

        }
        public async Task<IActionResult> ConsignmentListItems(/*[Bind("SearchCriteria,SortField,SortOrder,Page")] */ QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {
            //QueryModel querymodel=new QueryModel() { }
            ViewData["QueryModel"] = querymodel;
            querymodel.PageRecords = pageRecords;
           
          
             return PartialView(await _stockrepo.ConsignmentStock(querymodel, User.GetCompanyID()));
           // return PartialView(query);

        }
        public async Task<IActionResult> IngredientStockDetails(int id)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {
            //QueryModel querymodel=new QueryModel() { }
            
            return PartialView(await _stockrepo.IngredientStockDetail(id, User.GetCompanyID()));

        }
        
    }
}
