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
using System.Configuration;

namespace CateringPro.Controllers
{
    [Authorize(Roles = "Admin,CompanyAdmin")]
    public class DiscountController : GeneralController<Discount>
    {
        private readonly AppDbContext _context;
        private readonly IGenericModelRepository<Discount> _discountRepo;
        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
        private int pageRecords = 20;
        public DiscountController(AppDbContext context, IGenericModelRepository<Discount> discountRepo, ILogger<CompanyUser> logger, IConfiguration Configuration) :
            base(context, discountRepo, logger,Configuration)
        {
            _context = context;
            _discountRepo = discountRepo;
            _logger = logger;
            _configuration = Configuration;
            int.TryParse(_configuration["SQL:PageRecords"], out pageRecords);

        }

        //GET: Categories
        public IActionResult Index()
        {
            return View(new List<Discount>());
        }

        public async Task<IActionResult> ListItems(QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {
            //QueryModel querymodel = new QueryModel() { }

            var query = this.GetQueryList(_context.Discounts,
                querymodel,
                d => d.Name.Contains(querymodel.SearchCriteria)
                || d.Code.Contains(querymodel.SearchCriteria),
                pageRecords);

            return PartialView(/*await query.ToListAsync()*/);

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditModal(int id, /*[Bind("Id,Code,Value,Type,DateFrom,DateTo,Categories")]*/ Discount disc)
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

            var adr = await _context.Discounts.FindAsync(id);
            if (adr == null)
            {
                return NotFound();
            }
            
            return PartialView(adr);
        }
       

        

       


    }
}
