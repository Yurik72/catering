using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public class DayDishesRepository : IDayDishesRepository
    {
        private readonly AppDbContext _context;

        public DayDishesRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<DayDishViewModel> DishesPerDay(DateTime daydate)
        {
            var query = from dish in _context.Dishes
                        join dd in (from subday in _context.DayDish where subday.Date == daydate select subday) on dish.Id equals dd.DishId into proto
                        from dayd in proto.DefaultIfEmpty()

                        select new DayDishViewModel() { DishId = dish.Id, DishName = dish.Name, Date = daydate, Enabled = proto.Count() > 0/*dayd != null*/ };
            return query;
        }
        public DayDish SelectSingleOrDefault(int dishId, DateTime daydate) {
            return _context.DayDish.SingleOrDefault(dd => dd.DishId == dishId && dd.Date == daydate);
       }
    }
}
