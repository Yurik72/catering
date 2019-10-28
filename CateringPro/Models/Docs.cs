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
        public int Type { get; set; }

        public string Number { get; set; }
        public DateTime Date { get; set; }

        public string Description { get; set; }


        public decimal Amount { get; set; }

        public ICollection<DocLines> DocLines { get; set; }
    }
 }
