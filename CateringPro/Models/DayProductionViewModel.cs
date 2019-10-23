using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class DayProductionViewModel
    {
        public CompanyModel Company { get; set; }
        public IEnumerable<DayProductionDishViewModel> Items { get; set; }

    }
    public class DayProductionDishViewModel
    {
        public string DishCode { get; set; }
        public string DishName { get; set; }
        public int Quantity { get; set; }
    }

    public class DayIngredientsViewModel
    {
        public CompanyModel Company { get; set; }
        public IEnumerable<DayIngredientsDetails> Items { get; set; }

    }
    public class DayIngredientsDetails
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public decimal  Quantity { get; set; }
        public int  DishQuantity { get; set; }

        public string MeasureUnit { get; set; }
    }

}
