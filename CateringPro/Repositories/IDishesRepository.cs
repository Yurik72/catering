using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
namespace CateringPro.Repositories
{
    public interface IDishesRepository
    {

        Task<bool> UpdateDishIngredients(Dish dish, List<string> ingredients, List<DishIngredients> proportion,int companyid);
        Task<bool> UpdateDishIngredients(Dish dish, List<DishIngredients> proportion, int companyid);
        Task<bool> UpdateDishEntity(Dish dish, List<DishIngredients> proportion, int companyid);
    }
}
