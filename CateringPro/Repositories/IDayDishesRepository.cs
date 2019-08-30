using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
namespace CateringPro.Repositories
{
    public interface IDayDishesRepository
    {

        IQueryable<DayDishViewModel> DishesPerDay(DateTime dayDate);
    }
}
