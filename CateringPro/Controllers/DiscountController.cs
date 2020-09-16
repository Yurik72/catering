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
using System.Text.Json;

namespace CateringPro.Controllers
{
    [Authorize(Roles = "Admin,CompanyAdmin")]
    public class DiscountController : GeneralController<Discount>
    {
        private readonly AppDbContext _context;
        private readonly IGenericModelRepository<Discount> _discountRepo;
        private readonly ILogger<CompanyUser> _logger;
        private IConfiguration _configuration;
       
        public DiscountController(AppDbContext context, IGenericModelRepository<Discount> discountRepo, ILogger<CompanyUser> logger, IConfiguration Configuration) :
            base(context, discountRepo, logger,Configuration)
        {
            _context = context;
            _discountRepo = discountRepo;
            _logger = logger;
            _configuration = Configuration;
           

        }


        /*
        public async Task<IActionResult> ListItems(QueryModel querymodel)//(string searchcriteria,string sortdir,string sortfield, int? page)
        {
            //QueryModel querymodel = new QueryModel() { }

            var query = this.GetQueryList(_context.Discounts,
                querymodel,
                d => d.Name.Contains(querymodel.SearchCriteria)
                || d.Code.Contains(querymodel.SearchCriteria),
                pageRecords);

            return PartialView(await query.ToListAsync());

        }
        */
        [ValidateAntiForgeryToken]
        [HttpPost]
        public override async Task<IActionResult> EditModal(int id, /*[Bind("Id,Code,Value,Type,DateFrom,DateTo,Categories")]*/ Discount disc)
        {
            if (id != disc.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return PartialView(disc);
            }
            if (disc.Code == null) disc.Code = "";
            //string json = JsonSerializer.Serialize(disc.Categories);
            //disc.Categories = json;
            DiscountJson json = JsonSerializer.Deserialize<DiscountJson>(disc.Categories);
            return await this.UpdateCompanyDataAsync(disc, _context, _logger);

        }


        public override async Task<IActionResult> EditModal(int? id)
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
            ViewData["CategoriesId"] = _context.Categories.OrderBy(c => c.Code)/*.WhereCompany(User.GetCompanyID())*/.ToList();
            return PartialView(adr);
        }

        public override IActionResult CreateModal()
        {

            var model = new Discount();
            if (model == null)
            {
                return NotFound();
            }
            ViewData["CategoriesId"] = _context.Categories.OrderBy(c => c.Code)/*.WhereCompany(User.GetCompanyID())*/.ToList();
            return PartialView("EditModal", model);
        }



    }
}
