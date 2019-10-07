using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public class UserDayDishesRepository : IUserDayDishesRepository
    {
        private readonly AppDbContext _context;

        public UserDayDishesRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<UserDayDishViewModel> DishesPerDay(DateTime daydate, string userId,int companyid)
        {
            var query = from dish in _context.Dishes
                        join dd in (from subday in _context.DayDish where subday.Date == daydate &&  subday.CompanyId == companyid select subday) on dish.Id equals dd.DishId into proto
                        from dayd in proto.DefaultIfEmpty()

                        select new UserDayDishViewModel() { DishId = dish.Id, DishName = dish.Name, Date = daydate, Enabled = proto.Count() > 0/*dayd != null*/ };
            return query;
        }
        public IQueryable<UserDayDishViewModelPerGategory> CategorizedDishesPerDay(DateTime daydate,string userId, int companyid)
        {
            var query = from entry in   (
                            from dd in _context.DayDish
                            join d in _context.Dishes on dd.DishId equals d.Id
                            where dd.Date==daydate &&  dd.CompanyId == companyid
                            join ud in _context.UserDayDish on new { dd.DishId, dd.Date, uid=userId ,cid= companyid } equals new {ud.DishId, ud.Date,uid=ud.UserId, cid = ud.CompanyId } into proto
                            from userday in proto.DefaultIfEmpty()
                            select new { DishId = d.Id, CategoryID = d.CategoriesId, DishName = d.Name, Date = daydate, Enabled = proto.Count() > 0,  Quantity = proto.Count()>0? proto.First().Quantity :0}
                        )
                        group entry by entry.CategoryID into catgroup
                        join cat in _context.Categories on new { id=catgroup.Key, cid = companyid } equals new { id=cat.Id, cid =cat.CompanyId }
                        orderby cat.Code
                        select new UserDayDishViewModelPerGategory()
                        {
                            CategoryCode = cat.Code,
                            CategoryName = cat.Name,
                            UserDayDishes = from dentry in catgroup
                                            select new UserDayDishViewModel()
                                            {
                                                DishId = dentry.DishId,
                                                DishName = dentry.DishName,
                                                Date = dentry.Date,
                                                Enabled = dentry.Enabled,
                                                Quantity=dentry.Quantity
                                            }
                        };
            /*
            from entry in
                    (
                        from dish in _context.Dishes
                            //join cat in _context.Categories on dish.CategoriesId equals cat.Id
                        join dd in (from subday in _context.DayDish where subday.Date == daydate select subday) on dish.Id equals dd.DishId into proto
                        from dayd in proto.DefaultIfEmpty()
                        select new { DishId = dish.Id, CategoryID = dish.CategoriesId, DishName = dish.Name, Date = daydate, Enabled = proto.Count() > 0 }
                    )
                    group entry by entry.CategoryID into catgroup
                    join cat in _context.Categories on catgroup.Key equals cat.Id
                    orderby cat.Code
                    select new UserDayDishViewModelPerGategory() {
                        CategoryCode = cat.Code,
                        CategoryName = cat.Name,
                        UserDayDishes = from dentry in catgroup select new UserDayDishViewModel()
                        {
                            DishId = dentry.DishId,
                            DishName = dentry.DishName,
                            Date = dentry.Date,
                            Enabled = dentry.Enabled
                        } };
        //                        group dish by dish.CategoriesId into catGroup
        //                        select new DayDishViewModelPerGategory() {CategoryCode=cat;

                    //select new DayDishViewModel() { DishId = dish.Id, DishName = dish.Name, Date = daydate, Enabled = proto.Count() > 0
        */
            return query;
            
        }
        public DayDish SelectSingleOrDefault(int dishId, DateTime daydate) {
            return _context.DayDish.SingleOrDefault(dd => dd.DishId == dishId && dd.Date == daydate);
       }

        public IQueryable<CustomerOrdersViewModel> CustomerOrders(DateTime daydate,  int companyid)
        {
            var query1 =
                           from dd in _context.DayDish
                           join d in _context.Dishes on dd.DishId equals d.Id
                           where dd.Date == daydate && dd.CompanyId == companyid
                           join ud in _context.UserDayDish on new { dd.DishId, dd.Date, cid = companyid } equals new { ud.DishId, ud.Date, cid = ud.CompanyId }
                           join cu in _context.Users on ud.UserId equals cu.Id
                           select new { UserId = cu.Id, UserName = cu.NormalizedUserName, DishId = d.Id, CategoryID = d.CategoriesId, DishName = d.Name, Date = daydate, ItemQuanity = ud.Quantity, ItemPrice = d.Price, ItemAmount = ud.Quantity * d.Price };
                           
            var query = from entry in (
                           from dd in _context.DayDish
                           join d in _context.Dishes on dd.DishId equals d.Id
                           where dd.Date == daydate && dd.CompanyId == companyid
                           join ud in _context.UserDayDish on new { dd.DishId, dd.Date, cid = companyid } equals new { ud.DishId, ud.Date, cid = ud.CompanyId }
                           join cu in _context.Users on ud.UserId equals cu.Id
                           select new { UserId = cu.Id, UserName = cu.NormalizedUserName, DishId = d.Id, CategoryID = d.CategoriesId, DishName = d.Name, Date = daydate, ItemQuanity = ud.Quantity, ItemPrice = d.Price, ItemAmount = ud.Quantity * d.Price }
                           )
                        group entry by entry.UserId into ordergroup
                      //  join cat in _context.Categories on new { id = ordergroup.First().CategoryID, cid = companyid } equals new { id = cat.Id, cid = cat.CompanyId }

                        select new CustomerOrdersViewModel
                        {
                            UserId= ordergroup.Key,
                          //  UserName = ordergroup.First().UserName,
                          //  Date = daydate,
                          //  DishesCount = ordergroup.Count(),
                          //  Amount= ordergroup.Sum(a=>a.ItemAmount)

                        };
            return query;
        }
    }
}
