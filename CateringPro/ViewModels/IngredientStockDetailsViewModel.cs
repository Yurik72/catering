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

        public DateTime ValidUntil { get; set; }
        //  public List<ConsignmentM> 
    }
    public class IngredientStockDetailsViewModel
    {
        public Ingredients Ingredients { get; set; }
        public List<ConsignmentDetailsViewModel> ConsignmentsDetail { get; set; }
      //  public List<ConsignmentM> 
    }
}
