using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CateringPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly string mediadir = @"/media";
        private readonly string mediadir_local = @"wwwroot/media";

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index(string ReturnUrl)
        {
            //return RedirectToAction("Index", "UserDayDish", new { area = "Admin" });
            ViewData["NotRenderMainContainer"] = true;
            if(!string.IsNullOrEmpty(ReturnUrl) )
                ViewData["autoLogon"] = true;
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return PartialView();
        }
        public IActionResult UserError()
        {
            return View("Error");
        }
        [Authorize(Roles = "Admin,CompanyAdmin,KitchenAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile()
        {
            if (Request.Form.Files.Count > 0)
            {
               
                var file = Request.Form.Files[0];
                using (var stream = Request.Form.Files[0].OpenReadStream())
                {

                    using (var fileStream = new FileStream(Path.Combine(mediadir_local,file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                }
            }
            return FileBrowse("");
        }
        [Authorize(Roles = "Admin,CompanyAdmin,KitchenAdmin")]
        public IActionResult FileBrowse(string search)
        {
            var searchmask = "*";
            if (!string.IsNullOrEmpty(search))
            {
                searchmask = $"*{search}*";
            }
            var files = new DirectoryInfo(mediadir_local)
                .GetFiles(searchmask)
                .OrderByDescending(f => f.Name).Select(f=>new FileViewModel()
                {
                    Name=f.Name,
                    DateLastModif=f.LastWriteTime,
                    Size=f.Length,
                    URLLink=Url.Content($"~{mediadir}/{f.Name}")
                });
            return View("FileBrowse", files);
        }
    }
}
