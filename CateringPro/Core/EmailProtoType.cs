using CateringPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Core
{
    public class EmailProtoType
    {
        public EmailProtoType()
        {
            Attachments = new List<EMailAttachment>();
        }
        public string Subject { get; set; }
        public string Message { get; set; }
  
        public string EmailAdress { get; set; }

        public bool JustAttachment { get; set; }
        public List<EMailAttachment> Attachments { get; private set; }
    }
}
