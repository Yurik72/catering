using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
namespace CateringPro.ViewModels
{


    public class EmailTemplateViewModel
    {
        public EmailTemplateViewModel()
        {
            Models = new Dictionary<DateTime, dynamic>();
        }
        public Company Company { get; set; }
        public CompanyUser User { get; set; }
        public  string Message { get; set; }

        public string Greeting { get; set; }

        public  Dictionary<DateTime, dynamic> Models;
    }
}
