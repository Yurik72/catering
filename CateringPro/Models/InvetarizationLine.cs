using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class InventarizationLines : CompanyDataOwnId
    {

        public int InventarizationId { get; set; }

        public int Number { get; set; }

        public int IngredientsId { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal InventarizationQuantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Differance { get; set; }


        public virtual Inventarization Inventarization { get; set; }

        public virtual Ingredients Ingredients { get; set; }


    }
}
