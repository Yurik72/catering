using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class ProductionForecastViewModel
    {
        public CompanyModel Company { get; set; }
        public IEnumerable<ProductionForecastDateViewModel> Days { get; set; }

    }
    public class ProductionForecastDateViewModel
    {
        public DateTime Daydate { get; set; }

        public IEnumerable<ProductionForecastItemViewModel> Items { get; set; }
    }
    public class ProductionForecastItemViewModel
    {

        public DateTime DayDate { get; set; }
        public int CompanyId { get; set; }
        public int IngredientId { get; set; }
        public string Name { get; set; }
        
        public decimal StockValue { get; set; }

        public decimal BeginDay { get; set; }
        public decimal DayProduction { get; set; }

        public decimal ProductionQuantity { get; set; }
        public decimal AfterDayStockValue { get; set; }

        public string MeasureUnit { get; set; }
        public int IngredientCategoriesId { get; set; }
        public string IngredientCategoriesName { get; set; }
    }

   

}
