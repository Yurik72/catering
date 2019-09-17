using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
namespace CateringPro.Repositories
{
    public interface IUserDayDishesRepository
    {

        IQueryable<UserDayDishViewModel> DishesPerDay(DateTime dayDate, string userId);
        IQueryable<UserDayDishViewModelPerGategory> CategorizedDishesPerDay(DateTime daydate, string userId);

        DayDish SelectSingleOrDefault(int dishId, DateTime daydate);
    }
}

