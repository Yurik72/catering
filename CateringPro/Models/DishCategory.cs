using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class DishCategory
    {
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public int CategoryId { get; set; }
        public Categories Categories { get; set; }
    }
}
