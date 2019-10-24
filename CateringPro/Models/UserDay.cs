using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class UserDay: UserData
    {
        public DateTime Date { get; set; }



        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public decimal Total { get; set; }
        [DataType(DataType.Currency)]
        public decimal TotalPaid { get; set; }

        public bool IsConfirmed { get; set; }

        public bool IsPaid { get; set; }
    }
}
