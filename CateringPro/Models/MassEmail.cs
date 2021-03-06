﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public enum DistributionEnum:int
    {
        None=0,
        Admin=1,
        Users=2,
        UsersParents=3,
        All=4,
        Dedicated=5,

    }

    public class MassEmail:CompanyDataOwnId
    {
        public string Name { get; set; }
        public string Schedule { get; set; }


        [DataType(DataType.MultilineText)]
        public string TemplateText { get; set; }

        public int DistributionType { get; set; }
        [NotMapped]
        public DistributionEnum DistribType {
            get => (DistributionEnum)this.DistributionType;
            set => this.DistributionType = (int)value;
        }

        public string DistributionList { get; set; }

        public bool OnePerUser { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Subject { get; set; }
        public string Greetings { get; set; }

        public string TemplateName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public int DayFrom { get; set; }

        public int DayTo { get; set; }
        public DateTime NextSend { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("SQL or Reportname")]
        public string SQLCommand { get; set; }
    }
}
