using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using CateringPro.ViewModels;

namespace CateringPro.Repositories
{
    public interface IReportRepository
    {
        CompanyModel GetOwnCompany(int companyid);

        CompanyModel GetUserCompany(string UserId);
        InvoiceModel CustomerInvoice(string UserId, DateTime daydate, int companyid);
       

        DayProductionViewModel CompanyDayProduction(DateTime daydate, int companyid);

        DayIngredientsViewModel CompanyDayIngredients(DateTime daydate, int companyid);

        Task<ProductionForecastViewModel> CompanyProductionForecast(DateTime datefrom, DateTime dateto, int companyId);

        DayProductioDayViewModel CompanyDayProduction(DateTime datefrom, DateTime dateto, int companyid);


        IEnumerable<CompanyMenuComplexModel> CompanyComplexMenu(DateTime datefrom, DateTime dateto, int companyid);
        CompanyMenuModel CompanyMenu(DateTime datefrom, DateTime dateto, int companyid);

        DishSpecificationViewModel DishSpecification(DateTime datefrom, DateTime dateto, int companyid);

        InvoiceModel EmailWeekInvoice(DateTime daydate, int companyid, CompanyUser user);
    }
}
