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

        public IQueryable<DayDishViewModel> DishesPerDay(DateTime daydate, int companyid)
        {
            var query = from dish in _context.Dishes
                        join dd in (from subday in _context.DayDish where subday.Date == daydate && subday.CompanyId == companyid select subday) on dish.Id equals dd.DishId into proto
                        from dayd in proto.DefaultIfEmpty()

                        select new DayDishViewModel() { DishId = dish.Id, DishName = dish.Name, Date = daydate, Enabled = proto.Count() > 0/*dayd != null*/ };
            return query;
        }
        public IQueryable<DayDishViewModelPerGategory> CategorizedDishesPerDay(DateTime daydate, int companyid)
        {
            var query = from entry in
                        (
                            from dish in _context.Dishes
                            where  dish.CompanyId == companyid
                            //join cat in _context.Categories on dish.CategoriesId equals cat.Id
                            join dd in (from subday in _context.DayDish where subday.Date == daydate && subday.CompanyId == companyid select subday ) on dish.Id equals dd.DishId into proto
                            from dayd in proto.DefaultIfEmpty()
                            select new { DishId = dish.Id, CategoryID = dish.CategoriesId, DishName = dish.Name, Date = daydate, Enabled = proto.Count() > 0/*dayd != null*/ }
                        )
                        group entry by entry.CategoryID into catgroup
                        join cat in _context.Categories on new { id = catgroup.Key, cid = companyid } equals new { id = cat.Id, cid = cat.CompanyId }
                        orderby cat.Code
                        select new DayDishViewModelPerGategory()
                        {
                            CategoryCode = cat.Code,
                            CategoryName = cat.Name,
                            DayDishes = from dentry in catgroup
                                        select new DayDishViewModel()
                                        {
                                            DishId = dentry.DishId,
                                            DishName = dentry.DishName,
                                            Date = dentry.Date,
                                            Enabled = dentry.Enabled
                                        }
                        };
            //                        group dish by dish.CategoriesId into catGroup
            //                        select new DayDishViewModelPerGategory() {CategoryCode=cat;

            //select new DayDishViewModel() { DishId = dish.Id, DishName = dish.Name, Date = daydate, Enabled = proto.Count() > 0/*dayd != null*/ };
            return query;
        }
        public DayDish SelectSingleOrDefault(int dishId, DateTime daydate)
        {
            return _context.DayDish.SingleOrDefault(dd => dd.DishId == dishId && dd.Date == daydate);
        }
        public DayDish SelectSingleOrDefault(DayDish src)
        {
            return _context.DayDish.SingleOrDefault(dd => dd.DishId == src.DishId && dd.Date == src.Date && dd.CompanyId == src.CompanyId);
        }
    }
}
