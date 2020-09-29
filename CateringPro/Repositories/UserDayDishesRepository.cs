using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using CateringPro.Core;
using Microsoft.Extensions.Caching.Memory;
using System.Transactions;
using System.Xml.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Drawing2D;

namespace CateringPro.Repositories
{
    public class UserDayDishesRepository : IUserDayDishesRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        private readonly UserManager<CompanyUser> _userManager;
        private readonly IMemoryCache _cache;
        private readonly IPluginsRepository _plugins;
        public UserDayDishesRepository(AppDbContext context, ILogger<CompanyUser> logger,
            UserManager<CompanyUser> userManager, IMemoryCache cache, IPluginsRepository plugins)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _cache = cache;
            _plugins = plugins;
        }
        public OrderTypeEnum GetCompanyOrderType(int companyid)
        {
            return _cache.GetCachedCompanyAsync(_context, companyid).Result.GetOrderType();
        }
        public bool GetConfrimedAdmin(string userid)
        {
            return _context.Users.Where(us => us.Id == userid).Select(us => us.ConfirmedByAdmin).FirstOrDefault();
        }
        public bool IsBalancePositive(string userid)
        {
            return _context.UserFinances.Where(us => us.Id == userid).Select(us => us.TotalPreOrderBalance).FirstOrDefault()>0;
        }
        public bool IsAllowDayEdit(DateTime dt, int companyid)
        {
            var company = _cache.GetCachedCompanyAsync(_context, companyid).Result; //_context.Companies.Find(companyid);
            if (company == null)
                return false;
            var dateNow = DateTime.Now;
            DateTime min = dateNow.AddHours(-(company.OrderLeadTimeH.Value));
            dateNow = dateNow.AddDays(1);
            DateTime max = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 0, 0, 0);
            dateNow = dateNow.AddDays(1);
            
            //DateTime min = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 0, 0, 0);
            
           // dt = new DateTime(dt.Year, dt.Month, dt.Day, dateNow.Hour, dateNow.Minute, dateNow.Second);
           max = max.AddHours(company.OrderThresholdTimeH.HasValue ? company.OrderThresholdTimeH.Value : 24);
            //min = min.AddHours(-(company.OrderLeadTimeH.HasValue ? company.OrderLeadTimeH.Value : 24));
          
           // min = min.AddHours(-(company.OrderLeadTimeH.Value));
            //DateTime min = DateTime.Now.AddHours(-(company.OrderLeadTimeH.HasValue ? company.OrderLeadTimeH.Value : 24));
            if ((dt - min).TotalDays < 7)
                for (DateTime t = dt; t > min; t = t.AddDays(-1))
                {
                    if (t.DayOfWeek == DayOfWeek.Saturday || t.DayOfWeek == DayOfWeek.Sunday)
                    {
                        min = min.AddDays(-1);
                    }
                }
            //if (dt.Day == min.Day)
            //{
            //    return dt.TimeOfDay < min.TimeOfDay && dt < max;
            //}
            return dt > min && dt < max;
        }
        public CompanyModel GetOwnCompany(int companyid)
        {
            CompanyModel res;
            try
            {
                var company = _context.Companies.Find(companyid);
                if (company == null)
                    throw new Exception("Company not exists");
                res = new CompanyModel()
                {
                    Name = company.Name,
                    Phone = company.Phone,
                    ZipCode = company.ZipCode,
                    Email = company.Email,

                    City = company.City,
                    Address1 = company.Address1,
                    Address2 = company.Address2,
                    Country = company.Country,
                    PictureId = company.PictureId,
                };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetOwnCompany Company={0} ", companyid);
                return new CompanyModel(); //to do
            }
            return res;
        }
        public CompanyModel GetUserCompany(string UserId)
        {
            CompanyModel res;
            try
            {
                var user = _context.Users.Find(UserId);
                if (user == null)
                    throw new Exception("User not exists");
                res = new CompanyModel()
                {
                    Phone = user.PhoneNumber,
                    ZipCode = user.ZipCode,
                    Email = user.Email,
                    Name = user.UserName,
                    City = user.City,
                    Address1 = user.Address1,
                    Address2 = user.Address2,
                    Country = user.Country
                };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserCompany User={0} ", UserId);
                return new CompanyModel(); //to do
            }
            return res;
        }
        public IQueryable<UserDayDishViewModel> DishesPerDay(DateTime daydate, string userId, int companyid)
        {
            var query = from dish in _context.Dishes
                        join dd in (from subday in _context.DayDish where subday.Date == daydate && subday.CompanyId == companyid select subday) on dish.Id equals dd.DishId into proto
                        from dayd in proto.DefaultIfEmpty()

                        select new UserDayDishViewModel() { DishId = dish.Id, DishName = dish.Name, Date = daydate, Enabled = proto.Count() > 0/*dayd != null*/ };
            return query;
        }
        public IQueryable<UserDayDishViewModelPerGategory> CategorizedDishesPerDay(DateTime daydate, string userId, int companyid)
        {
            var query1 = from dish in _context.Dishes.Where(d => d.CompanyId == companyid)
                         join dd in _context.DayDish.Where(d => d.CompanyId == companyid && d.Date == daydate) on dish.Id equals dd.DishId
                         join ud in _context.UserDayDish.Where(ud => ud.CompanyId == companyid && ud.UserId == userId && ud.Date == daydate)
                             on dish.Id equals ud.DishId into Details
                         from udayd in Details.DefaultIfEmpty()
                         select new UserDayDishViewModel
                         {
                             DishId = dish.Id,
                             CategoryId = dish.CategoriesId,
                             DishName = dish.Name,
                             //Price=dish.Price,
                             DishDescription = dish.Description,
                             DishIngredientds = string.Join(",", from di in _context.DishIngredients.Where(t => t.DishId == dish.Id)
                                                                 join ingr in _context.Ingredients on di.IngredientId equals ingr.Id
                                                                 select ingr.Name),
                             PictureId = dish.PictureId,
                             Date = daydate,
                             Quantity = udayd.Date == daydate ? udayd.Quantity : 0,
                             Price = udayd.Date == daydate ? udayd.Price : dish.Price
                         };
            var query2 = from cat in _context.Categories
                         select new UserDayDishViewModelPerGategory()
                         {
                             CategoryCode = cat.Code,
                             CategoryName = cat.Name,
                             UserDayDishes = from dd in query1.Where(q => q.CategoryId == cat.Id)
                                             select new UserDayDishViewModel()
                                             {
                                                 Date = dd.Date,
                                                 DishId = dd.DishId,
                                                 DishName = dd.DishName,
                                                 Price = dd.Price,
                                                 ReadyWeight = dd.ReadyWeight,
                                                 KKal = dd.KKal,
                                                 Quantity = dd.Quantity,
                                                 PictureId = dd.PictureId,
                                                 Enabled = dd.Enabled,
                                                 DishDescription = dd.DishDescription,
                                                 DishIngredientds = dd.DishIngredientds
                                             }
                         };
            /* !! not more working on EF 3.0*/
            /*
            var query = from entry in   (
                            from dd in _context.DayDish
                            join d in _context.Dishes on dd.DishId equals d.Id
                            where dd.Date==daydate &&  dd.CompanyId == companyid
                            join ud in _context.UserDayDish on new { dd.DishId, dd.Date, uid=userId ,cid= companyid } equals new {ud.DishId, ud.Date,uid=ud.UserId, cid = ud.CompanyId } into proto
                            from userday in proto.DefaultIfEmpty()
                            select new { DishId = d.Id, CategoryID = d.CategoriesId, DishName = d.Name, Date = daydate, Enabled = proto.Count() > 0,  Quantity = proto.Count()>0? proto.First().Quantity :0}
                        )
                        group entry by entry.CategoryID into catgroup
                        join cat in _context.Categories on new { id=catgroup.Key, cid = companyid } equals new { id=cat.Id, cid =cat.CompanyId }
                        orderby cat.Code
                        select new UserDayDishViewModelPerGategory()
                        {
                            CategoryCode = cat.Code,
                            CategoryName = cat.Name,
                            UserDayDishes = from dentry in catgroup
                                            select new UserDayDishViewModel()
                                            {
                                                DishId = dentry.DishId,
                                                DishName = dentry.DishName,
                                                Date = dentry.Date,
                                                Enabled = dentry.Enabled,
                                                Quantity=dentry.Quantity
                                            }
                        };
           */
            return query2;

        }
        public DayDish SelectSingleOrDefault(int dishId, DateTime daydate)
        {
            return _context.DayDish.SingleOrDefault(dd => dd.DishId == dishId && dd.Date == daydate);
        }

        public IQueryable<CustomerOrdersViewModel> CustomerOrders(DateTime daydate, int companyid)
        {
            var query1 = from ud in _context.UserDay.Where(dd => dd.CompanyId == companyid && dd.Date == daydate)
                         join user in _context.Users on ud.UserId equals user.Id
                         select new CustomerOrdersViewModel
                         {
                             UserId = ud.UserId,
                             UserName = "", //! todo
                             Date = daydate,
                             DishesCount = ud.Quantity,
                             Amount = ud.Total,
                             IsConfirmed = ud.IsConfirmed,
                             IsPaid = ud.IsPaid,
                             User = new CompanyModel()
                             {
                                 Phone = user.PhoneNumber,
                                 ZipCode = user.ZipCode,
                                 Email = user.Email,
                                 Name = user.UserName,
                                 City = user.City,
                                 Address1 = user.Address1,
                                 Address2 = user.Address2,
                                 Country = user.Country
                             }

                         };


            return query1;


        }
        public CustomerOrdersViewModel CustomerOrders(string UserId, DateTime daydate, int companyid)
        {
            var query1 =
                           from dd in _context.DayDish.Where(dd => dd.CompanyId == companyid && dd.Date == daydate)
                           join d in _context.Dishes.Where(dd => dd.CompanyId == companyid) on dd.DishId equals d.Id
                           join ud in _context.UserDayDish.Where(ud => ud.CompanyId == companyid && ud.Date == daydate) on dd.DishId equals ud.DishId
                           join cu in _context.Users on ud.UserId equals cu.Id
                           select new CustomerOrdersDetailsViewModel
                           {
                               UserId = cu.Id,
                               UserName = cu.NormalizedUserName,
                               DishId = d.Id,
                               CategoryId = d.CategoriesId,
                               DishName = d.Name,
                               Date = daydate,
                               Quantity = ud.Quantity,
                               Price = d.Price,
                               Amount = ud.Quantity * d.Price
                           };
            var querysingle = query1.FirstOrDefault();
            var res = new CustomerOrdersViewModel()
            {

                Details = query1
            };
            if (querysingle != null)
            {
                res.UserId = querysingle.UserId;
                res.UserName = querysingle.UserName;
                res.Date = querysingle.Date;
            }


            return res;

        }

        public bool SaveDay(List<UserDayDish> daydishes, HttpContext httpcontext)
        {

            try
            {
                daydishes.ForEach(d =>
                {
                    //await saveday(d);
                    d.IsComplex = false;
                    httpcontext.User.AssignUserAttr(d);

                    var userDayDish = _context.UserDayDish.SingleOrDefault(c => c.CompanyId == d.CompanyId
                                && c.Date == d.Date
                                && c.UserId == d.UserId
                                && c.ComplexId == d.ComplexId);

                    if (userDayDish != null)
                    {
                        userDayDish.Quantity = d.Quantity;
                        userDayDish.Price = d.Price;
                        userDayDish.IsComplex = false;
                        _context.Update(userDayDish);
                    }
                    else if (d.Quantity > 0)
                    {
                        //d.UserId = this.User.GetUserId();

                        _context.Add(d);
                    }


                });
                //if (!UpdateUserDay(daydishes, httpcontext))
                //    return false;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update user day dish");
                return false;
            }
            return true;
        }

        private bool UpdateUserDay(List<UserDayDish> daydishes, HttpContext httpcontext)
        {
            var userid = httpcontext.User.GetUserId();
            var companyid = httpcontext.User.GetCompanyID();
            bool isnew = false;
            UserDay userDay = null;

            if (daydishes.Count > 0)
            {
                DateTime daydate = daydishes.First().Date;
                userDay = _context.UserDay.FirstOrDefault(ud => ud.UserId == userid
                && ud.CompanyId == companyid && ud.Date == daydate);
                if (userDay == null)
                {
                    isnew = true;
                    userDay = new UserDay() { Date = daydate };
                    httpcontext.User.AssignUserAttr(userDay);
                }
                userDay.Total = daydishes.Sum(d => d.Price * d.Quantity);

                userDay.Quantity = daydishes.Sum(d => d.Quantity);

            }
            try
            {
                if (isnew && userDay != null)
                {
                    _context.Add(userDay);
                }
                if (!isnew && userDay != null)
                {
                    _context.Update(userDay);
                }
                if (userDay != null)
                    _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update user day ");
                return false;
            }
            return true;


        }

        public async Task<bool> SaveDayComplex(List<UserDayComplex> daycomplex, string userId, int companyId)
        {
            daycomplex.ForEach(d => { d.CompanyId = companyId; d.UserId = userId; });
            try
            {
                daycomplex.ForEach(d =>
                {
                    //await saveday(d);
                    // httpcontext.User.AssignUserAttr(d);
                    if (d.Price == null)
                    {
                        return;
                    }
                    var userDayComplex = _context.UserDayComplex.SingleOrDefault(c => c.CompanyId == d.CompanyId
                                && c.Date == d.Date
                                && c.UserId == d.UserId
                                && c.ComplexId == d.ComplexId);
                    if (userDayComplex != null)
                    {
                        userDayComplex.Quantity = d.Quantity;
                        userDayComplex.Price = d.Price;
                        _context.Update(userDayComplex);
                    }
                    else if (d.Quantity > 0)
                    {
                        //d.UserId = this.User.GetUserId();

                        _context.Add(d);
                    }

                });
                _context.SaveChanges();
                //  if (!UpdateUserComplex(daycomplex, httpcontext))
                //     return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update user day complex");
                return false;
            }
            return true;
        }

        public async Task<bool> SaveDayDishInComplex(List<UserDayDish> userDayDishes, string userId, int companyId)
        {
            userDayDishes.ForEach(d => { d.CompanyId = companyId; d.UserId = userId; }); 
            try
            {

                userDayDishes.ForEach(d =>
                {
                    //await saveday(d);
                    //httpcontext.User.AssignUserAttr(d);
                    var userDayDish = _context.UserDayDish.SingleOrDefault(c => c.CompanyId == d.CompanyId
                                && c.DishId == d.DishId
                                && c.Date == d.Date
                                && c.ComplexId == d.ComplexId
                                && c.UserId == d.UserId);
                    if (userDayDish != null)
                    {
                        userDayDish.Quantity = d.Quantity;
                        userDayDish.Price = d.Price;
                        _context.Update(userDayDish);
                    }
                    else if (d.Quantity > 0)
                    {
                        //d.UserId = this.User.GetUserId();

                        _context.Add(d);
                    }

                });
                _context.SaveChanges();
                //  if (!UpdateUserComplex(daycomplex, httpcontext))
                //     return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update user day dish");
                return false;
            }

            return true;
        }
        public async Task<bool> SaveUserDay(int quantity, decimal total, decimal discount,DateTime date, string userId, int companyId)
        {
            UserDay order = new UserDay();
            order.CompanyId = companyId;
            order.Date = date;
            order.UserId = userId;
            order.Quantity = quantity;
            order.Total = total-discount;
            order.Discount = discount;
            order.TotalWtithoutDiscount = total;
            order.IsConfirmed = true;

            try
            {
                
                    //await saveday(d);
                    // httpcontext.User.AssignUserAttr(d);
                    var userDay = _context.UserDay.SingleOrDefault(c => c.CompanyId == order.CompanyId
                                && c.Date == order.Date
                                && c.UserId == order.UserId);
                    if (userDay != null)
                    {
                    userDay.Quantity += order.Quantity;
                    // userDay.Total += order.Total;
                    userDay.Total = total - discount;
                    userDay.TotalWtithoutDiscount = total;
                    userDay.Discount = discount;
                    userDay.IsConfirmed = true;
                        _context.Update(userDay);
                    }
                    else if (order.Quantity > 0)
                    {
                        //d.UserId = this.User.GetUserId();

                        _context.Add(order);
                    }

               
                _context.SaveChanges();
                //  if (!UpdateUserComplex(daycomplex, httpcontext))
                //     return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update user day");
                return false;
            }
            return true;
        }
        public async Task<bool> UserFinanceEdit(decimal total, string userId, int companyId, bool add)
        {
//            exec MakeOrderPayment '2020-08-07', 1
//Select*
//from UserFinOutComes
            DateTime date =  DateTime.Now;
            try
            {

                //await saveday(d);
                // httpcontext.User.AssignUserAttr(d);
                var userFinance = _context.UserFinances.SingleOrDefault(c => c.CompanyId == companyId && c.Id == userId);
                if (userFinance != null)
                {
                    userFinance.LastUpdated = date;
                    if (add)
                    {
                        userFinance.TotalPreOrderBalance += total;
                    }
                    else
                    {
                        userFinance.TotalPreOrderBalance -= total;
                    }
                    _context.Update(userFinance);
                }
                else 
                {
                    //d.UserId = this.User.GetUserId();

                    //_context.Add(order);
                }


                _context.SaveChanges();
                //  if (!UpdateUserComplex(daycomplex, httpcontext))
                //     return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update user finance");
                return false;
            }
            return true;
        }
        public async Task<bool> SaveComplexAndDishesDay(List<UserDayComplex> daycomplex, List<UserDayDish> userDayDishes, string userId, int companyId)
        {
            decimal total = 0;
            int quan = 0;
            daycomplex = daycomplex.Where(d => d.Quantity > 0).ToList();
            userDayDishes = userDayDishes.Where(d => d.Quantity > 0).ToList();
            daycomplex.ForEach(d => {
                if (d.Quantity > 0)
                {
                    quan += d.Quantity;
                    total += d.Price * d.Quantity;
                }
            });
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var discountplugin = _plugins.GetDiscointPlugin();
                decimal discount = 0;
                var res = OrderedComplexDay(daycomplex.First().Date, userId, companyId).ToList();
                bool ordered = res.Any(x => daycomplex.Any(y => y.ComplexId == x.ComplexId));
                if(ordered)
                {
                    _logger.LogWarning("Already ordered complex in User Day {0} userId {1}", daycomplex.First().Date, userId);
                    return false;
                }
                res.ForEach(ord => {
                    total += ord.Price*ord.Quantity;
                    daycomplex.Add(new UserDayComplex() { ComplexId = ord.ComplexId }); 
                });
                if (discountplugin != null)
                {
                    daycomplex.ForEach(dc => { dc.Complex = _context.Complex.Find(dc.ComplexId); });
                    //discountplugin.CalculateComplexDayDiscount(daycomplex, userDayDishes);
                    discount = discountplugin.GetComplexDayDiscount(daycomplex,companyId);
                }
                if (!await SaveDayComplex(daycomplex, userId, companyId))
                    return false;


                if (!await SaveDayDishInComplex(userDayDishes, userId, companyId))
                    return false;
                if (!await SaveUserDay(quan, total,discount, daycomplex.First().Date, userId, companyId))
                    return false;
                //if (!await UserFinanceEdit(total,userId, companyId,false))
                //    return false;
                scope.Complete();
            }
            return true;
        }
        //Delete ordered complex with dishes
        public async Task<bool> DeleteDayComplex(UserDayComplex userDayComplex, string userId, int companyId)
        {
            
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var discountplugin = _plugins.GetDiscointPlugin();
                decimal discount = 0;
                decimal total = 0;
                var res = OrderedComplexDay(userDayComplex.Date, userId, companyId).ToList();
                List<UserDayComplex> daycomplex = new List<UserDayComplex>();
                res.ForEach(ord => {
                    if (userDayComplex.ComplexId != ord.ComplexId)
                    {
                        //total += ord.Price;
                        total += ord.Price * ord.Quantity;
                        daycomplex.Add(new UserDayComplex() { ComplexId = ord.ComplexId });
                    }
                });
                if (discountplugin != null)
                {
                    daycomplex.ForEach(dc => { dc.Complex = _context.Complex.Find(dc.ComplexId); });
                    //discountplugin.CalculateComplexDayDiscount(daycomplex, userDayDishes);
                    //discount = discountplugin.GetComplexDayDiscount(daycomplex);
                    discount = discountplugin.GetComplexDayDiscount(daycomplex, companyId);
                }
                if (!await DeleteDayComplexDb(userDayComplex, userId, companyId))
                    return false;


                if (!await DeleteDayDishInComplex(userDayComplex, userId, companyId))
                    return false;
                if (!await DeleteUserDay(total, discount, userDayComplex.Quantity, userDayComplex.Date, userId, companyId))
                    return false;
                //if (!await UserFinanceEdit(userDayComplex.Price, userId, companyId, true))
                //    return false;
                scope.Complete();
            }
            return true;
        }
        private async Task<bool> DeleteDayComplexDb(UserDayComplex userDayComplex, string userId, int companyId)
        {
            //var userId = httpcontext.User.GetUserId();
            //var companyId = httpcontext.User.GetCompanyID();
            try
            {
                var existing_db = await _context.UserDayComplex.Where
                    (di => di.ComplexId == userDayComplex.ComplexId &&
                    di.CompanyId==companyId &&
                    di.UserId == userId &&
                    di.Date==userDayComplex.Date).ToListAsync();
                if (existing_db.Count() == 0)
                {
                    _logger.LogError("Delete UserDayComplex that doesn't exists {0}",userId);
                    return false;
                }
                _context.UserDayComplex.RemoveRange(existing_db);
                

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete UserDayComplex");
                return false;
            }
            return true;
        }
        private async Task<bool> DeleteDayDishInComplex(UserDayComplex userDayComplex, string userId, int companyId)
        {
            //var userId = httpcontext.User.GetUserId();
            //var companyId = httpcontext.User.GetCompanyID();
            try
            {
                var existing_db = await _context.UserDayDish.Where
                    (di => di.ComplexId == userDayComplex.ComplexId &&
                    di.CompanyId == companyId &&
                    di.UserId == userId &&
                    di.Date == userDayComplex.Date &&
                    di.IsComplex == true).ToListAsync();
                _context.UserDayDish.RemoveRange(existing_db);


                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete UserDayDish");
                return false;
            }
            return true;
        }
        private async Task<bool> DeleteUserDay(decimal total, decimal discount,int quantity,DateTime date , string userId, int companyId)
        {
            //var userId = httpcontext.User.GetUserId();
            //var companyId = httpcontext.User.GetCompanyID();
            try
            {
                var existing_db = await _context.UserDay.Where
                    (di => di.CompanyId == companyId &&
                    di.UserId == userId &&
                    di.Date == date).ToListAsync();
                if (existing_db.FirstOrDefault().Quantity > 1&& existing_db.FirstOrDefault().Quantity!=quantity)
                {
                    var userDay = existing_db.FirstOrDefault();
                    if (userDay != null)
                    {
                        userDay.Quantity -= quantity;
                        userDay.Total = total-discount;
                        userDay.TotalWtithoutDiscount = total;
                        userDay.Discount = discount;
                        _context.Update(userDay);
                    }
                }
                else
                {
                    _context.UserDay.RemoveRange(existing_db);
                }

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete UserDayComplex");
                return false;
            }
            return true;
        }
        private bool UpdateUserComplex(List<UserDayComplex> daycomplex, HttpContext httpcontext)
        {
            var userid = httpcontext.User.GetUserId();
            var companyid = httpcontext.User.GetCompanyID();
            bool isnew = false;
            UserDay userDay = null;


            return true;


        }
        /*   old , to be deleted further */
        public IQueryable<CustomerOrdersViewModel> CustomerOrders_old(DateTime daydate, int companyid)
        {
            var query1 =
                           from dd in _context.DayDish.Where(dd => dd.CompanyId == companyid && dd.Date == daydate)
                           join d in _context.Dishes.Where(dd => dd.CompanyId == companyid) on dd.DishId equals d.Id
                           join ud in _context.UserDayDish.Where(ud => ud.CompanyId == companyid && ud.Date == daydate) on dd.DishId equals ud.DishId
                           join cu in _context.Users on ud.UserId equals cu.Id
                           select new CustomerOrdersDetailsViewModel
                           {
                               UserId = cu.Id,
                               UserName = cu.NormalizedUserName,
                               DishId = d.Id,
                               CategoryId = d.CategoriesId,
                               DishName = d.Name,
                               Date = daydate,
                               Quantity = ud.Quantity,
                               Price = d.Price,
                               Amount = ud.Quantity * d.Price
                           };
            var query2 = from entry in query1
                         group entry by entry.UserId into grp
                         select new CustomerOrdersViewModel
                         {
                             UserId = grp.Key,
                             UserName = grp.Min(a => a.UserName), //! todo
                             Date = daydate,
                             DishesCount = grp.Count(),
                             Amount = grp.Sum(a => a.Amount)

                         };

            /*
                        var query = from entry in (
                                       from dd in _context.DayDish
                                       join d in _context.Dishes on dd.DishId equals d.Id
                                       where dd.Date == daydate && dd.CompanyId == companyid
                                       join ud in _context.UserDayDish on new { dd.DishId, dd.Date, cid = companyid } equals new { ud.DishId, ud.Date, cid = ud.CompanyId }
                                       join cu in _context.Users on ud.UserId equals cu.Id
                                       select new { UserId = cu.Id, UserName = cu.NormalizedUserName, DishId = d.Id, CategoryID = d.CategoriesId, DishName = d.Name, Date = daydate, ItemQuanity = ud.Quantity, ItemPrice = d.Price, ItemAmount = ud.Quantity * d.Price }
                                       )
                                    group entry by entry.UserId into ordergroup
                                  //  join cat in _context.Categories on new { id = ordergroup.First().CategoryID, cid = companyid } equals new { id = cat.Id, cid = cat.CompanyId }

                                    select new CustomerOrdersViewModel
                                    {
                                        UserId= ordergroup.Key,
                                        UserName = ordergroup.First().UserName,
                                      //  Date = daydate,
                                      //  DishesCount = ordergroup.Count(),
                                      //  Amount= ordergroup.Sum(a=>a.ItemAmount)

                                    };
                */
            return query2;

        }
        public IQueryable<UserDayComplexViewModel> ComplexPerDay(DateTime daydate, string userId, int companyid)
        {
            var query = from comp in _context.Complex

                        join dc in (from subday in _context.DayComplex where subday.Date == daydate && subday.CompanyId == companyid select subday) on comp.Id equals dc.ComplexId
                        join dd in (from usubday in _context.UserDayComplex where usubday.UserId == userId && usubday.Date == daydate && usubday.CompanyId == companyid select usubday) on dc.ComplexId equals dd.ComplexId into proto
                        from dayd in proto.DefaultIfEmpty()

                        select new UserDayComplexViewModel()
                        {
                            ComplexId = comp.Id,
                            ComplexName = comp.Name,
                            Quantity = dayd.Quantity,
                            Price = comp.Price,
                            Date = daydate,
                            Enabled = dayd.Date == daydate,  /*dayd != null*/
                            ComplexDishes = from d in _context.Dishes.WhereCompany(companyid)
                                            join dc in _context.DishComplex.WhereCompany(companyid) on d.Id equals dc.DishId
                                            //join udd in _context.UserDayDish.WhereCompany(companyid).Where(i => i.Date == daydate && i.UserId == userId)  on d.Id equals udd.DishId
                                            where dc.ComplexId == comp.Id
                                            orderby dc.DishCourse
                                            select new UserDayComplexDishViewModel()
                                            {

                                                DishId = d.Id,
                                                DishName = d.Name,
                                                DishReadyWeight = d.ReadyWeight,
                                                PictureId = d.PictureId,
                                                DishCourse = dc.DishCourse,
                                                //  DishQuantity = udd.Quantity,

                                                DishDescription = d.Description,
                                                DishIngredients = string.Join(",", from di in _context.DishIngredients.WhereCompany(companyid).Where(t => t.DishId == d.Id)
                                                                                   join ingr in _context.Ingredients on di.IngredientId equals ingr.Id
                                                                                   select ingr.Name),
                                            }
                        };
            return query;
        }




        public IQueryable<UserDayComplexViewModel> AvaibleComplexDay(DateTime daydate, string userId, int companyid)
        {
            var ordered = OrderedComplexDay(daydate, userId, companyid);
            var orderedList = ordered.ToList();
            var query = from comp in _context.Complex
                        join dc in (from subday in _context.DayComplex where subday.Date == daydate && subday.CompanyId == companyid select subday) on comp.Id equals dc.ComplexId
                        join cat in _context.Categories.WhereCompany(companyid) on comp.CategoriesId equals cat.Id
                        join dk in _context.DishesKind on comp.DishKindId equals dk.Id into leftdk
                        from subdk in leftdk.DefaultIfEmpty()
                        select new UserDayComplexViewModel()
                        {
                            ComplexId = comp.Id,
                            ComplexName = comp.Name,
                            ComplexCategoryId = cat.Id,
                            ComplexCategoryName = cat.Name,
                            ComplexCategoryCode = cat.Code,
                            DishKindId = comp.DishKindId,
                            Quantity = 0,
                            Price = comp.Price,
                            Date = daydate,
                            Enabled = dc.Date == daydate,  /*dayd != null*/
                            ComplexDishes = from d in _context.Dishes.WhereCompany(companyid)
                                            join dishCom in _context.DishComplex.WhereCompany(companyid) on d.Id equals dishCom.DishId
                                            //join udd in _context.UserDayDish.WhereCompany(companyid).Where(i => i.Date == daydate && i.UserId == userId)  on d.Id equals udd.DishId
                                            where dishCom.ComplexId == comp.Id
                                            orderby dishCom.DishCourse ascending,dishCom.IsDefault descending
                                            select new UserDayComplexDishViewModel()
                                            {

                                                DishId = d.Id,
                                                DishName = d.Name,
                                                DishReadyWeight = d.ReadyWeight,
                                                PictureId = d.PictureId,
                                                DishCourse = dishCom.DishCourse,
                                                IsDefault = dishCom.IsDefault,
                                                //  DishQuantity = udd.Quantity,

                                                DishDescription = d.Description,
                                                DishIngredients =""/* string.Join(",", from di in _context.DishIngredients.WhereCompany(companyid).Where(t => t.DishId == d.Id)
                                                                                   join ingr in _context.Ingredients on di.IngredientId equals ingr.Id
                                                                                   select ingr.Name)*/
                                            }
                        };
            query = query.Where(x => !ordered.Any(o => o.ComplexCategoryId == x.ComplexCategoryId));
            //foreach (var item in ordered) {
            //    query = query.Where(x => x.ComplexCategoryId != item.ComplexCategoryId);
            //        }
            return query;
        }
        public IEnumerable<UserDayComplexViewModel> WeekOrder(DateTime dayFrom, DateTime dayTo, string userId, int companyid)
        {
            var avaibleComplexDays = from comp in _context.DayComplex
                                     where comp.Date >= dayFrom && comp.Date <= dayTo && comp.CompanyId == companyid
                                     select new UserDayComplexViewModel()
                                     {
                                         ComplexId = comp.ComplexId,
                                         Date = comp.Date,
                                         Enabled = false
                                     };
            var ordered = from comp in _context.Complex
                              // join udd in (from subday in _context.UserDayDish where subday.Date == daydate && subday.CompanyId == companyid select subday) on comp.Id equals udd.ComplexId
                          join cat in _context.Categories.WhereCompany(companyid) on comp.CategoriesId equals cat.Id
                          join dd in (from usubday in _context.UserDayComplex where usubday.UserId == userId && usubday.Date >= dayFrom && usubday.Date <= dayTo && usubday.CompanyId == companyid select usubday) on comp.Id equals dd.ComplexId into proto
                          from dayd in proto.DefaultIfEmpty()
                          where dayd.Quantity > 0
                          select new UserDayComplexViewModel()
                          {
                              ComplexId = comp.Id,
                              ComplexName = comp.Name,
                              ComplexCategoryId = cat.Id,
                              ComplexCategoryName = cat.Name,
                              Quantity = dayd.Quantity,
                              Price = comp.Price,
                              Date = dayd.Date,
                              Enabled = true,  /*dayd != null*/
                              ComplexDishes = from d in _context.Dishes.WhereCompany(companyid)
                                                  //join dc in _context.DishComplex.WhereCompany(companyid) on d.Id equals dc.DishId
                                              join udd in _context.UserDayDish.WhereCompany(companyid).Where(i => i.Date >= dayFrom && i.Date <= dayTo && i.UserId == userId && i.ComplexId == comp.Id) on d.Id equals udd.DishId
                                              where udd.ComplexId == comp.Id
                                              //   orderby dc.DishCourse
                                              select new UserDayComplexDishViewModel()
                                              {

                                                  DishId = d.Id,
                                                  DishName = d.Name,
                                                  DishReadyWeight = d.ReadyWeight,
                                                  PictureId = d.PictureId,
                                                  // DishCourse = dc.DishCourse,
                                                  DishQuantity = udd.Quantity,

                                                  DishDescription = d.Description,
                                                  DishIngredients = string.Join(",", from di in _context.DishIngredients.WhereCompany(companyid).Where(t => t.DishId == d.Id)
                                                                                     join ingr in _context.Ingredients on di.IngredientId equals ingr.Id
                                                                                     select ingr.Name),
                                              }
                          };
            var ordered_list = ordered.ToList();
            var avaibleList = avaibleComplexDays.ToList();
            avaibleList.ForEach(a => { 
                if (!ordered_list.Any(o => o.Date == a.Date))
                {
                    ordered_list.Add(a);
                }
                
            });
            return ordered_list;
        }
        public IQueryable<UserDayComplexViewModel> OrderedComplexDay(DateTime daydate, string userId, int companyid)
        {
            var confirmed = from ud in _context.UserDay
                            where ud.CompanyId == companyid & ud.Date == daydate && ud.UserId == userId
                             select new UserDay();
            var query = from comp in _context.Complex
                            // join udd in (from subday in _context.UserDayDish where subday.Date == daydate && subday.CompanyId == companyid select subday) on comp.Id equals udd.ComplexId
                        join cat in _context.Categories.WhereCompany(companyid) on comp.CategoriesId equals cat.Id
                        join uday in _context.UserDay.Where(ud => ud.CompanyId == companyid & ud.Date == daydate && ud.UserId == userId) on comp.CompanyId equals uday.CompanyId
                        join dd in (from usubday in _context.UserDayComplex where usubday.UserId == userId && usubday.Date == daydate && usubday.CompanyId == companyid select usubday) on comp.Id equals dd.ComplexId into proto
                        from dayd in proto.DefaultIfEmpty()
                        where dayd.Quantity>0
                        select new UserDayComplexViewModel()
                        {
                            ComplexId = comp.Id,
                            ComplexName = comp.Name,
                            ComplexCategoryId = cat.Id,
                            ComplexCategoryName = cat.Name,
                            Quantity = dayd.Quantity,
                            Price = dayd.Price,
                            Date = daydate,
                            Confirmed = uday.IsConfirmed,
                            Total = uday.Total,
                            TotalWithoutDiscount = uday.TotalWtithoutDiscount,
                            Discount = uday.Discount,
                            Enabled = dayd.Date == daydate,  /*dayd != null*/
                            ComplexDishes = from d in _context.Dishes.WhereCompany(companyid)
                                            //join dc in _context.DishComplex.WhereCompany(companyid) on d.Id equals dc.DishId
                                            join udd in _context.UserDayDish.WhereCompany(companyid).Where(i => i.Date == daydate && i.UserId == userId && i.ComplexId==comp.Id)  on d.Id equals udd.DishId
                                            where udd.ComplexId == comp.Id
                                            //orderby dc.DishCourse
                                            select new UserDayComplexDishViewModel()
                                            {

                                                DishId = d.Id,
                                                DishName = d.Name,
                                                DishReadyWeight = d.ReadyWeight,
                                                PictureId = d.PictureId,
                                               // DishCourse = dc.DishCourse,
                                                DishQuantity = udd.Quantity,

                                                DishDescription = d.Description,
                                                DishIngredients =""/* string.Join(",", from di in _context.DishIngredients.WhereCompany(companyid).Where(t => t.DishId == d.Id)
                                                                                   join ingr in _context.Ingredients on di.IngredientId equals ingr.Id
                                                                                   select ingr.Name)*/
                                            }
                        };
            var query1 = query.ToList();
            var confirmed1 = confirmed.ToList();
            query1.ForEach(d => d.Confirmed = confirmed1.FirstOrDefault().IsConfirmed);
            return query;
        }
        public IEnumerable<SelectListItem> DishesKind(DateTime dateFrom, DateTime dateTo, int companyid)
        { 
            var query = (from udc in _context.DayComplex.WhereCompany(companyid).Where(u=> u.Date>=dateFrom&&u.Date<=dateTo)
                        join com in _context.Complex on udc.ComplexId equals com.Id
                        join dc in _context.DishesKind on com.DishKindId equals dc.Id
                        select new
                        {
                            Code = dc.Code,
                            Value = dc.Id.ToString(),
                            Text = dc.Name
                        }).Distinct().OrderBy(dc => dc.Code).ToList();
            List<SelectListItem> res = new List<SelectListItem>();
            query.ForEach(dc =>
            {
                res.Add(new SelectListItem() { Value = dc.Value, Text = dc.Text });
            });
            
            return res;
        }
        }
}
     //to do make a separate context for async
/*
Func<UserDayDish, Task<bool>> saveday = async d =>  {

        var userDayDish = await _context.UserDayDish.FindAsync(this.User.GetUserId(), d.Date, d.DishId);
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
*/
                  