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
using System;
using System.Collections.Generic;

namespace CateringPro.Controllers
{
    [Authorize(Roles = "Admin,CompanyAdmin,KitchenAdmin")]
    public class DocsController : Controller
    {
     
     

        private readonly AppDbContext _context;

        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        private IDocRepository _docrepo;
        private int pageRecords = 20;
        public DocsController(AppDbContext context, ILogger<CompanyUser> logger, IConfiguration configuration,IDocRepository docrepo)
        {

            _context = context;
            _logger = logger;
            _configuration = configuration;
            _docrepo = docrepo;
            int.TryParse(_configuration["SQL:PageRecords"], out pageRecords);
        }

        public IActionResult Index()
        {
            return View(new List<Docs>());
        }
        public async Task<IActionResult> ListItems([Bind("SearchCriteria,SortField,SortOrder,Page")]  QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {


            var query = this.GetQueryList(_context.Docs,
                querymodel,
               ( d => d.Description.Contains(querymodel.SearchCriteria) || d.Number.Contains(querymodel.SearchCriteria)),
                pageRecords);



            return PartialView(await query.ToListAsync());

        }
        [HttpPost]
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Number,Type,Date,Amount,Description,DocLines")] Docs doc)
        {
            if (id != doc.Id)
            {
                return NotFound();
            }
            foreach(var docLine in doc.DocLines)
            {
                doc.Amount += docLine.Amount;
            }
            if (doc.Description == null) doc.Description = "";
           
            if (doc.Number == null) doc.Number = "";
            
            //var res = .UpdateDBCompanyDataAsync(_context, _logger, User.GetCompanyID());
            var res = await this.UpdateDBCompanyDataAsyncEx(doc, _logger,
                e => { return _docrepo.UpdateDocEntity(e, User.GetCompanyID()); });
            if (!ModelState.IsValid)
            {
               
                var docLines = await _context.DocLines.Where(d => d.DocsId == id && d.CompanyId == User.GetCompanyID()).Include(dl => dl.Ingredients).ToListAsync();
                doc.DocLines = docLines;
                return PartialView(doc);
            }
                //return await this.UpdateCompanyDataAsync(
                //    doc.ExcludeTrack(typeof(Ingredients))
                //    .IncludeCompany(typeof(DocLines))
                //    .TrackCollection(_context.DocLines.WhereCompany(User.GetCompanyID()).Where(l => l.DocsId == doc.Id), doc.DocLines),
                //    _context, _logger);
                return res;
        }


        public async Task<IActionResult> EditModal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var doc = await _context.Docs.Include(d=>d.DocLines).ThenInclude(dl=>dl.Ingredients).SingleOrDefaultAsync(d=>d.Id== id && d.CompanyId==User.GetCompanyID());
            var doc = await _context.Docs.SingleOrDefaultAsync(d => d.Id == id && d.CompanyId == User.GetCompanyID());
            var docLines = await _context.DocLines.Where(d => d.DocsId == id && d.CompanyId == User.GetCompanyID()).Include(dl => dl.Ingredients).ToListAsync();
            doc.DocLines = docLines;
            if (doc == null)
            {
                return NotFound();
            }

            return PartialView(doc);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var doc = await _context.Docs.Include(d=>d.DocLines).ThenInclude(dl=>dl.Ingredients).SingleOrDefaultAsync(d=>d.Id== id && d.CompanyId==User.GetCompanyID());
            var doc = await _context.Docs.SingleOrDefaultAsync(d => d.Id == id && d.CompanyId == User.GetCompanyID());
            var docLines = await _context.DocLines.Where(d => d.DocsId == id && d.CompanyId == User.GetCompanyID()).Include(dl => dl.Ingredients).ToListAsync();
            doc.DocLines = docLines;
            if (doc == null)
            {
                return NotFound();
            }

            return PartialView(doc);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var doc = await _context.Docs.FindAsync(id);
                var docLine = _context.DocLines.Where(d => d.DocsId == id);
                _context.Docs.Remove(doc);
                _context.DocLines.RemoveRange(docLine);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult CreateModal()
        {

            var doc = new Docs();

            doc.Date = DateTime.Now;

            return PartialView("EditModal", doc);
        }

        public IActionResult CreateNewLine(int docId,int linenum)
        {
           
            var docline = new DocLines();
            docline.DocsId = docId;
            docline.CompanyId = User.GetCompanyID();
            docline.Number = linenum+1;
            ViewData["lineindex"] = linenum;

            return PartialView("CreateNewLine", docline);
        }
    }
}