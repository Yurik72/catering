using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
namespace CateringPro.Repositories
{
    public interface IDayDishesRepository
    {

        IQueryable<DayDishViewModel> DishesPerDay(DateTime dayDate, int companyid);
        IQueryable<DayDishViewModelPerGategory> CategorizedDishesPerDay(DateTime dayDate, int companyid);

        DayDish SelectSingleOrDefault(int dishId, DateTime daydate);
        DayDish SelectSingleOrDefault(DayDish src);

        DayComplex SelectComplexSingleOrDefault(int complexId, DateTime daydate);

        DayComplex SelectComplexSingleOrDefault(DayComplex src);
        IQueryable<DayComplexViewModel> ComplexDay(DateTime daydate, int companyid);
        OrderTypeEnum GetCompanyOrderType(int companyid);
    }
}
