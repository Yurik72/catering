using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class UserDay: UserData
    {
        public DateTime Date { get; set; }



        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPaid { get; set; }

        public bool IsConfirmed { get; set; }

        public bool IsPaid { get; set; }
    }
}
