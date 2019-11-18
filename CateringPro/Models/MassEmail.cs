using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public enum DistibutionEnum:int
    {
        None=0,
        Admin=1,
        User=2,
        All=3
    }
    public class MassEmail:CompanyDataOwnId
    {
        public string Name { get; set; }
        public string Schedule { get; set; }


        [DataType(DataType.MultilineText)]
        public string TemplateText { get; set; }

        public int DistributionType { get; set; }
        [NotMapped]
        public DistibutionEnum DistribType {
            get => (DistibutionEnum)this.DistributionType;
            set => this.DistributionType = (int)value;
        }

        public string DistributionList { get; set; }


        public DateTime NextSend { get; set; }
    }
}
