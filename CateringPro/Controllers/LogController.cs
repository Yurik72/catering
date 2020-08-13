using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CateringPro.Controllers
{

    public class LogController : Controller
    {
        private readonly ILogger<CompanyUser> _logger;
        public LogController(ILogger<CompanyUser> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string text = "";
            try
            {
                // string text = System.IO.File.ReadAllText(@"..\wwwroot\Logs\logs-"+date+".txt");
                string path = Path.Combine(AppContext.BaseDirectory, "Logs");
                string[] lines = System.IO.File.ReadAllLines(path+"\\logs-" + date + ".txt");

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
            catch (Exception ex)
            {
                LogsViewModel log = new LogsViewModel() { Logs = ex.ToString() };
                _logger.LogError(ex, "Read log file ");
                return View(log);
            
            }
        }
    }
}
