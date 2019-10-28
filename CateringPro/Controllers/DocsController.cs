﻿using System.Linq;
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

namespace CateringPro.Controllers
{
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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Docs.ToListAsync());
        }
        public async Task<IActionResult> ListItems([Bind("SearchCriteria,SortField,SortOrder,Page")]  QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {
            //QueryModel querymodel=new QueryModel() { }
            ViewData["QueryModel"] = querymodel;

            var query = (IQueryable<Docs>)_context.Docs.WhereCompany(User.GetCompanyID());

            if (!string.IsNullOrEmpty(querymodel.SearchCriteria))
            {
                query = query.Where(d => d.Description.Contains(querymodel.SearchCriteria) || d.Number.Contains(querymodel.SearchCriteria));


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
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Number,Type,Date,Amount,Description,DocLines")] Docs doc)
        {
            if (id != doc.Id)
            {
                return NotFound();
            }
            if (doc.Description == null) doc.Description = "";
           
            if (doc.Number == null) doc.Number = "";
           
            return await this.UpdateCompanyDataAsync(
                doc.ExcludeTrack(typeof(Ingredients)).IncludeCompany(typeof(DocLines)), 
                _context, _logger);

        }


        public async Task<IActionResult> EditModal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doc = await _context.Docs.Include(d=>d.DocLines).ThenInclude(dl=>dl.Ingredients).SingleOrDefaultAsync(d=>d.Id== id && d.CompanyId==User.GetCompanyID());
            if (doc == null)
            {
                return NotFound();
            }

            return PartialView(doc);
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
            docline.Number = linenum+1;
            ViewData["lineindex"] = linenum;

            return PartialView("CreateNewLine", docline);
        }
    }
}