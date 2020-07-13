using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Transactions;

namespace CateringPro.Repositories
{
    public class DishesRepository : IDishesRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        public DishesRepository(AppDbContext context,  ILogger<CompanyUser> logger)
        {
            _context = context;
            _logger = logger;
        }

       // public AppDbContext Context => _context;
        public async Task<bool> UpdateDishIngredients(Dish dish,List<string> ingredients, List<DishIngredients> proportion,int companyid)
        {
            try
            {
                
                List<int> ing= ingredients.ConvertAll(int.Parse);
                var existing = await _context.DishIngredients.WhereCompany(companyid).Where(di => di.DishId == dish.Id).ToListAsync();
                IEnumerable<DishIngredients> newRange=null;
                if (ingredients == null || ingredients.Count() == 0)
                {
                    existing.RemoveAll(di=>true);
                }
                else
                {


                    existing.RemoveAll(di => !ing.Contains(di.IngredientId));
//                    existing.AddRange(ing.Where(p2 =>
//                                    existing.All(p1 => p1.IngredientId != p2)).Select(it => new DishIngredients() { IngredientId = it, DishId = dish.Id })
//                                    );
                    newRange=ing.Where(p2 =>
                                    existing.All(p1 => p1.IngredientId != p2)).Select(it => new DishIngredients() { IngredientId = it, DishId = dish.Id,CompanyId= companyid });
                }
                AssignProportion(existing, proportion);
                _context.UpdateRange(existing);
                if (newRange != null)
                {
                    AssignProportion(newRange, proportion);
                    await _context.AddRangeAsync(newRange);
                }
                
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "UpdateDishIngredients");
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateDishEntity(Dish dish, List<DishIngredients> proportion, int companyid)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (!await dish.UpdateDBCompanyDataAsync(_context, _logger, companyid))
                    return false;


                if (!await UpdateDishIngredients(dish, proportion, companyid))
                    return false;
                scope.Complete();
            }
            return true;
        }
        public async Task<bool> UpdateDishIngredients(Dish dish,  List<DishIngredients> proportion, int companyid)
        {
            try
            {

               // List<int> ing = ingredients.ConvertAll(int.Parse);
                var existing = await _context.DishIngredients.Where(di => di.DishId == dish.Id).ToListAsync();
                _context.RemoveRange(existing);
                proportion.ForEach(p => {
                    p.CompanyId = companyid;
                    p.DishId = dish.Id;
                });
                await _context.AddRangeAsync(proportion);
                

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateDishIngredients");
                return false;
            }
            return true;
        }
        private void AssignProportion(IEnumerable<DishIngredients> target, List<DishIngredients> src)
        {
            var merged=target.Join(src, i => i.IngredientId, o => o.IngredientId, (t, s) => new { target = t, proportion = s.Proportion });
            merged.ToList().ForEach(it => it.target.Proportion = it.proportion);
        }
    }
}
