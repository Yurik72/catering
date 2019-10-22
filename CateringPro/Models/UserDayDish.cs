using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class UserDayDish: UserData
    {
        public DateTime Date { get; set; }

        public int DishId { get; set; }
        //public virtual ICollection<Dish> Dishes { get; set; }

        public virtual Dish Dish { get; set; }


        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
