using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class Inventarization : CompanyDataOwnId
    {
        public Inventarization()
        {
            InventarizationLines = new HashSet<InventarizationLines>();
        }
        [DisplayName("Doc Type")]
        public int Type { get; set; }
        [DisplayName("Doc Number")]
        public string Number { get; set; }

        [DisplayName("Doc Date")]
        public DateTime Date { get; set; }

        [DisplayName("Doc Description")]
        public string Description { get; set; }

        public ICollection<InventarizationLines> InventarizationLines { get; set; }


        [DisplayName("Doc ProductionDate")]
        public DateTime? ProductionDay { get; set; }

       
    }
 }
