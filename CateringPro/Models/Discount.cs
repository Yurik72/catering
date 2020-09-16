using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class Discount : CompanyDataOwnId
    {

        [StringLength(10)]
        [DataType(DataType.Text)]
        [DisplayName("Code")]
        //  [Required]
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int Type { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        [DisplayName("Categories")]
        public string Categories { get; set; }

    }
}
