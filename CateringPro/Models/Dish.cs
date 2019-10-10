﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class Dish: CompanyDataOwnId
    {
        

        [StringLength(10)]
        [DataType(DataType.Text)]
        [Required]
        public string Code { get; set; }

        [StringLength(100, MinimumLength = 2)]
     //   [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "The field Name should only include letters and number.")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; }

        [Range(0, 1000)]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }

        [StringLength(255, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [DisplayName("Category")]
        public int CategoriesId { get; set; }

        [DisplayName("Category")]
        public virtual Categories Category { get; set; }

      
        public int? PictureId { get; set; }
        public virtual ICollection<DishCategory> DishCategories { get; set; }

        public virtual ICollection<DayDish> DayDishes { get; set; }
        public virtual ICollection<UserDayDish> UserDayDishes { get; set; }

        public virtual ICollection<DishIngredients> DishIngredients { get; set; }
    }
}
