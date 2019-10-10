using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CateringPro.Models
{
    public class Categories: CompanyDataOwnId
    {
        public Categories()
        {
            Dishes = new HashSet<Dish>();

          DishCategories =new  HashSet<DishCategory>();


        }


        [StringLength(10)]
        [Required]

        public string Code { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [StringLength(255, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual ICollection<Pizzas> Pizzas { get; set; }


        public virtual ICollection<Dish> Dishes { get; set; }
        
        public ICollection<DishCategory> DishCategories { get; set; }
    }
}