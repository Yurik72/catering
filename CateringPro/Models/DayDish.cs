using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CateringPro.Models
{
    public class DayDish:CompanyData
    {
        public DateTime Date { get; set; }

        public int DishId { get; set; }
        //public virtual ICollection<Dish> Dishes { get; set; }

        public virtual Dish Dish { get; set; }
    }
}
