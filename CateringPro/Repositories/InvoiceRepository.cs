using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace CateringPro.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;
        //private readonly UserDayDishesRepository _userDayDishesRepository;
        private readonly ILogger<CompanyUser> _logger;
        public InvoiceRepository(AppDbContext context,  ILogger<CompanyUser> logger)
        {
            _context = context;
            _logger = logger;
            //_userDayDishesRepository = new UserDayDishesRepository(context, logger,);
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
                    StampPictureId = company.StampPictureId,
                    UrlPicture = string.Format(@"http://catering.in.ua/Pictures/GetPicture/{0}", company.PictureId),
                    UrlStampPicture= string.Format(@"http://catering.in.ua/Pictures/GetPicture/{0}", company.StampPictureId)
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
        public InvoiceModel CustomerInvoice(string UserId, DateTime daydate, int companyid)
        {
            InvoiceModel res = new InvoiceModel();
            try
            {

                res.Buyer = GetUserCompany(UserId);
                res.Seller = GetOwnCompany(companyid);
                var query1 = from dd in _context.DayDish.Where(dd => dd.CompanyId == companyid && dd.Date == daydate)
                             join d in _context.Dishes.Where(dd => dd.CompanyId == companyid) on dd.DishId equals d.Id
                             join ud in _context.UserDayDish.Where(ud => ud.UserId== UserId && ud.CompanyId == companyid && ud.Date == daydate) on dd.DishId equals ud.DishId
                             //join cu in _context.Users on ud.UserId equals cu.Id
                             select new InvoiceItemModel
                             {
                                 Code = d.Code,
                                 Name = d.Name,
                                 Quantity = ud.Quantity,
                                 Price = ud.Price,
                                 Amount = ud.Quantity * d.Price
                             };
                var ordered = from comp in _context.Complex
                                  // join udd in (from subday in _context.UserDayDish where subday.Date == daydate && subday.CompanyId == companyid select subday) on comp.Id equals udd.ComplexId
                              join cat in _context.Categories.WhereCompany(companyid) on comp.CategoriesId equals cat.Id
                              join dd in (from usubday in _context.UserDayComplex where usubday.UserId == UserId && usubday.Date == daydate && usubday.CompanyId == companyid select usubday) on comp.Id equals dd.ComplexId into proto
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
                                  Date = daydate,
                                  Enabled = dayd.Date == daydate,  /*dayd != null*/
                                  ComplexDishes = from d in _context.Dishes.WhereCompany(companyid)
                                                  join dc in _context.DishComplex.WhereCompany(companyid) on d.Id equals dc.DishId
                                                  join udd in _context.UserDayDish.WhereCompany(companyid).Where(i => i.Date == daydate && i.UserId == UserId && i.ComplexId == comp.Id) on d.Id equals udd.DishId
                                                  where dc.ComplexId == comp.Id
                                                  orderby dc.DishCourse
                                                  select new UserDayComplexDishViewModel()
                                                  {

                                                      DishId = d.Id,
                                                      DishName = d.Name,
                                                      DishReadyWeight = d.ReadyWeight,
                                                      PictureId = d.PictureId,
                                                      DishCourse = dc.DishCourse,
                                                      DishQuantity = udd.Quantity,

                                                      DishDescription = d.Description,
                                                      DishIngredients = string.Join(",", from di in _context.DishIngredients.WhereCompany(companyid).Where(t => t.DishId == d.Id)
                                                                                         join ingr in _context.Ingredients on di.IngredientId equals ingr.Id
                                                                                         select ingr.Name),
                                                  }
                              };
                var ordered_list=ordered.ToList();
                var query2 = from dd in _context.DayComplex.Where(dd => dd.CompanyId == companyid && dd.Date == daydate)
                             join d in _context.Complex.Where(dd => dd.CompanyId == companyid) on dd.ComplexId equals d.Id
                             join ud in _context.UserDayComplex.Where(ud => ud.UserId == UserId && ud.CompanyId == companyid && ud.Date == daydate) on dd.ComplexId equals ud.ComplexId
                             //join cu in _context.Users on ud.UserId equals cu.Id
                             select new InvoiceItemModel
                             {
                                 Code = "",
                                 ComplexId = d.Id,
                                 Name = d.Name,
                                 Quantity = ud.Quantity,
                                 Price = ud.Price,
                                 Amount = ud.Quantity * d.Price//,
                               //  DayComplex = ordered_list.Where(x => x.ComplexId == d.Id).FirstOrDefault()
                             };
                var query3 = query2.ToList();
                query3.ForEach(it => it.DayComplex = ordered_list.Where(x => x.ComplexId == it.ComplexId).FirstOrDefault());
                var resitems = query1.ToList();
                // resitems.AddRange(query2.ToList());
                resitems.AddRange(query3);
                res.Items = resitems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CustomerInvoice User={0} ", UserId);
                return res; //to do
            }
            return res;
        }
     
        public DayProductionViewModel CompanyDayProduction(DateTime daydate, int companyid)
        {
            var query1 =
                           from dd in _context.DayDish.Where(dd => dd.CompanyId == companyid && dd.Date == daydate)
                           join d in _context.Dishes.Where(dd => dd.CompanyId == companyid) on dd.DishId equals d.Id
                           join ud in _context.UserDayDish.Where(ud => ud.CompanyId == companyid && ud.Date == daydate) on dd.DishId equals ud.DishId
                           group ud by new { id = d.Id, name = d.Name ,code=d.Code} into grp
                           select new
                           {
                               DishId = grp.Key.id,
                               DishCode= grp.Key.code,
                               DishName = grp.Key.name,
                               Quantity=grp.Sum(it=>it.Quantity)
                           };
            DayProductionViewModel res = new DayProductionViewModel()
            {
                Company = GetOwnCompany(companyid),
                Items= from  q in query1
                       select new DayProductionDishViewModel()
                       {
                           DishCode=q.DishCode,
                           DishName=q.DishName,
                           Quantity=q.Quantity
                       }


            };
          

            return res;
        }
        public DayIngredientsViewModel CompanyDayIngredients(DateTime daydate, int companyid)
        {
            {
                var query1 =
                               from ud in _context.UserDayDish.Where(ud => ud.CompanyId == companyid && ud.Date == daydate)
                               join d in _context.Dishes.Where(dd => dd.CompanyId == companyid) on ud.DishId equals d.Id
                               join di in _context.DishIngredients.Where(ud => ud.CompanyId == companyid ) on d.Id equals di.DishId
                               join ing in _context.Ingredients.Where(ud => ud.CompanyId == companyid) on di.IngredientId equals ing.Id
                               group new { di,ud} by new { id = ing.Id, name = ing.Name,mu=ing.MeasureUnit} into grp
                               select new
                               {
                                   IngId = grp.Key.id,
                                   
                                   IngName = grp.Key.name,
                                   IngMu = grp.Key.mu,
                                   Quantity = grp.Sum(it=>it.ud.Quantity*it.di.Proportion),
                                   DishQuantity = grp.Sum(it => it.ud.Quantity )
                               };
                DayIngredientsViewModel res = new DayIngredientsViewModel()
                {
                    Company = GetOwnCompany(companyid),
                    Items = from q in query1
                            select new DayIngredientsDetails()
                            {
                                IngredientId = q.IngId,
                                IngredientName = q.IngName,
                                Quantity = q.Quantity,
                                MeasureUnit=q.IngMu,
                                DishQuantity=q.DishQuantity
                            }


                };


                return res;
            }
        }
    }
}
