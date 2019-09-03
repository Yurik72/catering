using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CateringPro.Models
{
    public class Categories
    {
        public Categories()
        {
            Pizzas = new HashSet<Pizzas>();
            Dishes = new HashSet<Dish>();

          DishCategories =new  HashSet<DishCategory>();


        }

    public int Id { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "The field Name should only include letters and number.")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; }

        [StringLength(255, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual ICollection<Pizzas> Pizzas { get; set; }


        public virtual ICollection<Dish> Dishes { get; set; }
        
        public ICollection<DishCategory> DishCategories { get; set; }
    }
}