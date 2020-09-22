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
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
//using AspNetCore;

namespace CateringPro.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        private readonly IUserDayDishesRepository _udaydishrepo;
        private readonly IInvoiceRepository _invoicerepo;
        private readonly IMemoryCache _cache;
        public ReportRepository(AppDbContext context,  ILogger<CompanyUser> logger, IUserDayDishesRepository ud, IInvoiceRepository invoicerepo, IMemoryCache cache)
        {
            _context = context;
            _logger = logger;
            _udaydishrepo = ud;
            _invoicerepo = invoicerepo;
            _cache = cache;
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
                var query1 = from dd in _context.DayDish.Where(dd=> dd.Date == daydate)
                             join d in _context.Dishes on dd.DishId equals d.Id
                             join ud in _context.UserDayDish.Where(ud =>   ud.Date == daydate) on dd.DishId equals ud.DishId
                             join cu in _context.Users on ud.UserId equals cu.Id
                             select new InvoiceItemModel
                             {
                                 Code = d.Code,
                                 Name = d.Name,
                                 Quantity = ud.Quantity,
                                 Price = ud.Price,
                                 Amount = ud.Quantity * d.Price
                             };
                var query2= from dd in _context.DayComplex.Where(dd =>  dd.Date == daydate)
                                  join d in _context.Complex on dd.ComplexId equals d.Id
                                  join ud in _context.UserDayComplex.Where(ud =>  ud.Date == daydate) on dd.ComplexId equals ud.ComplexId
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
            if (!_context.IsHttpContext())
            {
                _context.SetCompanyID(companyid);
            }
            var query1 =
                           from ud in _context.UserDayDish.Where(ud => ud.CompanyId == companyid && ud.Date >= datefrom   && ud.Date <= dateto) 
                           join udc in _context.Complex.Where(ud => ud.CompanyId == companyid ) on ud.ComplexId equals udc.Id
                           join cat in _context.Categories.Where(ud => ud.CompanyId == companyid) on udc.CategoriesId equals cat.Id
                           join d in _context.Dishes.Where(dd => dd.CompanyId == companyid) on ud.DishId equals d.Id
                           //orderby cat.Code
                           group ud by new { id = d.Id, name = d.Name, code = d.Code,
                               daydate=ud.Date,complexName=udc.Name,complexCat=cat.Name,complexCode= cat.Code,
                               weight = d.ReadyWeight
                           } into grp
                           select new
                           {
                               DayDate = grp.Key.daydate,
                               DishId = grp.Key.id,
                               DishCode = grp.Key.code,
                               DishName = grp.Key.name,
                               CategoryName = grp.Key.complexCat,
                               ComplexCode = grp.Key.complexCode,
                               Quantity = grp.Sum(it => it.Quantity),
                               ReadyWeight=grp.Key.weight* grp.Sum(it => it.Quantity)
                           };
            DayProductioDayViewModel res = new DayProductioDayViewModel() { 
                    Company = GetOwnCompany(companyid),

                    Days = from q in query1.ToList()
                           group q by q.DayDate into grp
                           orderby grp.Key.Date
                            select new DayProductionViewModel()
                            {
                                DayDate= grp.Key,
                                Items=from it in grp
                                      orderby it.ComplexCode, it.DishName
                                      select new DayProductionDishViewModel()
                                {
                                    DishCode = it.DishCode,
                                    DishId = it.DishId,
                                    DishName = it.DishName,
                                    CategoryName = it.CategoryName,
                                    ComplexCode = it.ComplexCode,
                                    ReadyWeight=it.ReadyWeight,
                                    Quantity = it.Quantity,
                                    Ingridients = (from ing in _context.Ingredients.WhereCompany(companyid)
                                                   join dishIng in _context.DishIngredients.WhereCompany(companyid) on ing.Id equals dishIng.IngredientId
                                                   where dishIng.DishId == it.DishId
                                                   select new DayIngredientsDetails()
                                                   {
                                                       IngredientId = ing.Id,
                                                       IngredientName = ing.Name,
                                                       Quantity = dishIng.Proportion,
                                                       QuantityNetto = dishIng.ProportionNetto,
                                                       MeasureUnit = ing.MeasureUnit
                                                   })
                                }
                            }


            };
            var resD = res.Days.ToList();
            resD.ForEach(
                d => {
                    var itemList = d.Items.ToList();
                    var resList = new List<DayProductionDishViewModel>();
                    itemList.ForEach(it =>
                    {
                        if (resList.Where(d => d.DishId == it.DishId && d.CategoryName == it.CategoryName).Count() != 0)
                        {
                            var item = resList.Where(d => d.DishId == it.DishId && d.CategoryName == it.CategoryName).SingleOrDefault();
                            var index = resList.FindIndex(c => c.DishId == it.DishId && c.CategoryName == it.CategoryName);
                            resList[index].ReadyWeight = item.ReadyWeight + it.ReadyWeight;
                            resList[index].Quantity = item.Quantity + it.Quantity;
                            //var item = resList.Where(d => d.DishId == it.DishId).SingleOrDefault();
                            //resList.Remove(item);
                            //item.ReadyWeight = item.ReadyWeight + it.ReadyWeight;
                            //item.Quantity = item.Quantity + it.Quantity;
                            //resList.Add(item);
                        }
                        else
                        {
                            resList.Add(it);
                        }
                    });
                    //resList = resList.OrderBy(d => d.DishCode).ToList();
                    d.Items = resList;
                }

                );
            res.Days = resD;
            return res;
        }
        public DayProductioDayViewModel CompanyDayProductionWithoutIngredients(DateTime datefrom, DateTime dateto, int companyid)
        {
            if (!_context.IsHttpContext())
            {
                _context.SetCompanyID(companyid);
            }
            var query1 =
                           from ud in _context.UserDayDish.Where(ud => ud.CompanyId == companyid && ud.Date >= datefrom && ud.Date <= dateto)
                           join udc in _context.Complex.Where(ud => ud.CompanyId == companyid) on ud.ComplexId equals udc.Id
                           join cat in _context.Categories.Where(ud => ud.CompanyId == companyid) on udc.CategoriesId equals cat.Id
                           join d in _context.Dishes.Where(dd => dd.CompanyId == companyid) on ud.DishId equals d.Id
                           //orderby cat.Code
                           group ud by new
                           {
                               id = d.Id,
                               name = d.Name,
                               code = d.Code,
                               daydate = ud.Date,
                               complexName = udc.Name,
                               complexCat = cat.Name,
                               complexCode = cat.Code,
                               weight = d.ReadyWeight
                           } into grp
                           select new
                           {
                               DayDate = grp.Key.daydate,
                               DishId = grp.Key.id,
                               DishCode = grp.Key.code,
                               DishName = grp.Key.name,
                               CategoryName = grp.Key.complexCat,
                               ComplexCode = grp.Key.complexCode,
                               Quantity = grp.Sum(it => it.Quantity),
                               ReadyWeight = grp.Key.weight * grp.Sum(it => it.Quantity)
                           };
            DayProductioDayViewModel res = new DayProductioDayViewModel()
            {
                Company = GetOwnCompany(companyid),

                Days = from q in query1.ToList()
                       group q by q.DayDate into grp
                       orderby grp.Key.Date
                       select new DayProductionViewModel()
                       {
                           DayDate = grp.Key,
                           Items = from it in grp
                                   orderby it.ComplexCode, it.DishName
                                   select new DayProductionDishViewModel()
                                   {
                                       DishCode = it.DishCode,
                                       DishId = it.DishId,
                                       DishName = it.DishName,
                                       CategoryName = it.CategoryName,
                                       ComplexCode = it.ComplexCode,
                                       ReadyWeight = it.ReadyWeight,
                                       Quantity = it.Quantity
                                       //Ingridients = (from ing in _context.Ingredients.WhereCompany(companyid)
                                       //               join dishIng in _context.DishIngredients.WhereCompany(companyid) on ing.Id equals dishIng.IngredientId
                                       //               where dishIng.DishId == it.DishId
                                       //               select new DayIngredientsDetails()
                                       //               {
                                       //                   IngredientId = ing.Id,
                                       //                   IngredientName = ing.Name,
                                       //                   Quantity = dishIng.Proportion,
                                       //                   MeasureUnit = ing.MeasureUnit
                                       //               })
                                   }
                       }


            };
            var resD = res.Days.ToList();
            resD.ForEach(
                d => {
                    var itemList = d.Items.ToList();
                    var resList = new List<DayProductionDishViewModel>();
                    itemList.ForEach(it =>
                    {
                        if (resList.Where(d => d.DishId == it.DishId && d.CategoryName==it.CategoryName).Count() != 0)
                        {
                            var item = resList.Where(d => d.DishId == it.DishId && d.CategoryName == it.CategoryName).SingleOrDefault();
                            var index = resList.FindIndex(c => c.DishId == it.DishId && c.CategoryName == it.CategoryName);
                            resList[index].ReadyWeight = item.ReadyWeight + it.ReadyWeight;
                            resList[index].Quantity = item.Quantity + it.Quantity;
                            //resList.Remove(item);
                            //item.ReadyWeight = item.ReadyWeight + it.ReadyWeight;
                            //item.Quantity = item.Quantity + it.Quantity;
                            //resList.Add(item);
                        }
                        else
                        {
                            resList.Add(it);
                        }
                    });
                    //resList = resList.OrderBy(d => d.DishCode).ToList();
                    d.Items = resList;
                }
                
                );
            res.Days = resD;
            return res;
        }
        public DayProductionViewModel CompanyDayProduction(DateTime daydate, int companyid)
        {
            
            if (!_context.IsHttpContext())
            {
                _context.SetCompanyID(companyid);
            }
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
                           Quantity=q.Quantity,
                           Ingridients = (from ing in _context.Ingredients.WhereCompany(companyid)
                                          join dishIng in _context.DishIngredients.WhereCompany(companyid) on ing.Id equals dishIng.IngredientId
                                          select new DayIngredientsDetails()
                                          {
                                              IngredientId = ing.Id,
                                              IngredientName = ing.Name,
                                              Quantity = dishIng.Proportion,
                                              QuantityNetto = dishIng.ProportionNetto,
                                              MeasureUnit = ing.MeasureUnit
                                          })
                       }


            };
          


            return res;
        }
        //Mass email week order invoice
        public InvoiceModel EmailWeekInvoice(DateTime daydate, int companyid, CompanyUser user)
        {
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)daydate.DayOfWeek + 7) % 7;
            daydate = daydate.AddDays(daysUntilMonday);
            if (!_context.IsHttpContext())
            {
                _context.SetCompanyID(companyid);
            }
            string userid = user.Id;
            InvoiceModel res = new InvoiceModel();
            try
            {

                var model = _invoicerepo.CustomerInvoice(userid, daydate, companyid);
                var avaible = _udaydishrepo.AvaibleComplexDay(daydate, userid, companyid);
                var items = model.Items.ToList();
                if (avaible.Count() > 0 && items.Count() == 0)
                {
                    var inItem = new InvoiceItemModel();
                    inItem.DayComplex = new UserDayComplexViewModel();
                    inItem.DayComplex.Date = daydate;
                    items.Add(inItem);
                    model.Items = items;
                }
                
                for (int i = 0; i < 6; i++)
                {
                    daydate = daydate.AddDays(1);
                    avaible = _udaydishrepo.AvaibleComplexDay(daydate, userid, companyid);
                    var nextModel = _invoicerepo.CustomerInvoice(userid, daydate, companyid);
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

                    }
                    items.AddRange(nextItems);
                    model.Items = items;


                }
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mass email User={0} ");
                return res; //to do
            }


        }
        public async Task<ProductionForecastViewModel> CompanyProductionForecast(DateTime datefrom, DateTime dateto, int companyId)
        {
            if (!_context.IsHttpContext())
            {
                _context.SetCompanyID(companyId);
            }
            ProductionForecastViewModel res = new ProductionForecastViewModel();
            res.Company = GetOwnCompany(companyId);
            //Action<IDataRecord, ProductionForecastItemViewModel> materilaize = (r, d) =>   ///to do auto
            //{
            //    d.DayDate = r.GetDateTime(0);
            //    //d.CompanyId
            //    d.IngredientId = r.GetInt32(1);
            //    d.Name = r.GetString(2);
            //    d.StockValue= r.GetDecimal(3);
            //    d.BeginDay= r.GetDecimal(4);
            //    d.ProductionQuantity= r.GetDecimal(5);
            //    d.DayProduction = r.GetDecimal(6);
            //    d.AfterDayStockValue = r.GetDecimal(7);
            //    d.MeasureUnit= r.GetString(8);
            //    d.IngredientCategoriesId= r.GetInt32(9);
            //    d.IngridientCategoriesName = r.GetString(10);

            //};

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
            if (!_context.IsHttpContext())
            {
                _context.SetCompanyID(companyid);
            }
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
                                   QuantityNetto = grp.Sum(it => it.ud.Quantity * it.di.ProportionNetto),
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
                                QuantityNetto=q.QuantityNetto,
                                MeasureUnit =q.IngMu,
                                DishQuantity=q.DishQuantity
                            }


                };


                return res;
            }
        }

        public IEnumerable<CompanyMenuComplexModel> CompanyComplexMenu(DateTime datefrom, DateTime dateto, int companyid)
        {
            if (!_context.IsHttpContext())
            {
                _context.SetCompanyID(companyid);
            }
            return from dc in _context.DayComplex.Where(dc => dc.CompanyId == companyid && dc.Date >= datefrom && dc.Date <= dateto)
            join c in _context.Complex.Where(c => c.CompanyId == companyid) on dc.ComplexId equals c.Id

            select new CompanyMenuComplexModel()
            {
                DayDate = dc.Date,

                Name = c.Name,
                Price = c.Price,
                Items = from dd in _context.DishComplex.WhereCompany(companyid).Where(it => it.ComplexId == c.Id)
                        join d in _context.Dishes.WhereCompany(companyid) on dd.DishId equals d.Id
                        select new CompanyMenuItemModel()
                        {
                            Name = d.Name,
                            Description = d.Description,
                            Weight = d.ReadyWeight
                        }
            };
        }
        public CompanyMenuModel CompanyMenu(DateTime datefrom, DateTime dateto, int companyid)
        {
            if (!_context.IsHttpContext())
            {
                _context.SetCompanyID(companyid);
            }
            var query1 =
                           (from ud in _context.DayDish.Where(ud => ud.CompanyId == companyid && ud.Date >= datefrom && ud.Date <= dateto)
                           join d in _context.Dishes.Where(dd => dd.CompanyId == companyid) on ud.DishId equals d.Id
                           
                           select new CompanyMenuItemModel()
                           {
                               DayDate = ud.Date,
                               Code = d.Code,
                               Name = d.Name,
                               Price = d.Price
                           }).ToList();
            var query11 = (from q1 in query1
                          group q1 by q1.DayDate into grp
                          select new CompanyMenuDayModel()
                          {
                              DayDate = grp.Key,
                              Items = grp
                          }).ToList();

            var query21= (from q2 in CompanyComplexMenu(datefrom,dateto,companyid).ToList()
                         group q2 by q2.DayDate into grp
                         select new CompanyMenuDayModel()
                         {
                             DayDate = grp.Key,
                             Items=new List<CompanyMenuItemModel>(),
                             ComplexItems = grp
                         }).ToList();

            query11.AddRange(query21);
            CompanyMenuModel res = new CompanyMenuModel()
            {
                Company = GetOwnCompany(companyid),
                Days=from day in query11

                     group day by day.DayDate into grp
                     select new CompanyMenuDayModel()
                     {
                         DayDate= grp.Key,
                         Items= grp.SelectMany(it=>it.Items).Where(it=>it!=null),
                         ComplexItems = grp.SelectMany(it => it.ComplexItems)
                     }

            };

            return res;
        }

        public DishSpecificationViewModel DishSpecification(DateTime datefrom, DateTime dateto, int companyid)
        {
            if (!_context.IsHttpContext())
            {
                _context.SetCompanyID(companyid);
            }
            DishSpecificationViewModel res = new DishSpecificationViewModel()
            {
                Company = GetOwnCompany(companyid),
                Items = (from d in _context.Dishes.Include(x => x.DishIngredients).ThenInclude(x => x.Ingredient).WhereCompany(companyid)
                         orderby d.Name
                         select new DishSpecificationItemViewModel()
                         {
                             DishId = d.Id,
                             DishName = d.Name,
                             Ingredients = d.DishIngredients.Count()>0?
                             ( from di in d.DishIngredients
                                select new DishIngredientsProportionViewModel()
                                {
                                    IngredientId = di.IngredientId,
                                    Name = di.Ingredient.Name,
                                    MeasureUnit=di.Ingredient.MeasureUnit,
                                    Proportion= di.Proportion,
                                    AvgPrice = di.Ingredient.AvgPrice
                                }).ToList()
                             :new List<DishIngredientsProportionViewModel>()
                             /*
                             (from di in _context.DishIngredients.Include(x=>x.Ingredient)
                                         .WhereCompany(companyid)
                                        .Where(x=>x.DishId==d.Id)
                                         select new DishIngredientsProportionViewModel()
                                         {
                                             IngredientId=di.IngredientId,
                                             Name=di.Ingredient.Name,
                                             Proportion=di.Proportion
                                         }).ToList()
                                         */

                         }).ToList()
            };


            return res;
        }
        public UserDayReportViewModel UserDayReport(int[] groupid, DateTime datefrom, DateTime dateto, int companyid)
        {

            var query1 =
                    from u in _context.Users.Where(u => u.UserSubGroupId.HasValue && groupid.Contains( u.UserSubGroupId.Value))
                    join g in _context.UserSubGroups on u.UserSubGroupId equals g.Id
                    join udd in _context.UserDayDish.Where(ud => ud.Date >= datefrom && ud.Date <= dateto && ud.CompanyId == companyid)on u.Id equals udd.UserId
                    //join d in _context.Dishes on udd.DishId equals d.Id
                    //join com in _context.Complex on udd.ComplexId equals com.Id
                    group u by new
                       {
                               userId = u.Id,
                               childNameSurname = u.ChildNameSurname,
                               groupId = u.UserSubGroupId,
                               groupName = g.Name,
                        dayDate = udd.Date,
                        //dishId = udd.DishId,
                        //dishName = d.Name,
                        //complexName = com.Name,
                        //complexKind = com.DishKindId

                    } into grp
                       select new
                       {
                           UserId = grp.Key.userId,
                           ChildNameSurname = grp.Key.childNameSurname,
                           GroupId = grp.Key.groupId,
                           GroupName = grp.Key.groupName,
                           DayDate = grp.Key.dayDate,
                           //DishId = grp.Key.dishId,
                           //DishName = grp.Key.dishName,
                           //ComplexName = grp.Key.complexName,
                           //ComplexKind = grp.Key.complexKind
                       };
            UserDayReportViewModel res = new UserDayReportViewModel()
            {
                Company = GetOwnCompany(companyid),

                Days = from q in query1.ToList()
                       group q by q.DayDate into grp
                       orderby grp.Key.Date
                       select new UserOrderDaysViewModel()
                       {
                           Day = grp.Key,
                           Users = from it in grp
                                 //group it by it.ChildNameSurname into child
                                   orderby it.GroupName, it.ChildNameSurname/*, it.DishName*/
                                   select new UserOneDayOrderViewModel()
                                   {
                                       UserId = it.UserId,
                                       ChildNameSurname = it.ChildNameSurname,
                                       GroupId = it.GroupId,
                                       GroupName = it.GroupName,
                                       Dishes = (from udd in _context.UserDayDish.Where(ud => ud.Date >= datefrom && ud.Date <= dateto && ud.CompanyId == companyid && ud.UserId == it.UserId)
                                                 join d in _context.Dishes on udd.DishId equals d.Id
                                                 join com in _context.Complex on udd.ComplexId equals com.Id
                                                 join kind in _context.DishesKind on com.DishKindId equals kind.Id
                                                 join cat in _context.Categories on com.CategoriesId equals cat.Id
                                                 orderby cat.Code, com.Name
                                                 select new UserDayReportDishesViewModel()
                                                 {
                                                     DishId = udd.DishId,
                                                     DishName = d.Name,
                                                     ComplexName = com.Name,
                                                     ComplexKind = com.DishKindId,
                                                     ComplexKindName = kind.Name
                                                 }
                                                               )
                                   }
                       }


            };
            //var res = from u in _context.Users.Where(u => u.UserGroupId == groupid)
            //          join g in _context.UserGroups on u.UserGroupId equals g.Id
            //          //join udd in _context.UserDayDish.Where(ud => ud.Date >= datefrom && ud.Date <= dateto && ud.CompanyId == companyid) on u.Id equals udd.UserId
            //          select new UserOneDayOrderViewModel()
            //          {
            //              UserId = u.Id,
            //              ChildNameSurname = u.ChildNameSurname,
            //              GroupId = u.UserGroupId,
            //              GroupName = g.Name,
            //              Dishes = (from udd in _context.UserDayDish.Where(ud => ud.Date >= datefrom && ud.Date <= dateto && ud.CompanyId == companyid && ud.UserId==u.Id) 
            //                        join d in _context.Dishes on udd.DishId equals d.Id
            //                        join com in _context.Complex on udd.ComplexId equals com.Id
            //                        select new UserDayReportDishesViewModel()
            //                        {
            //                            DayDate = udd.Date,
            //                            DishId = udd.DishId,
            //                            DishName = d.Name,
            //                            ComplexName = com.Name,
            //                            ComplexKind = com.DishKindId
            //                        }
            //                        )
            //          };

            return res;
        }
        public async Task<string> OrderPeriodDetailReportAsync(DateTime? dateFrom, DateTime? dateTo, int? companyId)
        {

            if (!companyId.HasValue)
                companyId = (await _cache.GetCachedCompaniesAsync(_context)).OrderBy(c => c.IsDefault).LastOrDefault().Id;
            if (!dateFrom.HasValue)
                dateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (!dateTo.HasValue)
                dateTo = dateFrom.Value.AddMonths(1);
            return await _context.Database.JsonWriter($"exec [OrderPeriodDetailReport] '{dateFrom.Value.ShortSqlDate()}','{dateTo.Value.ShortSqlDate()}', {companyId.Value}").ToStringAsync();
        }
        public async Task<IEnumerable<OrderDetailsViewModel>> GetOrderPeriodDetailReport(DateTime datefrom, DateTime dateto, int companyId,int?usersubGroupId)
        {
            if (!_context.IsHttpContext())
            {
                _context.SetCompanyID(companyId);
            }
            string subgroup = usersubGroupId.HasValue ? usersubGroupId.Value.ToString() : "NULL";
            var query = await _context.Database.SqlQuery<OrderDetailsViewModel>(
                $"exec OrderPeriodDetailReport '{datefrom.ShortSqlDate()}' ,'{dateto.ShortSqlDate()}' , {companyId},{subgroup}").ToListAsync();
            return query;
        }
        public async Task<IEnumerable<GroupResult<OrderDetailsViewModel>>> GetOrderPeriodDetailReportWithGroup(DateTime datefrom, DateTime dateto, int companyId, int? usersubGroupId)
        {
            var query = await GetOrderPeriodDetailReport(datefrom, dateto, companyId, usersubGroupId);
            /*  var querywithgroup=from orders in query
                                 group orders by orders.GroupName into subgroup
                                 from userGroup in
                                      (from user in subgroup
                                       group user by user.ChildNameSurname)
                                 group userGroup by subgroup.Key;
            */
            var querywithgroup = query.GroupByMany(o => o.Date, o => o.GroupName, o => o.ChildNameSurname, o => o.Category);
            return querywithgroup;
        }
        public async Task<IEnumerable<UserFinanceReportViewModel>> GetUserFinancePeriodReport(DateTime datefrom, DateTime dateto, int companyId, int? usersubGroupId)
        {
            if (!_context.IsHttpContext())
            {
                _context.SetCompanyID(companyId);
            }
            string subgroup = usersubGroupId.HasValue ? usersubGroupId.Value.ToString() : "NULL";
            var query = await _context.Database.SqlQuery<UserFinanceReportViewModel>(
                $"exec UserFinancePeriodReport '{datefrom.ShortSqlDate()}' ,'{dateto.ShortSqlDate()}' , {companyId},{subgroup}").ToListAsync();
            
            return query;
        }
        public async Task<IEnumerable<GroupResult<UserFinanceReportViewModel>>> GetUserFinancePeriodReportWithGroup(DateTime datefrom, DateTime dateto, int companyId, int? usersubGroupId)
        {
            var query = await GetUserFinancePeriodReport(datefrom, dateto, companyId, usersubGroupId);
            /*  var querywithgroup=from orders in query
                                 group orders by orders.GroupName into subgroup
                                 from userGroup in
                                      (from user in subgroup
                                       group user by user.ChildNameSurname)
                                 group userGroup by subgroup.Key;
            */
            var querywithgroup = query.GroupByMany(o => o.Date, o => o.GroupName);
            return querywithgroup;
        }
    }
}
