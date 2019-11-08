using System.ComponentModel;

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
        public decimal Proportion { get; set; }

        public virtual Ingredients Ingredient { get; set; }
        public virtual Dish Dish { get; set; }
    }
}