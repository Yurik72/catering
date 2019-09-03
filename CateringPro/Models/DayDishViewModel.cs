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

    public class DayDishViewModelPerGategory
    {
        public DayDishViewModelPerGategory()
        {
            DayDishes = new List<DayDishViewModel>();
        }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }

        public IEnumerable<DayDishViewModel> DayDishes{ get; set;}

    }
}
