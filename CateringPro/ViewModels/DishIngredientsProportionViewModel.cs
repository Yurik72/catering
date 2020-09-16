using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class DishIngredientsProportionViewModel
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }

        public string MeasureUnit { get; set; }
        public decimal Proportion { get; set; }
        public decimal AvgPrice { get; set; }

        public int LineIndex { get; set; }
    }
}
