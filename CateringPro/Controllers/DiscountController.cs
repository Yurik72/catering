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
    [Authorize(Roles = "Admin,CompanyAdmin")]
    public class DiscountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IGenericModelRepository<Address> _addressRepo;
        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        private int pageRecords = 20;
        public DiscountController(AppDbContext context, IGenericModelRepository<Address> addressRepo, ILogger<CompanyUser> logger, IConfiguration Configuration)
        {
            _context = context;
            _addressRepo = addressRepo;
            _logger = logger;
            _configuration = Configuration;
            int.TryParse(_configuration["SQL:PageRecords"], out pageRecords);

        }

        // GET: Categories
        public IActionResult Index()
        {
            return View(new List<Discount>());
        }

        public async Task<IActionResult> ListItems([Bind("SearchCriteria,SortField,SortOrder,Page")]  QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {
            //QueryModel querymodel=new QueryModel() { }

            //var query = this.GetQueryList(_context.Addresses,
            //    querymodel,
            //    d => d.Name.Contains(querymodel.SearchCriteria)
            //     || d.PhoneNumber.Contains(querymodel.SearchCriteria)
            //     || d.Email.Contains(querymodel.SearchCriteria)
            //     || d.Address1.Contains(querymodel.SearchCriteria)
            //     || d.Address2.Contains(querymodel.SearchCriteria)
            //     || d.Code.Contains(querymodel.SearchCriteria),
            //    pageRecords);

            return PartialView(/*await query.ToListAsync()*/);

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Code,Value,Type,DateFrom,DateTo,Categories")] Discount disc)
        {
            if (id != disc.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return PartialView(disc);
            }
            return await this.UpdateCompanyDataAsync(disc, _context, _logger);

        }


        public async Task<IActionResult> EditModal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adr = await _context.Addresses.FindAsync(id);
            if (adr == null)
            {
                return NotFound();
            }
            
            return PartialView(adr);
        }
        public IActionResult CreateModal()
        {

            var adr = new Address();
            if (adr == null)
            {
                return NotFound();
            }
            
            return PartialView("EditModal", adr);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adresses = await _addressRepo.GetByIdAsync(id);

            if (adresses == null)
            {
                return NotFound();
            }

            return View(adresses);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,AddressType,Email,PhoneNumber,ZipCode,Country,City,Address1,Address2")] Address adr)
        {
            if (ModelState.IsValid)
            {
                _addressRepo.Add(adr);
                await _addressRepo.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(adr);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adr = await _addressRepo.GetByIdAsync(id);

            if (adr == null)
            {
                return NotFound();
            }
            return View(adr);
        }

        [HttpGet]
        public ActionResult Search(string term, bool isShort = true)
        {
            var result = _context.Addresses.Where(d => d.Name.Contains(term) || d.Code.Contains(term));
            if (isShort)
            {
                return Ok(result.Select(d => new { id = d.Id, name = d.Code+" "+d.Name }));
            }

            return Ok(result);


        }
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adr = await _addressRepo.GetByIdAsync(id);

            if (adr == null)
            {
                return NotFound();
            }

            return PartialView(adr);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adr = await _addressRepo.GetByIdAsync(id);
            _addressRepo.Remove(adr);
            await _addressRepo.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
