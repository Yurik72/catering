using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CateringPro.Core;
using CateringPro.ViewModels;

namespace CateringPro.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        SharedViewLocalizer _localizer;
        public StockRepository(AppDbContext context, ILogger<CompanyUser> logger, SharedViewLocalizer localizer)
        {
            _context = context;
            _logger = logger;
            _localizer = localizer;
        }

        public IQueryable<ConsignmentStockViewModel> ConsignmentStock(int companyid, bool showZero = false)
        {
            return null;
            var query = from ing in _context.Ingredients.Where(cs => cs.CompanyId == companyid)
                        join c in _context.Consignment.Where(cs => cs.CompanyId == companyid) on ing.Id equals c.IngredientsId
                        join dl in _context.DocLines.Where(cs => cs.CompanyId == companyid) on c.LineId equals dl.Id
                        join d in _context.Docs.Where(cs => cs.CompanyId == companyid) on dl.DocsId equals d.Id
                        select new ConsignmentStockDetailViewModel()
                        {
                            DocNumber = d.Number,
                            DocDate = d.Date,
                            DocTypeName = (d.Type == 1 ? _localizer["DocTypeIncome"] : "..."),
                            LineId = c.LineId,
                            IngredientId = c.IngredientsId,
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
        public async Task<IEnumerable<ConsignmentStockViewModel>> ConsignmentStock(QueryModel querymodel, int companyid, bool showZero = false)
        {
            /*
            var query = from ing in _context.Ingredients.Where(cs => cs.CompanyId == companyid)
                        join c in _context.Consignment.Where(cs => cs.CompanyId == companyid) on ing.Id equals c.IngredientsId
                        join dl in _context.DocLines.Where(cs => cs.CompanyId == companyid) on c.LineId equals dl.Id
                        join d in _context.Docs.Where(cs => cs.CompanyId == companyid) on dl.DocsId equals d.Id
                        select new ConsignmentStockDetailViewModel()
                        {
                            DocNumber = d.Number,
                            DocDate = d.Date,
                            DocTypeName = (d.Type == 1 ? _localizer["DocTypeIncome"] : "..."),
                            LineId = c.LineId,
                            IngredientId = c.IngredientsId,
                            StockValue = c.Quantity,
                            InitialValue = c.InitialQuantity,
                            ValidUntil = c.ValidUntil,
                            Price =c.Price
                        };
            */
            var query1 = from ing in _context.Ingredients.Include(x => x.IngredientCategory).Where(cs => cs.CompanyId == companyid)
                         select ing;

            if (!string.IsNullOrEmpty(querymodel.SearchCriteria))
            {
                query1 = query1.Where(d => d.Name.Contains(querymodel.SearchCriteria));


            }
            //if (!string.IsNullOrEmpty(querymodel.SortField))
            // {
            //     query1 = query1.OrderByEx(querymodel.SortField, querymodel.SortOrder);
            //  }
            if (querymodel.Page > 0)
            {
                query1 = query1.Skip(querymodel.PageRecords * querymodel.Page);
            }
            var queryfinal = from ing in query1
                             select new ConsignmentStockViewModel()
                             {
                                 IngredientCategoryId = ing.IngredientCategory.Id,
                                 IngredientCategoryName = ing.IngredientCategory.Name,
                                 IngredientId = ing.Id,
                                 IngredientName = ing.Name,
                                 StockValue = ing.StockValue,
                                 MeasureUnit = ing.MeasureUnit/*,
                                Consignments = from entry in query.Where(x => x.IngredientId == ing.Id)
                                               select entry*/

                             };
            if (!string.IsNullOrEmpty(querymodel.SortField))
            {
                queryfinal = queryfinal.OrderByEx(querymodel.SortField, querymodel.SortOrder);
            }
            return await queryfinal.ToListAsync();
        }
        public bool WriteOffProduction(DateTime daydate, int companyId)
        {
            try
            {
                _logger.LogInformation("WriteOffProduction {0},{1}", daydate, companyId);
                var res = _context.Database.ExecuteSqlInterpolated($"exec WriteOffProduction {daydate} , {companyId}");
            }
            catch (Exception ex)
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
                var res = await _context.Database.ExecuteSqlInterpolatedAsync($"exec WriteOffProduction {daydate} , {companyId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WriteOffProduction");
                return false;
            }
            return true;
        }

        public async Task<IngredientStockDetailsViewModel> IngredientStockDetail(int id, int companyid)
        {
            var model = new IngredientStockDetailsViewModel();
            model.ConsignmentsDetail = await (from cons in _context.Consignment.Where(c => c.IngredientsId == id)
                                              join dl in _context.DocLines on cons.LineId equals dl.Id
                                              join d in _context.Docs on dl.DocsId equals d.Id
                                              select new ConsignmentDetailsViewModel()

                                              {
                                                  InitialQuantity= cons.InitialQuantity,
                                                  Quantity=cons.Quantity,
                                                  Price=cons.Price,
                                                  DocNumber = d.Number,
                                                  DocDate=d.Date,
                                                  DocId=d.Id

                                              }).ToListAsync();
            model.Ingredients = await _context.Ingredients.Include(i => i.IngredientCategory).FirstOrDefaultAsync(i => i.Id == id);
            return model;
        }
    }
}
