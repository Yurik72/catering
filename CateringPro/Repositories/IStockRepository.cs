using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using CateringPro.ViewModels;

namespace CateringPro.Repositories
{
    public interface IStockRepository
    {
        IQueryable<ConsignmentStockViewModel> ConsignmentStock(int companyid, bool showZero = false);
        Task<IEnumerable<ConsignmentStockViewModel>> ConsignmentStock(QueryModel querymodel, int companyid, bool showZero = false);
        Task<IngredientStockDetailsViewModel> IngredientStockDetail(int id, int companyid);
        bool WriteOffProduction(DateTime daydate, int companyId);
        Task<bool> WriteOffProductionAsync(DateTime daydate, int companyId);

    }
}
