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

       
        public DiscountController(AppDbContext context, IGenericModelRepository<Discount> discountRepo, ILogger<Discount> logger, IConfiguration Configuration) :
            base(context, discountRepo, logger,Configuration)
        {
           

        }


        public override void OnBeforeUpdateEntity(Discount entity)
        {
            if (entity.Code == null) entity.Code = "";
          //  DiscountJson json = JsonSerializer.Deserialize<DiscountJson>(entity.Categories);
        }

        public override void OnViewEdit(Discount entity)
        {
           // base.OnViewEdit(entity);  // no need
           
            ViewData["CategoriesId"] = _context.Categories.OrderBy(c => c.Code).ToList();

        }

    }
}
