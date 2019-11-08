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

namespace CateringPro.Repositories
{
    public class UserDayDishesRepository : IUserDayDishesRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        private readonly UserManager<CompanyUser> _userManager;

        public UserDayDishesRepository(AppDbContext context, ILogger<CompanyUser> logger, UserManager<CompanyUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public  bool IsAllowDayEdit( DateTime dt, int companyid)
        {
            var company = _context.Companies.Find(companyid);
            if (company == null)
                return false;
            DateTime max = DateTime.Now.AddHours(company.OrderThresholdTimeH.HasValue ? company.OrderThresholdTimeH.Value : 24);
            DateTime min = DateTime.Now.AddHours(-(company.OrderLeadTimeH.HasValue ? company.OrderLeadTimeH.Value : 24));
            if ((dt - min).TotalDays < 7)
            for(DateTime t=dt;t>min ; t = t.AddDays(-1))
            {
                if(t.DayOfWeek== DayOfWeek.Saturday || t.DayOfWeek == DayOfWeek.Sunday)
                {
                        min = min.AddDays(-1);
                }
            }
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
        public IQueryable<UserDayDishViewModel> DishesPerDay(DateTime daydate, string userId,int companyid)
        {
            var query = from dish in _context.Dishes
                        join dd in (from subday in _context.DayDish where subday.Date == daydate &&  subday.CompanyId == companyid select subday) on dish.Id equals dd.DishId into proto
                        from dayd in proto.DefaultIfEmpty()

                        select new UserDayDishViewModel() { DishId = dish.Id, DishName = dish.Name, Date = daydate, Enabled = proto.Count() > 0/*dayd != null*/ };
            return query;
        }
        public IQueryable<UserDayDishViewModelPerGategory> CategorizedDishesPerDay(DateTime daydate,string userId, int companyid)
        {
            var query1 = from dish in _context.Dishes.Where(d => d.CompanyId == companyid)
                         join dd in _context.DayDish.Where(d => d.CompanyId == companyid && d.Date == daydate) on dish.Id equals dd.DishId
                         join ud in _context.UserDayDish.Where(ud=>ud.CompanyId==companyid && ud.UserId==userId && ud.Date== daydate) 
                             on dish.Id equals ud.DishId into Details
                         from udayd in Details.DefaultIfEmpty()
                         select new UserDayDishViewModel
                         {
                             DishId = dish.Id,
                             CategoryId = dish.CategoriesId,
                             DishName = dish.Name,
                             //Price=dish.Price,
                             DishDescription=dish.Description,
                             DishIngredientds=string.Join(",",from di in _context.DishIngredients.Where(t=>t.DishId==dish.Id)
                                               join ingr in _context.Ingredients on di.IngredientId equals ingr.Id
                                               select ingr.Name),
                             PictureId = dish.PictureId,
                             Date = daydate,
                             Quantity = udayd.Date == daydate? udayd.Quantity:0,
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
                                             Price=dd.Price,
                                             ReadyWeight=dd.ReadyWeight,
                                             KKal=dd.KKal,
                                             Quantity = dd.Quantity,
                                             PictureId = dd.PictureId,
                                             Enabled = dd.Enabled,
                                             DishDescription=dd.DishDescription,
                                             DishIngredientds=dd.DishIngredientds
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
        public DayDish SelectSingleOrDefault(int dishId, DateTime daydate) {
            return _context.DayDish.SingleOrDefault(dd => dd.DishId == dishId && dd.Date == daydate);
       }

        public IQueryable<CustomerOrdersViewModel> CustomerOrders(DateTime daydate,  int companyid)
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
                             IsConfirmed=ud.IsConfirmed,
                             IsPaid = ud.IsPaid,
                             User= new CompanyModel()
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
        public CustomerOrdersViewModel CustomerOrders(string UserId,DateTime daydate, int companyid)
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
            if (querysingle!=null)
            {
                res.UserId = querysingle.UserId;
                res.UserName = querysingle.UserName;
                res.Date = querysingle.Date;
            }


            return res;

        }

        public bool SaveDay(List<UserDayDish> daydishes,HttpContext httpcontext)
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
            UserDay userDay= null;
            
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
                
                userDay.Quantity = daydishes.Sum(d =>  d.Quantity);
                
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
            catch (Exception ex) {
                _logger.LogError(ex, "Update user day ");
                return false;
            }
            return true;


        }

        public bool SaveDayComplex(List<UserDayComplex> daycomplex, HttpContext httpcontext)
        {

            try
            {
                daycomplex.ForEach(d =>
                {
                    //await saveday(d);
                    httpcontext.User.AssignUserAttr(d);
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
                        
                        join dc in (from subday in _context.DayComplex where  subday.Date == daydate && subday.CompanyId == companyid select subday) on comp.Id equals dc.ComplexId 
                        join dd in (from usubday in _context.UserDayComplex where usubday.UserId== userId && usubday.Date == daydate && usubday.CompanyId == companyid select usubday) on  dc.ComplexId equals dd.ComplexId into proto
                        from dayd in proto.DefaultIfEmpty()

                        select new UserDayComplexViewModel() { 
                            ComplexId = comp.Id, ComplexName = comp.Name, 
                            Quantity= dayd.Quantity,
                            Price =comp.Price,
                            Date = daydate, Enabled = dayd.Date == daydate,  /*dayd != null*/
                            ComplexDishes=from d in _context.Dishes.WhereCompany(companyid)
                                          join dc in _context.DishComplex.WhereCompany(companyid) on d.Id equals dc.DishId
                                          where dc.ComplexId ==comp.Id
                                          select new UserDayComplexDishViewModel()
                                          {
                                              
                                              DishId = d.Id,
                                              DishName = d.Name,
                                              DishReadyWeight=d.ReadyWeight,
                                              PictureId = d.PictureId,
                                             
                                              DishDescription = d.Description,
                                              DishIngredients = string.Join(",", from di in _context.DishIngredients.WhereCompany(companyid).Where(t => t.DishId == d.Id)
                                                                                  join ingr in _context.Ingredients on di.IngredientId equals ingr.Id
                                                                                  select ingr.Name),
                                          }
                        };
            return query;
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
                  