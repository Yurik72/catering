using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class Consignment: CompanyData
    {
        public int LineId { get; set; }

        public int IngredientsId { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal InitialQuantity { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public  decimal Price { get; set; }

        public DateTime ValidUntil { get; set; }

        public virtual Ingredients Ingredients { get; set; }

    }
}
