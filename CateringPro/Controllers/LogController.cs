using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            //string date = DateTime.Now.ToString("yyyyMMdd");
            //string text = "";
            try
            {
                // string text = System.IO.File.ReadAllText(@"..\wwwroot\Logs\logs-"+date+".txt");
               // string path = Path.Combine(AppContext.BaseDirectory, "Logs");
                LogsViewModel log = new LogsViewModel();
                var file = new DirectoryInfo("Logs")
                            .GetFiles("*")
                            .OrderBy(f => f.Name)
                            .LastOrDefault();
                if (file == null)
                {
                    return StatusCode((int)HttpStatusCode.OK, "");
                }
                using (var fs = new FileStream(file.FullName, FileMode.Open,
                              FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        log.Logs = sr.ReadToEnd();
                    }
                }
                //    string[] lines = System.IO.File.ReadAllLines(path + "\\logs-" + date + ".txt");

                // Display the file contents by using a foreach loop.
                // foreach (string line in lines)
                //{
                // Use a tab to indent each line of the file.
                //     text += " \r\n " + line;
                //Console.WriteLine("\t" + line);
                // }
                return StatusCode((int)HttpStatusCode.OK, log.Logs);
                //return View(log);
            }
            catch (Exception ex)
            {
                LogsViewModel log = new LogsViewModel() { Logs = ex.ToString() };
                _logger.LogError(ex, "Read log file ");
                return StatusCode((int)HttpStatusCode.InternalServerError, log.Logs);
             
            
            }
        }
    }
}
