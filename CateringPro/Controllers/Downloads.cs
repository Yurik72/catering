using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CateringPro.Controllers
{
    public class Downloads : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
