using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class DishComplex: CompanyData
    {

        [DisplayName("Select Dish")]
        public int DishId { get; set; }

        [DisplayName("Select Complex")]
        public int ComplexId { get; set; }

        [DisplayName("Select Dish course")]
        public int DishCourse { get; set; }


        public virtual Dish Dish { get; set; }

        public virtual Complex Complex { get; set; }

    }
}
