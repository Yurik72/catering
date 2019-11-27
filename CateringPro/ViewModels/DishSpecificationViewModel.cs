using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class DishSpecificationViewModel
    {
        public CompanyModel Company { get; set; }
        public List<DishSpecificationItemViewModel> Items { get; set; }
    }
    public class DishSpecificationItemViewModel
    {
        public int DishId { get; set; }
        public string  DishName { get; set; }

        public List<DishIngredientsProportionViewModel> Ingredients { get; set; }
    }
}
