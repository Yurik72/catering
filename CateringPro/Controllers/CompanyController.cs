using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System.IO;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.Extensions.Logging;
using CateringPro.Core;
using Microsoft.AspNetCore.Authorization;

namespace CateringPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompanyController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ILogger<CompanyUser> _logger;
        public CompanyController(AppDbContext context, ILogger<CompanyUser> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Setting()
        {
            Company comp = await _context.Companies.FindAsync(User.GetCompanyID());
            if (comp == null)
                return View("Error");
            return View(comp);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Setting(Company model)
        {
            if (User.GetCompanyID() != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Company setting");
                    return NotFound();

                }
                return View(model);
            }
            return View(model);
        }
    }
}
