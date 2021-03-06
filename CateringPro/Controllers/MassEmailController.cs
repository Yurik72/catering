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
    [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin")]
    public class MassEmailController : Controller
    {
        private readonly AppDbContext _context;
       
        private readonly ILogger<MassEmailController> _logger;
        private readonly IMassEmailRepository _mailrepo;
        private readonly IMassEmailService _massmailservice;
        private IConfiguration _configuration;
        private int pageRecords = 20;
        public MassEmailController(AppDbContext context, ILogger<MassEmailController> logger, IConfiguration Configuration, IMassEmailRepository mailrepo, IMassEmailService massmailservice)
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
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Name,Schedule,TemplateText,DistributionType,DistributionList,TemplateName,Greetings,OnePerUser,Text,Subject,DayFrom,DayTo,SQLCommand")] MassEmail em)
        {
            if (id != em.Id)
            {
                return NotFound();
            }
            if (em.DistributionList == null) em.DistributionList = "";
            if (em.TemplateText == null) em.TemplateText = "";
            if (em.Schedule == null) em.Schedule = "";
            if (em.Name == null) em.Name = "";
             em.NextSend = new DateTime(2000,1,1);


            ViewData["Templates"] = GetTemplates(em.TemplateName);
            return await this.UpdateCompanyDataAsync(em, _context, _logger);

        }
        public async Task<JsonResult> SendTest([Bind("Id,Name,Schedule,TemplateText,DistributionType,DistributionList,TemplateName,Greetings,OnePerUser,Text,Subject,DayFrom,DayTo,SQLCommand")] MassEmail em)
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

            var masEmail = await _context.MassEmail
                .SingleOrDefaultAsync(m => m.Id == id);
            if (masEmail == null)
            {
                return NotFound();
            }

            return View(masEmail);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var masEmail = await _context.MassEmail
                .SingleOrDefaultAsync(m => m.Id == id);
            if (masEmail == null)
            {
                return NotFound();
            }
            try
            {
                _context.Remove(masEmail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbex)
            {
                _logger.LogError(dbex, "DeleteConfirmed");
                return StatusCode((int)HttpStatusCode.FailedDependency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteConfirmed");
                return BadRequest();
            }
            return RedirectToAction("Index");
        }

       
    }
}
