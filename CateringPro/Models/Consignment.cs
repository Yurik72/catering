using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class Consignment: CompanyData
    {
        public int LineId { get; set; }

        public int IngredientsId { get; set; }

        public decimal InitialQuantity { get; set; }
        public decimal Quantity { get; set; }
        
        public  decimal Price { get; set; }

        public DateTime ValidUntil { get; set; }

        public virtual Ingredients Ingredients { get; set; }

    }
}
