using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class Docs : CompanyDataOwnId
    {
        public Docs()
        {
            DocLines = new HashSet<DocLines>();
        }
        [DisplayName("Doc Type")]
        public int Type { get; set; }
        [DisplayName("Doc Number")]
        public string Number { get; set; }

        [DisplayName("Doc Date")]
        public DateTime Date { get; set; }

        [DisplayName("Doc Description")]
        public string Description { get; set; }

        [DisplayName("Doc Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public ICollection<DocLines> DocLines { get; set; }


        [DisplayName("Doc ProductionDate")]
        public DateTime? ProductionDay { get; set; }

        [DisplayName("Doc Address")]
        public int? AddressId { get; set; }
        [DisplayName("Doc Address")]
        public virtual Address Address { get; set; }
    }
 }
