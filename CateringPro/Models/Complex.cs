using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CateringPro.Models
{
    public class Complex: CompanyDataOwnId
    {
        public Complex()
        {
            DishComplex = new HashSet<DishComplex>();
        }

        // public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Required]
        [DisplayName("Complex Name")]
        public string Name { get; set; }

        [DisplayName("Complex Price")]
        public decimal Price { get; set; }

        [DisplayName("DishesQuantity")]
        public int DishesQuantity { get; set; }

        [DisplayName("Complex Dishes")]
        public virtual ICollection<DishComplex> DishComplex { get; set; }
       
    }
}
