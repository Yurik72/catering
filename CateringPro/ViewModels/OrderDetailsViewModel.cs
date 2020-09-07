using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class OrderDetailsViewModel
    {
        public DateTime Date { get; set; }
        public string  GroupName { get; set; }
        public string ChildNameSurname { get; set; }
        public string Category { get; set; }
        public string DishName { get; set; }
        public string DishKind { get; set; }

        public bool IsDelivered { get; set; }

    }
}
