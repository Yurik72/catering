using System;
using System.Collections.Generic;
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


        public string IngredientName { get; set; }
        public decimal StockValue { get; set; }

        public string MeasureUnit { get; set; }

        public IEnumerable<ConsignmentStockDetailViewModel> Consignments { get; set; }
    }
    public class ConsignmentStockDetailViewModel
    {
        public int LineId { get; set; }

        public string DocNumber { get; set; }
        public DateTime DocDate { get; set; }
        public int IngredientId { get; set; }
        public decimal StockValue { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
