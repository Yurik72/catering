using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CateringPro.Repositories
{
    public class DayDishesRepository : IDayDishesRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        public DayDishesRepository(AppDbContext context,  ILogger<CompanyUser> logger)
        {
            _context = context;
            _logger = logger;
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
            var query1 = from dish in _context.Dishes.Where(d=>d.CompanyId== companyid)
                         join dd in _context.DayDish.Where(d => d.CompanyId == companyid && d.Date== daydate) on dish.Id equals  dd.DishId  into Details
                         from dayd in Details.DefaultIfEmpty()
                         select new DayDishViewModel
                         {
                             DishId = dish.Id,
                             CategoryId = dish.CategoriesId,
                             PictureId=dish.PictureId,
                             DishName = dish.Name,
                             Date = daydate,
                             Enabled = dayd.Date == daydate,/*dayd != null*/
                                                            //CatId= cat.Id,
                                                            // CatName=cat.Name,
                                                            //CatCode=cat.Code
                         };
            var query2 = from cat in _context.Categories
                         select new DayDishViewModelPerGategory()
                         {
                             CategoryCode = cat.Code,
                             CategoryName = cat.Name,
                             DayDishes=from dd in query1.Where(q=>q.CategoryId==cat.Id) select new DayDishViewModel()
                             {
                                 Date=dd.Date,
                                 DishId = dd.DishId,
                                 DishName = dd.DishName,
                                 PictureId = dd.PictureId,
                                 Enabled = dd.Enabled
                             }
                         };
                /* !! not more working on EF 3.0*/
                /*
                var query = from entry in
                        (
                            from dish in _context.Dishes
                            where  dish.CompanyId == companyid           
                            //join cat in _context.Categories on dish.CategoriesId equals cat.Id
                            join dd in (from subday in _context.DayDish where subday.Date == daydate && subday.CompanyId == companyid select subday ) on dish.Id equals dd.DishId into Details

                            from dayd in Details.DefaultIfEmpty()
                            select new {
                                DishId = dish.Id, 
                                CategoryID = dish.CategoriesId,
                                DishName = dish.Name, 
                                Date = daydate, 
                                Enabled = dayd.Date== daydate
                            }
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

            
            */
            return query2;
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
