using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class DeliveryQueue:CompanyDataOwnId
    {
        public int TerminalId { get; set; }
        public DateTime DayDate { get; set; }

        [StringLength(100)]
        public string UserId { get; set; }

        public int DishId { get; set; }

        public int DishCourse { get; set; }

        public int ComplexId { get; set; }

    }
}
