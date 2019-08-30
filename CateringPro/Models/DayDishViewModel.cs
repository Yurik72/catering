using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class DayDishViewModel
    {
        public DateTime Date { get; set; }

        public int DishId { get; set; }
        public string DishName { get; set; }



        public bool Enabled { get; set; }
    }
}
