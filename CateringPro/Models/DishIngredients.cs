using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CateringPro.Models
{
    public class DishIngredients: CompanyData
    {
        //public int Id { get; set; }

        [DisplayName("Select Dish")]
        public int DishId { get; set; }

        [DisplayName("Select Ingredient")]
        public int IngredientId { get; set; }

        [DisplayName("Ingredients Proportion")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Proportion { get; set; }

        public virtual Ingredients Ingredient { get; set; }
        public virtual Dish Dish { get; set; }
    }
}