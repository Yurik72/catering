﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class UserDayComplexViewModel
    {
        public string UserId { get; set; }

        public int CompanyId { get; set; }
        public DateTime Date { get; set; }

        public int ComplexId { get; set; }
        public string ComplexName { get; set; }

        public string ComplexDescription { get; set; }

        public int ComplexCategoryId { get; set; }

        public string ComplexCategoryName { get; set; }

        public decimal Price { get; set; }


        public int Quantity { get; set; }

        public bool Enabled { get; set; }


    
        public IEnumerable<UserDayComplexDishViewModel> ComplexDishes { get; set; }

    }
    public class UserDayComplexDishViewModel
    {
        public int DishId { get; set; }

        public string  DishName { get; set; }

       public  decimal DishReadyWeight { get; set; }

        public string DishDescription { get; set; }


        public string DishIngredients { get; set; }

        public int? PictureId { get; set; }

        public int DishCourse { get; set; }

        public int DishQuantity { get; set; }

    }

}
