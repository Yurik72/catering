using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class UserDayDishViewModel
    {
        public string UserId { get; set; }

        public int CompanyId { get; set; }
        public DateTime Date { get; set; }

        public int DishId { get; set; }
        public string DishName { get; set; }

        public string DishDescription { get; set; }

        public string DishIngredientds { get; set; }
        public int Quantity { get; set; }

        public bool Enabled { get; set; }

        public int CategoryId { get; set; }

        public int? PictureId { get; set; }
    }

    public class UserDayDishViewModelPerGategory
    {
        public UserDayDishViewModelPerGategory()
        {
            UserDayDishes = new List<UserDayDishViewModel>();
        }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }

        public int TotalQuantity 
        {
            get
            {
                if (UserDayDishes == null) return 0;
                return UserDayDishes.Sum(it => it.Quantity);

            }
        }
        public IEnumerable<UserDayDishViewModel> UserDayDishes { get; set;}

    }
}
