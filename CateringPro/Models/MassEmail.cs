using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class MassEmail:CompanyDataOwnId
    {
        public string Name { get; set; }
        public string Schedule { get; set; }


        [DataType(DataType.MultilineText)]
        public string TemplateText { get; set; }

        public int DistributionType { get; set; }

        public string DistributionList { get; set; }


        public DateTime NextSend { get; set; }
    }
}
