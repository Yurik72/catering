using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class EmailQueue:CompanyData
    {
        public int Id { get; set; }
        [StringLength(2000)]
        [DefaultValue("")]
        public string EmailAddress { get; set; }
        [StringLength(200)]
        [DefaultValue("")]
        public string Subject { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        [DefaultValue("")]
        public string Body { get; set; }

    }
}
