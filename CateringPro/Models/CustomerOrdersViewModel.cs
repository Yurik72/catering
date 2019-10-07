using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class CustomerOrdersViewModel
    {
        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public int DishesCount { get; set; }

        public decimal Amount { get; set; }
    }
}
