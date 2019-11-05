using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using Microsoft.Extensions.Logging;

namespace CateringPro.Repositories
{
    public class ComplexRepository : IComplexRepository
    {
        private readonly AppDbContext _context;
        ILogger<CompanyUser> _logger;

        public ComplexRepository(AppDbContext context, ILogger<CompanyUser> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> UpdateComplexDishes(Complex complex, List<string> dishes, int companyid)
        {
            try
            {

                List<int> ds = dishes.ConvertAll(int.Parse);
                var existing = await _context.DishComplex.WhereCompany(companyid).Where(di => di.DishId == complex.Id).ToListAsync();
                IEnumerable<DishComplex> newRange = null;
                if (dishes == null || dishes.Count() == 0)
                {
                    existing.RemoveAll(di => true);
                }
                else
                {


                    existing.RemoveAll(di => !ds.Contains(di.DishId));
                    //                    existing.AddRange(ing.Where(p2 =>
                    //                                    existing.All(p1 => p1.IngredientId != p2)).Select(it => new DishIngredients() { IngredientId = it, DishId = dish.Id })
                    //                                    );
                    newRange = ds.Where(p2 =>
                                      existing.All(p1 => p1.DishId != p2)).Select(it => new DishComplex() { DishId = it, ComplexId = complex.Id, CompanyId = companyid });
                }
                _context.UpdateRange(existing);
                if (newRange != null)
                    await _context.AddRangeAsync(newRange);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateComplexDishes");
                return false;
            }
            return true;
        }
    }
}
