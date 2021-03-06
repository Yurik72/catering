﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Amount")]
        public decimal Value { get; set; }
        [DisplayName("Type")]
        public int Type { get; set; }

        [DisplayName("DateFrom")]
        public DateTime? DateFrom { get; set; }

        [DisplayName("DateTo")]
        public DateTime? DateTo { get; set; }

        [DisplayName("Categories")]
        public string Categories { get; set; }

        [NotMapped]
        public DiscountJson DiscountJson { get; set; }
    }
}
