using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class DayProductioDayViewModel
    {
        public CompanyModel Company { get; set; }

      

        public IEnumerable<DayProductionViewModel> Days { get; set; }
    }
    public class DayProductionViewModel
    {
        public CompanyModel Company { get; set; }

        public DateTime DayDate { get; set; }
        public IEnumerable<DayProductionDishViewModel> Items { get; set; }

    }
    public class DayProductionDishViewModel
    {
        public string DishCode { get; set; }
        public int DishId { get; set; }
        public string ComplexCode { get; set; }
        public string ComplexName { get; set; }
        public string DishName { get; set; }
        public int Quantity { get; set; }
        public IEnumerable<DayIngredientsDetails> Ingridients { get; set; }
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
        public DayIngredientsDetails(int id, string name, decimal quantity, string measure) {
            IngredientId = id;
            IngredientName = name;
            Quantity = quantity;
            MeasureUnit = measure;
        }
        public DayIngredientsDetails() { }

    }

}
