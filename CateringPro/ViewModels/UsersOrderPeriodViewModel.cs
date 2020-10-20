using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class UsersOrderPeriodViewModel
    {
        public DateTime Date { get; set; }
        public string ChildNameSurname { get; set; }
        public string Category { get; set; }
        public string DishKind { get; set; }
        public bool IsDelivered { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalWithoutDiscount { get; set; }

    }
}
