﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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

        public async Task<Complex> GetByIdAsync(int? id)
        {
            return await _context.Complex.FirstOrDefaultAsync(p => p.Id == id);
        }
        public void Remove(Complex complex)
        {
            _context.Remove(complex);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateComplexDishes(Complex complex, List<string> dishes, int companyid, List<ItemsLine> dishLine)
        {
            try
            {

                List<int> ds = dishes.ConvertAll(int.Parse);
                var dishComplexes = from dl in dishLine
                                    from dlc in dl.DishesIds
                                    select new DishComplex()
                                    {
                                        CompanyId = companyid,
                                        ComplexId = complex.Id,
                                        DishId = dlc,
                                        DishCourse = dl.DishCourse
                                    };
                var existing_db = await _context.DishComplex.WhereCompany(companyid).Where(di => di.ComplexId == complex.Id ).ToListAsync();
               // IEnumerable<DishComplex> newRange = null;
               // empty remove all
                    //remove items which are not in db
                    // existing_db.RemoveAll(di => !dishComplexes.Any(dc=> dc.DishId== di.DishId && dc.ComplexId==di.ComplexId && dc.DishCourse==di.DishCourse));
                    //existing_db.RemoveAll(di=>true);
                    // _context.UpdateRange(existing_db);
                    _context.RemoveRange(existing_db);
                     await _context.AddRangeAsync(dishComplexes);
                    await _context.SaveChangesAsync();
                
                /*
                // IEnumerable<DishComplex> newRange1 = null;
                foreach (var dl in dishLine)
                {
                    var existing = await _context.DishComplex.WhereCompany(companyid).Where(di => di.ComplexId == complex.Id && di.DishCourse== dl.DishCourse).ToListAsync();
                    IEnumerable<DishComplex> newRange = null;
                    if (dishLine == null || dishLine.Count() == 0)
                    {
                        existing.RemoveAll(di => true);
                    }
                    else
                    {


                        existing.RemoveAll(di => !dl.DishesIds.Contains(di.DishId));
                        //                    existing.AddRange(ing.Where(p2 =>
                        //                                    existing.All(p1 => p1.IngredientId != p2)).Select(it => new DishIngredients() { IngredientId = it, DishId = dish.Id })
                        //                                    );
                        newRange = ds.Where(p2 =>
                                          existing.All(p1 => p1.DishId != p2)).Select(it => new DishComplex() { DishId = it, ComplexId = complex.Id, CompanyId = companyid, DishCourse = dl.DishCourse });
                    }
                    _context.UpdateRange(existing);
                    if (newRange != null)
                        await _context.AddRangeAsync(newRange);

                    await _context.SaveChangesAsync();

                }

                */
                //if (dishes == null || dishes.Count() == 0)
                //{
                //    existing.RemoveAll(di => true);
                //}
                //else
                //{


                //    existing.RemoveAll(di => !ds.Contains(di.DishId));
                //    //                    existing.AddRange(ing.Where(p2 =>
                //    //                                    existing.All(p1 => p1.IngredientId != p2)).Select(it => new DishIngredients() { IngredientId = it, DishId = dish.Id })
                //    //                                    );
                //    newRange = ds.Where(p2 =>
                //                      existing.All(p1 => p1.DishId != p2)).Select(it => new DishComplex() { DishId = it, ComplexId = complex.Id, CompanyId = companyid });
                //}
                //_context.UpdateRange(existing);
                //if (newRange != null)
                //    await _context.AddRangeAsync(newRange);

                //await _context.SaveChangesAsync();
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