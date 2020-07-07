using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CateringPro.Models
{
    public class UserGroups: CompanyDataOwnId
    {
        public UserGroups()
        {
            Dishes = new HashSet<Dish>();

          DishCategories =new  HashSet<DishCategory>();


        }


        [StringLength(10)]
        [Required]

        [DisplayName("Category Code")]
        public string Code { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [StringLength(255, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Category Decsciption")]
        public string Description { get; set; }

    //    public virtual ICollection<Pizzas> Pizzas { get; set; }


        public virtual ICollection<Dish> Dishes { get; set; }
        
        public ICollection<DishCategory> DishCategories { get; set; }
    }
}