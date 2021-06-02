using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class DayMenu
    {
        public DateTime Date { get; set; }
        public int DishKind { get; set; }

        public int Category { get; set; }
        public bool ShowDishes { get; set; }

        public bool ShowComplex { get; set; }

        public List<int> Categories { get; set; }

    }
   
}
