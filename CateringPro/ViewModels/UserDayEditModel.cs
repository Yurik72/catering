using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class UserDayEditModel
    {
        public DateTime DayDate { get; set; }

        public DayMenu DayMenu { get; set; }

        public bool ShowDishes { get; set; }

        public bool ShowComplex { get; set; }

        public bool AllowEdit { get; set; }

       
    }
}
