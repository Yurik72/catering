using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class UserDayComplexViewModel
    {
        public string UserId { get; set; }

        public int CompanyId { get; set; }
        public DateTime Date { get; set; }

        public int ComplexId { get; set; }
        public string ComplexName { get; set; }

        public string ComplexDescription { get; set; }

        public string DishIngredientds { get; set; }

        public decimal Price { get; set; }
        public int KKal { get; set; }
        public decimal ReadyWeight { get; set; }
        public int Quantity { get; set; }

        public bool Enabled { get; set; }


    
        public IEnumerable<Dish> Dishes { get; set; }

    }

   
}
