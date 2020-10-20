using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class ConsignmentMove: CompanyData
    {
        public int LineId { get; set; }

        public int LineOutId { get; set; }
        public int IngredientsId { get; set; }

        public int Type { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal Quantity { get; set; }
       

    }
}
