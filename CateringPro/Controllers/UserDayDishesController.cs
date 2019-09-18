﻿using System;
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

namespace CateringPro.Controllers
{
    [Authorize]
    public class UserDayDishesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUserDayDishesRepository _userdishes;
        private readonly UserManager<IdentityUser> _userManager;

        public UserDayDishesController(AppDbContext context, IUserDayDishesRepository ud, UserManager<IdentityUser> um)
        {
            _context = context;
            _userManager = um;
            _userdishes = ud;
        }

        // GET: UserDayDishes
        [Route("")]
        [Route("UserDayDishes")]
        [Route("MyOrders")]
        public async Task<IActionResult> Index()
        {
            //var appDbContext = _context.UserDayDish.Include(u => u.Dish).Include(u => u.User);
            return View(DateTime.Now); //await _userdishes.CategorizedDishesPerDay(DateTime.Now, _userManager.GetUserId(HttpContext.User)).ToListAsync());
        }
        public IActionResult EditUserDayComponent(DateTime daydate)
        {
            object paramets = new { daydate = daydate };
            return ViewComponent("UserDayDishComponent", paramets);
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
            //to do make a separate context for async
            Func<UserDayDish, Task<bool>> saveday = async d =>  {
              
                    var userDayDish = await _context.UserDayDish.FindAsync(_userManager.GetUserId(HttpContext.User), d.Date, d.DishId);
                    if (userDayDish != null)
                    {
                        userDayDish.Quantity = d.Quantity;
                        _context.Update(userDayDish);
                    }
                    else
                    {
                        d.UserId = _userManager.GetUserId(HttpContext.User);
                        _context.Add(d);
                    }
                
                return true;
                
            };
            daydishes.ForEach( d => {
                //await saveday(d);
                
                 var userDayDish =  _context.UserDayDish.Find(_userManager.GetUserId(HttpContext.User), d.Date, d.DishId);
                if (userDayDish != null)
                {
                    userDayDish.Quantity = d.Quantity;
                    _context.Update(userDayDish);
                }
                else
                {
                    d.UserId = _userManager.GetUserId(HttpContext.User);
                    _context.Add(d);
                }
                
            });
            await _context.SaveChangesAsync();
            return await Task.FromResult(Json(new { res = "OK" }));
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
