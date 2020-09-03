using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
namespace CateringPro.ViewModels
{

    public class EMailAttachment
    { 
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }

    }

    public class EmailTemplateViewModel
    {
        public EmailTemplateViewModel()
        {
            Models = new Dictionary<DateTime, dynamic>();
            Attachments = new List<EMailAttachment>();
        }
        public Company Company { get; set; }
        public CompanyUser User { get; set; }
        public  string Message { get; set; }
        public string Subject { get; set; }
        public string Greeting { get; set; }

        public  Dictionary<DateTime, dynamic> Models;

        public bool JustAttachment { get; set; }
        public List<EMailAttachment> Attachments { get; private set; }
    }
}
