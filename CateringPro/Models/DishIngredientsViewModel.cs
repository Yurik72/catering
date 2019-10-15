using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class DishIngredientsViewModel
    {
        public List<string> IngredientsIds { get; set; }

        
        public MultiSelectList Ingredients { get; set; }
    }
}
