using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CateringPro.Repositories;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Core;
using Microsoft.Extensions.Logging;
using CateringPro.ViewModels;

namespace CateringPro.Controllers
{
    [Authorize]
    public class UserDayDishesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUserDayDishesRepository _userdaydishesrepo;
        private readonly UserManager<CompanyUser> _userManager;
        private readonly ILogger<CompanyUser> _logger;
        private readonly IEmailService _email;

        public UserDayDishesController(AppDbContext context, IUserDayDishesRepository ud, UserManager<CompanyUser> um, ILogger<CompanyUser> logger, IEmailService email)
        {
            _context = context;
            _userManager = um;
            _userdaydishesrepo = ud;
            _logger = logger;
            _email = email;
        }

        // GET: UserDayDishes
   
        [Route("UserDayDishes")]
        [Route("MyOrders")]
        public async Task<IActionResult> Index()
        {
            var user=await  _userManager.GetUserAsync(HttpContext.User);
            if(user==null)
                return NotFound();
            UserDayEditModel model = new UserDayEditModel()
            {
                DayDate = DateTime.Now,
                ShowComplex= user.MenuType.HasValue && (user.MenuType.Value & 1)>0,
                ShowDishes = user.MenuType.HasValue && (user.MenuType.Value & 2) > 0,
               

            };
            return View(model); //await _userdishes.CategorizedDishesPerDay(DateTime.Now, _userManager.GetUserId(HttpContext.User)).ToListAsync());
        }
        public async Task<IActionResult>  EditUserDay(DateTime daydate)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return NotFound();
            UserDayEditModel model = new UserDayEditModel()
            {
                DayDate = daydate,
                ShowComplex = user.MenuType.HasValue && (user.MenuType.Value & 1) > 0,
                ShowDishes = user.MenuType.HasValue && (user.MenuType.Value & 2) > 0
            };
            return PartialView(model);
        }
        // GET: UserDayDishes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDayDish = await _context.UserDayDish
                .Include(u => u.Dish)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userDayDish == null)
            {
                return NotFound();
            }

            return View(userDayDish);
        }

        // GET: UserDayDishes/Create
        public IActionResult Create()
        {
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Code");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserDayDishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,DishId,Quantity,UserId")] UserDayDish userDayDish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userDayDish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Code", userDayDish.DishId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userDayDish.UserId);
            return View(userDayDish);
        }

        // GET: UserDayDishes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDayDish = await _context.UserDayDish.FindAsync(id);
            if (userDayDish == null)
            {
                return NotFound();
            }
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Code", userDayDish.DishId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userDayDish.UserId);
            return View(userDayDish);
        }
        [HttpPost]
        public async Task<JsonResult> SaveDay(List<UserDayDish> daydishes)
        {
            //await  _email.SendEmailAsync("yurik.kovalenko@gmail.com", "catering", "new order");
            DateTime daydate = DateTime.Now;
            if (daydishes.Count > 0)
                daydate = daydishes.First().Date;
            else
            {
                return await Task.FromResult(Json(new { res = "FAIL",reason="Empty" }));
            }
            if(!_userdaydishesrepo.IsAllowDayEdit(daydate, User.GetCompanyID()))
            {
                return await Task.FromResult(Json(new { res = "FAIL", reason = "OutDate" }));
            }
            await _email.SendInvoice(User.GetUserId(), daydate, User.GetCompanyID());
            if (  _userdaydishesrepo.SaveDay(daydishes, this.HttpContext)){
                return await Task.FromResult(Json(new { res = "OK" }));
            }
            else
            {
                return await Task.FromResult(Json(new { res = "FAIL",reason="Error" }));
            }
            /*
            try
            {
                daydishes.ForEach(d =>
                {
                    //await saveday(d);
                    this.AssignUserAttr(d);
                    var userDayDish = _context.UserDayDish.Find(d.UserId, d.Date, d.DishId,d.CompanyId  );
                    if (userDayDish != null)
                    {
                        userDayDish.Quantity = d.Quantity;
                        userDayDish.Price = d.Price;
                        _context.Update(userDayDish);
                    }
                    else if(d.Quantity>0)
                    {
                        //d.UserId = this.User.GetUserId();
                       
                        _context.Add(d);
                    }

                });
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Update user day dish");
                return await Task.FromResult(Json(new { res = "FAIL" }));
            }
            return await Task.FromResult(Json(new { res = "OK" }));
            */
        }
        public async Task<JsonResult> SaveDayComplex(List<UserDayComplex> daycomplexes)
        {
            //await  _email.SendEmailAsync("yurik.kovalenko@gmail.com", "catering", "new order");
            DateTime daydate = DateTime.Now;
            if (daycomplexes.Count > 0)
                daydate = daycomplexes.First().Date;
            else
            {
                return await Task.FromResult(Json(new { res = "FAIL", reason = "Empty" }));
            }
            if (!_userdaydishesrepo.IsAllowDayEdit(daydate, User.GetCompanyID()))
            {
                return await Task.FromResult(Json(new { res = "FAIL", reason = "OutDate" }));
            }
            await _email.SendInvoice(User.GetUserId(), daydate, User.GetCompanyID());
            if (_userdaydishesrepo.SaveDayComplex(daycomplexes, this.HttpContext))
            {
                return await Task.FromResult(Json(new { res = "OK" }));
            }
            else
            {
                return await Task.FromResult(Json(new { res = "FAIL" }));
            }
 
            
        }
        // POST: UserDayDishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Date,DishId,Quantity,UserId")] UserDayDish userDayDish)
        {
            if (id != userDayDish.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userDayDish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDayDishExists(userDayDish.UserId))
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
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Code", userDayDish.DishId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userDayDish.UserId);
            return View(userDayDish);
        }

        // GET: UserDayDishes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDayDish = await _context.UserDayDish
                .Include(u => u.Dish)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userDayDish == null)
            {
                return NotFound();
            }

            return View(userDayDish);
        }

        // POST: UserDayDishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userDayDish = await _context.UserDayDish.FindAsync(id);
            _context.UserDayDish.Remove(userDayDish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserDayDishExists(string id)
        {
            return _context.UserDayDish.Any(e => e.UserId == id);
        }
    }
}
