using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CateringPro.Controllers
{
    public class LogController : Controller
    {
        public IActionResult Index()
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string text = "";
           // string text = System.IO.File.ReadAllText(@"..\CateringPro\Logs\logs-"+date+".txt");
            string[] lines = System.IO.File.ReadAllLines(@"..\CateringPro\Logs\logs-" + date + ".txt");

            // Display the file contents by using a foreach loop.
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                text += " \r\n " + line;
                //Console.WriteLine("\t" + line);
            }
            LogsViewModel log = new LogsViewModel() { Logs = text };
            return View(log);
        }
    }
}
