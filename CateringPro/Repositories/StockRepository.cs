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
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        public StockRepository(AppDbContext context,  ILogger<CompanyUser> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<ConsignmentStockViewModel> ConsignmentStock( int companyid,bool showZero=false)
        {
            var query = from ing in _context.Ingredients.Where(cs => cs.CompanyId == companyid)
                        join c in _context.Consignment.Where(cs => cs.CompanyId == companyid) on ing.Id equals c.IngredientsId
                        join dl in _context.DocLines.Where(cs => cs.CompanyId == companyid) on c.LineId equals dl.Id
                        join d in _context.Docs.Where(cs => cs.CompanyId == companyid) on dl.DocsId equals d.Id
                        select new ConsignmentStockDetailViewModel()
                        {
                            DocNumber=d.Number,
                            DocDate=d.Date,
                            LineId  = c.LineId,
                            IngredientId=c.IngredientsId,
                            StockValue = c.Quantity,
                            ValidUntil = c.ValidUntil
                        };
            var query1 = from ing in _context.Ingredients.Where(cs => cs.CompanyId == companyid)
                         select new ConsignmentStockViewModel()
                         {
                             IngredientId = ing.Id,
                             IngredientName = ing.Name,
                             MeasureUnit = ing.MeasureUnit,
                             Consignments = from entry in query.Where(x => x.IngredientId == ing.Id)
                                            select entry

                         };
            return query1;
        }
        public bool WriteOffProduction(DateTime daydate, int companyId)
        {
            try
            {
                _logger.LogInformation("WriteOffProduction {0},{1}", daydate, companyId);
                var res= _context.Database.ExecuteSqlInterpolated($"exec WriteOffProduction {daydate} , {companyId}");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "WriteOffProduction");
                return false;
            }
            return true;
        }
        public async Task<bool> WriteOffProductionAsync(DateTime daydate, int companyId)
        {
            try
            {
                _logger.LogInformation("WriteOffProduction {0},{1}", daydate, companyId);
                var res =await _context.Database.ExecuteSqlInterpolatedAsync($"exec WriteOffProduction {daydate} , {companyId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WriteOffProduction");
                return false;
            }
            return true;
        }
    }
}
