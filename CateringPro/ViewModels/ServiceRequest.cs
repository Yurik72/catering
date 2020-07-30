using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class ServiceRequest
    {
        public DateTime DayDate { get; set; }
        public string UserId { get; set; }

        public string Type { get; set; }
        public int[] DishesNum { get; set; }
        public int[] DishesIds { get; set; }
    }
}
