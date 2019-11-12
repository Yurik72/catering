using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CateringPro.Core;
using System.Data;

namespace CateringPro.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        public ReportRepository(AppDbContext context,  ILogger<CompanyUser> logger)
        {
            _context = context;
            _logger = logger;
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
                             join ud in _context.UserDayDish.Where(ud => ud.CompanyId == companyid && ud.Date == daydate) on dd.DishId equals ud.DishId
                             join cu in _context.Users on ud.UserId equals cu.Id
                             select new InvoiceItemModel
                             {
                                 Code = d.Code,
                                 Name = d.Name,
                                 Quantity = ud.Quantity,
                                 Price = ud.Price,
                                 Amount = ud.Quantity * d.Price
                             };
                var query2= from dd in _context.DayComplex.Where(dd => dd.CompanyId == companyid && dd.Date == daydate)
                                  join d in _context.Complex.Where(dd => dd.CompanyId == companyid) on dd.ComplexId equals d.Id
                                  join ud in _context.UserDayComplex.Where(ud => ud.CompanyId == companyid && ud.Date == daydate) on dd.ComplexId equals ud.ComplexId
                                  join cu in _context.Users on ud.UserId equals cu.Id
                                  select new InvoiceItemModel
                                  {
                                      Code = "",
                                      Name = d.Name,
                                      Quantity = ud.Quantity,
                                      Price = ud.Price,
                                      Amount = ud.Quantity * d.Price
                                  };
                var resitems = query1.ToList();
                resitems.AddRange(query2.ToList());
                res.Items = resitems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CustomerInvoice User={0} ", UserId);
                return res; //to do
            }
            return res;
        }
        public DayProductioDayViewModel CompanyDayProduction(DateTime datefrom,DateTime dateto, int companyid)
        {
            var query1 =
                           from ud in _context.UserDayDish.Where(ud => ud.CompanyId == companyid && ud.Date >= datefrom   && ud.Date <= dateto) 
                           join d in _context.Dishes.Where(dd => dd.CompanyId == companyid) on ud.DishId equals d.Id
                           group ud by new { id = d.Id, name = d.Name, code = d.Code,daydate=ud.Date } into grp
                           select new
                           {
                               DayDate = grp.Key.daydate,
                               DishId = grp.Key.id,
                               DishCode = grp.Key.code,
                               DishName = grp.Key.name,
                               Quantity = grp.Sum(it => it.Quantity)
                           };
            DayProductioDayViewModel res = new DayProductioDayViewModel() { 
                    Company = GetOwnCompany(companyid),

                    Days = from q in query1.ToList()
                           group q by q.DayDate into grp
                            select new DayProductionViewModel()
                            {
                                DayDate= grp.Key,
                                Items=from it in grp
                                select new DayProductionDishViewModel()
                                {
                                    DishCode = it.DishCode,
                                    DishName = it.DishName,
                                    Quantity = it.Quantity
                                }
                            }


            };

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
        public async Task<ProductionForecastViewModel> CompanyProductionForecast(DateTime datefrom, DateTime dateto, int companyId)
        {
            ProductionForecastViewModel res = new ProductionForecastViewModel();
            res.Company = GetOwnCompany(companyId);
            Action<IDataRecord, ProductionForecastItemViewModel> materilaize = (r, d) =>   ///to do auto
            {
                d.DayDate = r.GetDateTime(0);
                //d.CompanyId
                d.IngredientId = r.GetInt32(1);
                d.Name = r.GetString(2);
                d.StockValue= r.GetDecimal(3);
                d.BeginDay= r.GetDecimal(4);
                d.ProductionQuantity= r.GetDecimal(5);
                d.DayProduction = r.GetDecimal(6);
                d.AfterDayStockValue = r.GetDecimal(7);
                d.MeasureUnit= r.GetString(8);

            };

            var query= await _context.Database.SqlQuery<ProductionForecastItemViewModel>(
                $"exec ForecastStockProduction '{datefrom.ShortSqlDate()}' ,'{dateto.ShortSqlDate()}' , {companyId}").ToListAsync();
            var query1 = from d in query
                         group d by d.DayDate into grp
                         select new ProductionForecastDateViewModel()
                         {
                             Daydate = grp.Key,
                             Items = grp

                         };
            res.Days = query1;

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
