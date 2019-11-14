using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class ConsignmentStockViewModel
    {
        public ConsignmentStockViewModel()
        {
            Consignments = new List<ConsignmentStockDetailViewModel>();
        }

        public int IngredientId { get; set; }

        [DisplayName("Consignment Ingredient Name")]
        public string IngredientName { get; set; }

        [DisplayName("Consignment StockValue")]
        public decimal StockValue { get; set; }

        [DisplayName("Consignment MeasureUnit")]
        public string MeasureUnit { get; set; }

        [DisplayName("Consignment ValidTo")]
        public DateTime ValidTo { get; set; }
        public IEnumerable<ConsignmentStockDetailViewModel> Consignments { get; set; }
    }
    public class ConsignmentStockDetailViewModel
    {
        public int LineId { get; set; }

        public string DocTypeName { get; set; }
        public string DocNumber { get; set; }
        public DateTime DocDate { get; set; }
        public int IngredientId { get; set; }
        public decimal StockValue { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
