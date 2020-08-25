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
        private readonly IInvoiceRepository _invoicerepo;
        private readonly SharedViewLocalizer _localizer;
        //private readonly IUserDayDishesRepository _udaydishrepo;

        public UserDayDishesController(AppDbContext context, 
            IUserDayDishesRepository ud, UserManager<CompanyUser> um, 
            ILogger<CompanyUser> logger, IEmailService email, IInvoiceRepository invoicerepo,
            SharedViewLocalizer localizer)
        {
            _context = context;
            _userManager = um;
            _userdaydishesrepo = ud;
            _logger = logger;
            _email = email;
            _invoicerepo = invoicerepo;
            _localizer = localizer;
            // _udaydishrepo = udaydishrepo;
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
                //ShowComplex = user.MenuType.HasValue && (user.MenuType.Value & 1) > 0,
                //ShowDishes = user.MenuType.HasValue && (user.MenuType.Value & 2) > 0,
                ShowComplex = (_userdaydishesrepo.GetCompanyOrderType(this.User.GetCompanyID()) & (OrderTypeEnum.OneComplexType | OrderTypeEnum.Complex)) > 0,
                ShowDishes = (_userdaydishesrepo.GetCompanyOrderType(this.User.GetCompanyID()) & OrderTypeEnum.Dishes) > 0

            };
            ViewData["DishKindId"] = new SelectList(GetDishesKindWithEmptyList(), "Value", "Text"); ;
            return View(model); //await _userdishes.CategorizedDishesPerDay(DateTime.Now, _userManager.GetUserId(HttpContext.User)).ToListAsync());
        }
        private List<SelectListItem> GetDishesKindWithEmptyList()
        {
            List<SelectListItem> disheskind = _context.DishesKind.AsNoTracking()
                  .OrderBy(n => n.Name).Select(n =>
                      new SelectListItem
                      {
                          Value = n.Id.ToString(),
                          Text = n.Name
                      }).ToList();
            var empty = new SelectListItem() { Value = "", Text = _localizer["NotSpecified"] };
            disheskind.Insert(0, empty);
            return disheskind;
        }
        public async Task<IActionResult>  EditUserDay(DateTime daydate, int dishKind)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return NotFound();
            UserDayEditModel model = new UserDayEditModel()
            {
                DayDate = daydate,
                DayMenu =new DayMenu() { Date = daydate, DishKind = dishKind },
                ShowComplex = (_userdaydishesrepo.GetCompanyOrderType(this.User.GetCompanyID()) & (OrderTypeEnum.OneComplexType | OrderTypeEnum.Complex) ) >0,
                //ShowComplex = user.MenuType.HasValue && (user.MenuType.Value & 1) > 0,
                ShowDishes = (_userdaydishesrepo.GetCompanyOrderType(this.User.GetCompanyID()) & OrderTypeEnum.Dishes ) > 0
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
        public async Task<JsonResult> SaveDayComplex(List<UserDayComplex> UserDayComplex, List<UserDayDish> UserDayDish)
        {
            var daycomplexes = UserDayComplex;
            //await  _email.SendEmailAsync("yurik.kovalenko@gmail.com", "catering", "new order");
            DateTime daydate = DateTime.Now;
  //          bool res = _userdaydishesrepo.SaveDayDishInComplex(UserDayDish, this.HttpContext);
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
            
            
            if (await _userdaydishesrepo.SaveComplexAndDishesDay(daycomplexes, UserDayDish, User.GetUserId(),User.GetCompanyID()))
            {
                //await _email.SendInvoice(User.GetUserId(), daydate, User.GetCompanyID());
                return await Task.FromResult(Json(new { res = "OK" }));
               
            }
            else
            {
                return await Task.FromResult(Json(new { res = "FAIL" , reason = "Adding to db" }));
            }
 
            
        }
        public async Task<JsonResult> SendWeekInvoice(string day)
        {
            DateTime daydate = Convert.ToDateTime(day);
            await _email.SendWeekInvoice(User.GetUserId(), daydate, User.GetCompanyID());
            return await Task.FromResult(Json(new { res = "OK" }));
        }
        public async Task<IActionResult> GetWeekOrderDetails(string day)
        {
            DateTime daydate = Convert.ToDateTime(day);
            string userid = User.GetUserId();
            int comapnyid = User.GetCompanyID();
            try
            {
                //var test = _userdaydishesrepo.WeekOrder(daydate, daydate.AddDays(7), userid, comapnyid);
                //test = test.OrderBy(a => a.Date);
                var model = _invoicerepo.CustomerInvoice(userid, daydate, comapnyid);

                //var testList = test.ToList();
                //var inList = new List<InvoiceItemModel>();
                //testList.ForEach(a =>
                //{
                //    inList.Add(new InvoiceItemModel() { DayComplex = a });
                //});
                // model.Items = inList;
                
                var avaible = _userdaydishesrepo.AvaibleComplexDay(daydate, userid, comapnyid);
                var items = model.Items.ToList();
                if (avaible.Count() > 0 && items.Count() == 0)
                {
                    var inItem = new InvoiceItemModel();
                    inItem.DayComplex = new UserDayComplexViewModel();
                    inItem.DayComplex.Date = daydate;
                    items.Add(inItem);

                }
                model.Items = items;
                for (int i = 0; i < 6; i++)
                {

                    daydate = daydate.AddDays(1);

                    avaible = _userdaydishesrepo.AvaibleComplexDay(daydate, userid, comapnyid);

                    var nextModel = _invoicerepo.CustomerInvoice(userid, daydate, comapnyid);
                    var nextItems = nextModel.Items.ToList();
                    var onlyComplex = new List<InvoiceItemModel>();
                    foreach (var it in nextItems)
                    {
                        if (it.DayComplex != null)
                        {
                            onlyComplex.Add(it);
                        }
                    }
                    nextItems = onlyComplex;
                    items = model.Items.ToList();

                    if (avaible.Count() > 0 && nextModel.Items.ToList().Count() == 0)
                    {
                        var inItem = new InvoiceItemModel();
                        inItem.DayComplex = new UserDayComplexViewModel();
                        inItem.DayComplex.Date = daydate;

                        inItem.DayComplex.Enabled = false;

                        items.Add(inItem);

                        //items.AddRange(nextModel.Items.ToList());
                    }
                    items.AddRange(nextItems);

                    model.Items = items;



                }
                return PartialView("~/Views/Invoice/EmailWeekInvoice.cshtml", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetWeekOrderDetails ");
                return await Task.FromResult(Json(new { res = "FAIL", reason = "Adding to db" }));
            }

                //await _email.SendWeekInvoice(User.GetUserId(), daydate, User.GetCompanyID());
                
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCom(UserDayComplex UserDayComplex)
        {
            if (UserDayComplex == null)
            {
                return NotFound();
            }
            var comName = await _context.Complex.FirstOrDefaultAsync(x => x.CompanyId == User.GetCompanyID()
                && x.Id == UserDayComplex.ComplexId);
            var userDayDish = await _context.UserDayComplex.
                FirstOrDefaultAsync(x => x.CompanyId == User.GetCompanyID()
                && x.Date == UserDayComplex.Date && x.ComplexId == UserDayComplex.ComplexId);
               
            if (userDayDish == null)
            {
                return NotFound();
            }
            userDayDish.Complex.Name = comName.Name;
            return PartialView("Delete",userDayDish);
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

        //delete ordered complex
        public async Task<JsonResult> DeleteDayComplex(UserDayComplex UserDayComplex)
        {
            //var daycomplexes = UserDayComplex;
            //await  _email.SendEmailAsync("yurik.kovalenko@gmail.com", "catering", "new order");
            DateTime daydate = DateTime.Now;
            //          bool res = _userdaydishesrepo.SaveDayDishInComplex(UserDayDish, this.HttpContext);
            
            if (!_userdaydishesrepo.IsAllowDayEdit(UserDayComplex.Date, User.GetCompanyID()))
            {
                return await Task.FromResult(Json(new { res = "FAIL", reason = "OutDate" }));
            }
            //await _email.SendInvoice(User.GetUserId(), daydate, User.GetCompanyID());

            if (await _userdaydishesrepo.DeleteDayComplex(UserDayComplex, User.GetUserId(), User.GetCompanyID()))
            {
                return await Task.FromResult(Json(new { res = "OK" }));
            }
            else
            {
                return await Task.FromResult(Json(new { res = "FAIL", reason = "Deleting in db" }));
            }


        }


        private bool UserDayDishExists(string id)
        {
            return _context.UserDayDish.Any(e => e.UserId == id);
        }
    }
}
