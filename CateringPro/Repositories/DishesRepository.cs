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
    public class DishesRepository : IDishesRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        public DishesRepository(AppDbContext context,  ILogger<CompanyUser> logger)
        {
            _context = context;
            _logger = logger;
        }

  
        public async Task<bool> UpdateDishIngredients(Dish dish,List<string> ingredients)
        {
            try
            {
                
                List<int> ing= ingredients.ConvertAll(int.Parse);
                var existing = await _context.DishIngredients.Where(di => di.DishId == dish.Id).ToListAsync();
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
                                    existing.All(p1 => p1.IngredientId != p2)).Select(it => new DishIngredients() { IngredientId = it, DishId = dish.Id });
                }
                _context.UpdateRange(existing);
                if(newRange!=null)
                    await _context.AddRangeAsync(newRange);
                
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "UpdateDishIngredients");
                return false;
            }
            return true;
        }
    }
}
