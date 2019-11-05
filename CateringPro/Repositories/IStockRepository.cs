using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
namespace CateringPro.Repositories
{
    public interface IStockRepository
    {
        IQueryable<ConsignmentStockViewModel> ConsignmentStock(int companyid, bool showZero = false);

        bool WriteOffProduction(DateTime daydate, int companyId);

    }
}
