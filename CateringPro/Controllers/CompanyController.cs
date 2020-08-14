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
    [Authorize(Roles = "Admin,CompanyAdmin")]
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
        public async Task<IActionResult> Setting(Company model, string orderType1)
        {
            OrderTypeEnum types;
            if (orderType1 == ""|| orderType1 == null)
            {
                types = OrderTypeEnum.None;
            }
            else
            {
               types = (OrderTypeEnum)Enum.Parse(typeof(OrderTypeEnum), orderType1);
            }
            
            if (User.GetCompanyID() != model.Id)
            {
                return NotFound();
            }
            model.OrderType = (int)types;
            if (model.Code == null) model.Code = "";
            if (model.Name == null) model.Name = "";
            if (model.City == null) model.City = "";
            if (model.Country == null) model.Country = "";
            if (model.Address1 == null) model.Address1 = "";
            if (model.Address2 == null) model.Address2 = "";
            if (model.ZipCode == null) model.ZipCode = "";
            if (model.Email == null) model.Email = "";
            if (model.Phone == null) model.Phone = "";
            if (ModelState.IsValid)
            {
              for(int i=0;i< Request.Form.Files.Count;i++)
                {
                    Pictures pict = _context.Pictures.SingleOrDefault(
                        p => p.Id == (Request.Form.Files[i].Name=="stamplogopicture"? model.StampPictureId: model.PictureId));
                    if (pict == null || true) //to do always new
                    {
                        pict = new Pictures();
                    }
                    var file = Request.Form.Files[0];
                    using (var stream = Request.Form.Files[0].OpenReadStream())
                    {
                        byte[] imgdata = new byte[stream.Length];
                        stream.Read(imgdata, 0, (int)stream.Length);
                        pict.PictureData = imgdata;
                    }
                    _context.Add(pict);
                    await _context.SaveChangesAsync();
                    if (Request.Form.Files[i].Name == "stamplogopicture")
                    {
                        model.StampPictureId = pict.Id;
                    }
                    else
                    {
                        model.PictureId = pict.Id;
                    }
                }
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
