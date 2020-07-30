using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CateringPro.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string ReturnUrl)
        {
            //return RedirectToAction("Index", "UserDayDish", new { area = "Admin" });
            ViewData["NotRenderMainContainer"] = true;
            if(!string.IsNullOrEmpty(ReturnUrl) )
                ViewData["autoLogon"] = true;
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
    }
}
