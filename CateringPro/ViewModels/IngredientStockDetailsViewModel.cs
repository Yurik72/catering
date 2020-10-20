using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{

    public class ConsignmentDetailsViewModel
    {
         public int DocId { get; set; }
         public DateTime DocDate { get; set; }
        public string DocNumber { get; set; }
        public string DocComment { get; set; }
        public decimal InitialQuantity { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public DateTime? ValidUntil { get; set; }
        //  public List<ConsignmentM> 
    }
    public class ConsignmentMoveDetailsViewModel
    {
        public DateTime DocDate { get; set; }
        public string DocNumber { get; set; }

        public decimal Quantity { get; set; }
        public int Type { get; set; }


    }
    public class ConsignmentDocDetailsViewModel
    {
        public DateTime DocDate { get; set; }

        public DateTime? DayProduction { get; set; }
        public string DocNumber { get; set; }

        public decimal Quantity { get; set; }
        public int Type { get; set; }

        public decimal Price { get; set; }

    }
    public class IngredientStockDetailsViewModel
    {
        public Ingredients Ingredients { get; set; }
        public List<ConsignmentDetailsViewModel> ConsignmentsDetail { get; set; }
        public List<ConsignmentMoveDetailsViewModel> ConsignmentMoveDetails{ get; set; }

         public List<ConsignmentDocDetailsViewModel> ConsignmentDocDetails{ get; set; }

        public decimal TotalDocIncome => ConsignmentDocDetails.Where(d => d.Type == 1).Sum(d => d.Quantity);
        public decimal TotalDocOutcome => ConsignmentDocDetails.Where(d => d.Type != 1).Sum(d => d.Quantity);
        //  public List<ConsignmentM> 
    }
}
