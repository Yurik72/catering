using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CateringPro.Core;
namespace CateringPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DayDishesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IDayDishesRepository _dayDishesRepo;
        private readonly ILogger<CompanyUser> _logger;
        public DayDishesController(AppDbContext context, IDayDishesRepository daydishrepo, ILogger<CompanyUser> logger)
        {
            _context = context;
            _dayDishesRepo = daydishrepo;
            _logger = logger;
        }

        // GET: DayDishes
        public async Task<IActionResult> Index()
        {
            return View(await _context.DayDish.ToListAsync());
        }
        public  ViewResult EditDay(DateTime daydate)
        {
            daydate = DateTime.Now;

            return View(daydate);
           
        }
        public async Task<IActionResult> EditDayPartial(DateTime daydate)
        {
            daydate = DateTime.Now;

            return PartialView(await _dayDishesRepo.DishesPerDay(daydate,this.User.GetCompanyID()).ToListAsync());
        }
        [HttpPost]
        public async Task<JsonResult>  SaveDayDish(int DishId,DateTime daydate,bool enabled)
        {
            //daydate = DateTime.Now;
            if (ModelState.IsValid)
            {
                DayDish proto = new DayDish() { DishId = DishId, Date = daydate };
                this.AssignCompantAttr(proto);
                DayDish exsistdd = _dayDishesRepo.SelectSingleOrDefault(proto);
               // this.AssignCompantAttr(exsistdd);
                try
                {
                    if (enabled)
                    {

                        if (exsistdd != null)
                        {
                            //something wrong
                        }
                        else
                        {
                            await _context.AddAsync(proto);
                        }
                    }
                    else
                    {
                        _context.Remove(exsistdd);

                    }
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Save DayDish");
                    return Json(-1);
                }
            }
            
            //return Json(DishId);
            return Json(DishId);
        }
        // GET: DayDishes/Details/5
        public async Task<IActionResult> Details(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dayDish = await _context.DayDish
                .FirstOrDefaultAsync(m => m.Date == id);
            if (dayDish == null)
            {
                return NotFound();
            }

            return View(dayDish);
        }
        public IActionResult EditDayComponent(DateTime daydate)
        {
            object paramets = new { daydate = daydate };
            return ViewComponent("DayDishComponent", paramets);
        }
        // GET: DayDishes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DayDishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,DishId")] DayDish dayDish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dayDish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dayDish);
        }

        // GET: DayDishes/Edit/5
        public async Task<IActionResult> Edit(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dayDish = await _context.DayDish.FindAsync(id);
            if (dayDish == null)
            {
                return NotFound();
            }
            return View(dayDish);
        }

        // POST: DayDishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime id, [Bind("Date,DishId")] DayDish dayDish)
        {
            if (id != dayDish.Date)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dayDish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DayDishExists(dayDish.Date))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dayDish);
        }

        // GET: DayDishes/Delete/5
        public async Task<IActionResult> Delete(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dayDish = await _context.DayDish
                .FirstOrDefaultAsync(m => m.Date == id);
            if (dayDish == null)
            {
                return NotFound();
            }

            return View(dayDish);
        }

        // POST: DayDishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime id)
        {
            var dayDish = await _context.DayDish.FindAsync(id);
            _context.DayDish.Remove(dayDish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DayDishExists(DateTime id)
        {
            return _context.DayDish.Any(e => e.Date == id);
        }
    }
}
