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
using System.Net;

namespace CateringPro.Controllers
{
    //[Authorize(Roles = "Admin,CompanyAdmin,KitchenAdmin")]
    public class GeneralController<TModel> : Controller where TModel : CompanyDataOwnId ,new()
    {
        protected readonly AppDbContext _context;
        protected readonly IGenericModelRepository<TModel> _generalRepo;
        protected readonly ILogger<TModel> _logger;
        protected IConfiguration _configuration;
        protected int pageRecords = 20;
        public GeneralController(AppDbContext context, IGenericModelRepository<TModel> generakRepo, ILogger<TModel> logger, IConfiguration Configuration)
        {
            _context = context;
            _generalRepo = generakRepo;
            _logger = logger;
            _configuration = Configuration;
            int.TryParse(_configuration["SQL:PageRecords"], out pageRecords);

        }


        public virtual IActionResult Index()
        {
            return View(new List<TModel>());
        }
        public virtual async Task<IActionResult> ListItems(QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {
    
            var query = this.GetQueryList(_context.Set<TModel>(), querymodel, _generalRepo.GetContainsFilter(querymodel.SearchCriteria), pageRecords);

            return PartialView(await query.ToListAsync());

        }
        [HttpGet]
        public virtual ActionResult Search(string term, bool isShort = true)
        {
            var result = _context.Set<TModel>().Where(_generalRepo.GetContainsFilter(term));
            if (isShort)
            {
                return Ok(result.Select(d => new { id = d.Id, name = _generalRepo.GetModelFriendlyNameEx(d) }));
            }

            return Ok(result);


        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public virtual async Task<IActionResult> EditModal(int id, TModel mod)
        {
            if (id != mod.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return PartialViewEdit(mod);
            }
            return await UpdateEntityAsync(mod);
            
        }


        public virtual async Task<IActionResult> EditModal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _generalRepo.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialViewEdit(entity);
        }



        public virtual IActionResult CreateModal()
        {

            var model = new TModel();
            if (model == null)
            {
                return NotFound();
            }

            return PartialViewEdit(model);// PartialView("EditModal", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TModel mod)
        {
            if (ModelState.IsValid)
            {
                _generalRepo.Add(mod);
                await _generalRepo.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mod);
        }
        // GET: Categories/Delete/5
        public virtual async Task<IActionResult> Delete(int? id)
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
           

            return PartialView("~/Views/Shared/DeleteDialog.cshtml", _generalRepo.GetDeleteDialogViewModel(mod));
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mod = await _generalRepo.GetByIdAsync(id);
            try
            {
                _generalRepo.Remove(mod);
                await _generalRepo.SaveChangesAsync();
            }
            catch (DbUpdateException dbex)
            {
                _logger.LogError(dbex, "Delete confirmed error DbUpdateException");
                return StatusCode((int)HttpStatusCode.FailedDependency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete confirmed error");
                return BadRequest();
            }
            return RedirectToAction("Index");
        }
        public virtual async Task<IActionResult> UpdateEntityAsync(TModel entity)
        {
            return await UpdateEntityAsync(entity, null);
        }
        public virtual async Task<IActionResult> UpdateEntityAsync(TModel entity, EntityWrap<TModel> wrap = null)
        {
            if (!ModelState.IsValid)
                return PartialView(entity);
            OnBeforeUpdateEntity(entity);
            bool res = await _generalRepo.UpdateEntityAsync(entity, wrap);
            OnAfterUpdateEntity(entity);
            if (!res)
                return NotFound();

            return UpdateOk();
        }
        public virtual void OnBeforeUpdateEntity(TModel entity)
        {

        }
        public virtual void OnAfterUpdateEntity(TModel entity)
        {

        }
        public IActionResult UpdateOk()
        {
            return Json(new { res = "OK" });

        }
        public IActionResult ErrorResult( Result res)
        {
            return Json(new { res = "FAIL", reason = res.Error });
        }
        public virtual IActionResult PartialViewEdit(TModel entity,string viewName="EditModal")
        {
            OnViewEdit(entity);
            return PartialView(viewName,entity);
        }
        public virtual void OnViewEdit(TModel entity)
        {
            var selectlist = _generalRepo.GetSelectList(entity);
            foreach(var item in selectlist)
            {
                ViewBag[item.SourceField] = item.SelectList;
            }
        }
    }
}
