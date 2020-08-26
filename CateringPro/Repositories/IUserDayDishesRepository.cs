using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using Microsoft.AspNetCore.Http;

namespace CateringPro.Repositories
{
    public interface IUserDayDishesRepository
    {
        bool IsAllowDayEdit(DateTime dt, int companyid);
        IQueryable<UserDayDishViewModel> DishesPerDay(DateTime dayDate, string userId,int companyid);
        IQueryable<UserDayDishViewModelPerGategory> CategorizedDishesPerDay(DateTime daydate, string userId, int companyid);

        DayDish SelectSingleOrDefault(int dishId, DateTime daydate);

        IQueryable<CustomerOrdersViewModel> CustomerOrders(DateTime daydate, int companyid);
        CustomerOrdersViewModel CustomerOrders(string UserId, DateTime daydate, int companyid);
        bool SaveDay(List<UserDayDish> daydishes, HttpContext httpcontext);
        Task<bool> SaveDayComplex(List<UserDayComplex> daycomplex, string userId, int companyId);
        Task<bool> SaveDayDishInComplex(List<UserDayDish> userDayDishes, string userId, int companyId);
        Task<bool> SaveComplexAndDishesDay(List<UserDayComplex> daycomplex, List<UserDayDish> userDayDishes, string userId, int companyId);
        Task<bool> DeleteDayComplex(UserDayComplex userDayComplex, string userId, int companyId);
        IQueryable<UserDayComplexViewModel> ComplexPerDay(DateTime daydate, string userId, int companyid);
        IQueryable<UserDayComplexViewModel> AvaibleComplexDay(DateTime daydate, string userId, int companyid);
        IQueryable<UserDayComplexViewModel> OrderedComplexDay(DateTime daydate, string userId, int companyid);
        IEnumerable<UserDayComplexViewModel> WeekOrder(DateTime dayFrom, DateTime dayTo, string userId, int companyid);
        OrderTypeEnum GetCompanyOrderType(int companyid);
        bool GetConfrimedAdmin(string userid);
        bool IsBalancePositive(string userid);
    }
}

