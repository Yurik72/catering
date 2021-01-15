using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        IEnumerable<SelectListItem> DishesKind(DateTime dateFrom, DateTime dateTo, int companyid);
        IEnumerable<DishKind> UserDishesKindNoContext(DateTime date, string userid, int companyid);
        IQueryable<UserDayComplexViewModel> OrderedComplexDayNoContext(DateTime daydate, string userId, int companyid, int categoryId = -1, int dishKindId = -1);
        IEnumerable<Categories> UserCategoriesNoContext(DateTime date, string userid, int companyid);
        Pictures GetPicture(int id);
        IEnumerable<DishKind> DishesKindNoContext(DateTime date, int companyid);
        IQueryable<UserDayComplexViewModel> AvaibleComplexDayNoContext(DateTime daydate, string userId, int companyid);
        Task<bool> SaveComplexOrderDay(UserDayComplex daycomplex, string userId, int companyId);
        IQueryable<UserDayComplexViewModel> AllComplexDay(DateTime daydate, string userId, int companyid);
        IEnumerable<SelectListItem> Categories(DateTime dateFrom, DateTime dateTo, int companyid);
        UserDayComplexViewModel ComplexDetails(int complexid, int companyid);
    }
}

