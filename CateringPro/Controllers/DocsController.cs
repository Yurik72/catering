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
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CateringPro.Controllers
{
    [Authorize(Roles = "Admin,CompanyAdmin,KitchenAdmin")]
    public class DocsController : Controller
    {
     
     

        private readonly AppDbContext _context;

        private readonly ILogger<DocsController> _logger;
        private IConfiguration _configuration;
        private IDocRepository _docrepo;
        private int pageRecords = 20;
        public DocsController(AppDbContext context, ILogger<DocsController> logger, IConfiguration configuration,IDocRepository docrepo)
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


            var query = this.GetQueryList(_context.Docs.Include(d=>d.Address),
                querymodel,
               ( d => d.Description.Contains(querymodel.SearchCriteria) || d.Number.Contains(querymodel.SearchCriteria)),
                pageRecords);



            return PartialView(await query.ToListAsync());

        }
        [HttpPost]
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Number,Type,Date,Amount,Description,DocLines,AddressId")] Docs doc)
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
            var doc = await _context.Docs.Include(d=>d.Address).SingleOrDefaultAsync(d => d.Id == id && d.CompanyId == User.GetCompanyID());
            var docLines = await _context.DocLines.Where(d => d.DocsId == id && d.CompanyId == User.GetCompanyID()).Include(dl => dl.Ingredients).ToListAsync();
            docLines = docLines.OrderBy(doc => doc.Number).ToList();
         
            if (doc == null)
            {
                return NotFound();
            }
            else
            {
                doc.DocLines = docLines;
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
            
            if (doc == null)
            {
                return NotFound();
            }
            else
            {
                doc.DocLines = docLines;
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
                _logger.LogError(ex, "DeleteConfirmed");
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
        public async Task<IActionResult> CreateInvetarization()
        {
            var invent = new Inventarization();

            invent.Date = DateTime.Now;
            var ingredients = _context.Ingredients.WhereCompany(User.GetCompanyID()).ToList();
            var query = from ing in _context.Ingredients.WhereCompany(User.GetCompanyID())
                        orderby ing.Name
                        select new InventarizationLines()
                        {
                            Ingredients = ing,
                            IngredientsId = ing.Id,
                            Quantity = ing.StockValue,
                            InventarizationQuantity = ing.StockValue,
                            Differance = 0,
                            CompanyId = User.GetCompanyID()
                        };
            List<InventarizationLines> inlines = new List<InventarizationLines>();
            foreach(var ing in ingredients)
            {
                inlines.Add(new InventarizationLines
                {
                    
                    Ingredients = ing,
                    IngredientsId=ing.Id,
                    Quantity = ing.StockValue,
                    InventarizationQuantity = ing.StockValue,
                    Differance = 0,
                    CompanyId = User.GetCompanyID()
                    
                });
            }
            inlines = inlines.OrderBy(o => o.Ingredients.Name).ToList();
            invent.InventarizationLines = query.ToList();
            return PartialView("Inventarization", invent);

           
        }

        [HttpPost]
        
        public async Task<IActionResult> Inventarization(int id,Inventarization invent)
        {
            if (id != invent.Id)
            {
                return NotFound();
            }
            
            if (invent.Description == null) invent.Description = "";

            if (invent.Number == null) invent.Number = "";

            //var res = .UpdateDBCompanyDataAsync(_context, _logger, User.GetCompanyID());
            //var res = await this.UpdateDBCompanyDataAsyncEx(doc, _logger,
            //    e => { return _docrepo.UpdateDocEntity(e, User.GetCompanyID()); });
            if (!ModelState.IsValid)
            {
                var ingredients = _context.Ingredients.WhereCompany(User.GetCompanyID()).ToList();
                List<InventarizationLines> inlines = new List<InventarizationLines>();
                foreach (var ing in ingredients)
                {
                    inlines.Add(new InventarizationLines
                    {

                        Ingredients = ing,
                        IngredientsId = ing.Id,
                        Quantity = ing.StockValue,
                        InventarizationQuantity = ing.StockValue,
                        Differance = 0,
                        CompanyId = User.GetCompanyID()

                    });
                }
                inlines = inlines.OrderBy(o => o.Ingredients.Name).ToList();
                invent.InventarizationLines = inlines;
                return PartialView(invent);
            }
            Docs doc = new Docs()
            {
                Id = invent.Id,
                CompanyId=invent.CompanyId,
                Number = invent.Number,
                Description= invent.Description,
                Date = invent.Date,
                Type = invent.Type

            };
            List<DocLines> dll = new List<DocLines>();
            foreach(var invl in invent.InventarizationLines)
            {
                dll.Add(new DocLines()
                {
                    Id=invl.Id,
                    DocsId=invl.InventarizationId,
                    IngredientsId=invl.IngredientsId,
                    Quantity=invl.Differance,
                    ActualQuantity = invl.InventarizationQuantity,
                    CompanyId=invl.CompanyId,
                });
            }
            doc.DocLines = dll;
            //if (doc.Description == null) doc.Description = "";

            //if (doc.Number == null) doc.Number = "";
            var res = await this.UpdateDBCompanyDataAsyncEx(doc, _logger,
              e => { return _docrepo.UpdateDocEntity(e, User.GetCompanyID()); });
            //return await this.UpdateCompanyDataAsync(
            //    doc.ExcludeTrack(typeof(Ingredients))
            //    .IncludeCompany(typeof(DocLines))
            //    .TrackCollection(_context.DocLines.WhereCompany(User.GetCompanyID()).Where(l => l.DocsId == doc.Id), doc.DocLines),
            //    _context, _logger);
            return res;
        }
        public async Task<IActionResult> EditInventarization(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var doc = await _context.Docs.Include(d=>d.DocLines).ThenInclude(dl=>dl.Ingredients).SingleOrDefaultAsync(d=>d.Id== id && d.CompanyId==User.GetCompanyID());
            var doc = await _context.Docs.Include(d => d.Address).SingleOrDefaultAsync(d => d.Id == id && d.CompanyId == User.GetCompanyID());
            var docLines = await _context.DocLines.Where(d => d.DocsId == id && d.CompanyId == User.GetCompanyID()).Include(dl => dl.Ingredients).ToListAsync();
            docLines = docLines.OrderBy(doc => doc.Number).ToList();

            if (doc == null)
            {
                return NotFound();
            }
            
            Inventarization invent = new Inventarization()
            {
                Id= doc.Id,
                CompanyId = doc.CompanyId,
                Date= doc.Date,
                Number = doc.Number,
                Description = doc.Description,
                Type = doc.Type,
                
            };
            List<InventarizationLines> inlines = new List<InventarizationLines>();
            foreach (var ing in docLines)
            {
                decimal actQuan = 0;
                if (ing.ActualQuantity != null) {
                 actQuan= (decimal)ing.ActualQuantity;
                }
                inlines.Add(new InventarizationLines
                {

                    Ingredients = ing.Ingredients,
                    IngredientsId = ing.Id,
                    Quantity = actQuan-ing.Quantity,
                    InventarizationQuantity = actQuan ,
                    Differance = ing.Quantity,
                    CompanyId = User.GetCompanyID()

                }); 
            }
            inlines = inlines.OrderBy(o => o.Ingredients.Name).ToList();
            invent.InventarizationLines = inlines;
            return PartialView("Inventarization", invent);
        }
    }
}