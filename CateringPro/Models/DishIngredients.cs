﻿using System.ComponentModel;

namespace CateringPro.Models
{
    public class DishIngredients
    {
        //public int Id { get; set; }

        [DisplayName("Select Dish")]
        public int DishId { get; set; }

        [DisplayName("Select Ingredient")]
        public int IngredientId { get; set; }

        public virtual Ingredients Ingredient { get; set; }
        public virtual Dish Dish { get; set; }
    }
}