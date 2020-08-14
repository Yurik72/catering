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
    [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin")]
    public class MassEmailController : Controller
    {
        private readonly AppDbContext _context;
       
        private readonly ILogger<CompanyUser> _logger;
        private readonly IMassEmailRepository _mailrepo;
        private readonly IMassEmailService _massmailservice;
        private IConfiguration _configuration;
        private int pageRecords = 20;
        public MassEmailController(AppDbContext context, ILogger<CompanyUser> logger, IConfiguration Configuration, IMassEmailRepository mailrepo, IMassEmailService massmailservice)
        {
            _context = context;
            _logger = logger;
            _configuration = Configuration;
            _mailrepo = mailrepo;
            _massmailservice = massmailservice;
            int.TryParse(_configuration["SQL:PageRecords"], out pageRecords);
        }

        // GET: Ingredients
        public async Task<IActionResult> Index()
        {
            return View(await _context.MassEmail.WhereCompany(User.GetCompanyID()).ToListAsync());
        }
        public async Task<IActionResult> ListItems([Bind("SearchCriteria,SortField,SortOrder,Page")]  QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {
            //QueryModel querymodel=new QueryModel() { }
            ViewData["QueryModel"] = querymodel;

            var query = (IQueryable<MassEmail>)_context.MassEmail.WhereCompany(User.GetCompanyID());

            if (!string.IsNullOrEmpty(querymodel.SearchCriteria))
            {
                query = query.Where(d => d.TemplateText.Contains(querymodel.SearchCriteria) );


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
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Name,Schedule,TemplateText,DistributionType,DistributionList,TemplateName,Greetings,OnePerUser,Text,Subject,DayFrom,DayTo")] MassEmail em)
        {
            if (id != em.Id)
            {
                return NotFound();
            }
            if (em.DistributionList == null) em.DistributionList = "";
            if (em.TemplateText == null) em.TemplateText = "";
            if (em.Schedule == null) em.Schedule = "";
            if (em.Name == null) em.Name = "";
            if (em.NextSend.Ticks == 0) em.NextSend = DateTime.Now;


            ViewData["Templates"] = GetTemplates(em.TemplateName);
            return await this.UpdateCompanyDataAsync(em, _context, _logger);

        }
        public async Task<JsonResult> SendTest([Bind("Id,Name,Schedule,TemplateText,DistributionType,DistributionList,TemplateName,Greetings,OnePerUser,Text,Subject,DayFrom,DayTo")] MassEmail em)
        {
            await _massmailservice.SendMassEmailToUser(User.GetCompanyID(), await _mailrepo.GetUserAsync(User.GetUserId()), em);
            return Json(0);
        }
        private IEnumerable<SelectListItem> GetTemplates(string current="Info")
        {
            //            string templates_list = "Info,DayMenu";
            //          return  templates_list.Split(",").Select(s => new SelectListItem() { Text = s, Value = s, Selected = current == s });


            return Enum.GetNames(typeof(EmailTemplateType)).Select(s => new SelectListItem() { Text = s, Value = s, Selected = current == s });

        }

        public async Task<IActionResult> EditModal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var em = await _context.MassEmail.FindAsync(id);
            if (em == null || em.CompanyId!=User.GetCompanyID())
            {
                return NotFound();
            }
            ViewData["Templates"] = GetTemplates();
            return PartialView(em);
        }
        public IActionResult CreateModal()
        {

            var em = new MassEmail() {OnePerUser = true,DayFrom=1,DayTo=1 };

            ViewData["Templates"] = GetTemplates();
            return PartialView("EditModal", em);
        }
        // GET: Ingredients/Details/5
     

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

            return View(ingredients);
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
