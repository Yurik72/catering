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
using System.Linq.Expressions;
using System.Reflection;

namespace CateringPro.Controllers
{
    //[Authorize(Roles = "Admin,CompanyAdmin,KitchenAdmin")]
    public class GeneralController<TModel> : Controller where TModel : CompanyDataOwnId ,new()
    {
        public readonly AppDbContext _context;
        public readonly IGenericModelRepository<TModel> _generalRepo;
        public readonly ILogger<CompanyUser> _logger;
        public IConfiguration _configuration;
        public int pageRecords = 20;
        public GeneralController(AppDbContext context, IGenericModelRepository<TModel> generakRepo, ILogger<CompanyUser> logger, IConfiguration Configuration)
        {
            _context = context;
            _generalRepo = generakRepo;
            _logger = logger;
            _configuration = Configuration;
            int.TryParse(_configuration["SQL:PageRecords"], out pageRecords);

        }

        // GET: Categories
        //public IActionResult Index()
        //{
        //    return View(new List<TModel>());
        //}

        //public async Task<IActionResult> ListItems(QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        //{
    
        //    var query = this.GetQueryList(_context.Set<TModel>(), querymodel, _generalRepo.GetContainsFilter(querymodel.SearchCriteria), pageRecords);

        //    return PartialView(await query.ToListAsync());

        //}
        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public async Task<IActionResult> EditModal(int id, TModel mod)
        //{
        //    if (id != mod.Id)
        //    {
        //        return NotFound();
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return PartialView(mod);
        //    }
        //     return await this.UpdateCompanyDataAsync(mod, _context, _logger);

        //}


        //public async Task<IActionResult> EditModal(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var adr = await _generalRepo.GetByIdAsync(id);
        //    if (adr == null)
        //    {
        //        return NotFound();
        //    }
            
        //    return PartialView(adr);
        //}
        

       
       
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mod = await _generalRepo.GetByIdAsync(id);

            if (mod == null)
            {
                return NotFound();
            }
            Type myType = typeof(TModel);
            PropertyInfo myPropInfo = myType.GetProperty("Name"); ;
            DeleteDialogViewModel del = new DeleteDialogViewModel()
            {
                CompanyId = User.GetCompanyID(),
                Id = mod.Id,
                ModelName = myPropInfo.Name,
                Name = myPropInfo.GetConstantValue().ToString()

            };

            return PartialView("~/Views/Shared/Delete.cshtml", del);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mod = await _generalRepo.GetByIdAsync(id);
            _generalRepo.Remove(mod);
            await _generalRepo.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
