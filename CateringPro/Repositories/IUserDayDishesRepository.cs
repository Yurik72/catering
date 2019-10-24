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

        IQueryable<UserDayDishViewModel> DishesPerDay(DateTime dayDate, string userId,int companyid);
        IQueryable<UserDayDishViewModelPerGategory> CategorizedDishesPerDay(DateTime daydate, string userId, int companyid);

        DayDish SelectSingleOrDefault(int dishId, DateTime daydate);

        IQueryable<CustomerOrdersViewModel> CustomerOrders(DateTime daydate, int companyid);
        CustomerOrdersViewModel CustomerOrders(string UserId, DateTime daydate, int companyid);
        bool SaveDay(List<UserDayDish> daydishes, HttpContext httpcontext);
    }
}

