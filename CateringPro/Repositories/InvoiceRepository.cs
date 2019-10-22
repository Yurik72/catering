using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CateringPro.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        public InvoiceRepository(AppDbContext context,  ILogger<CompanyUser> logger)
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
                    Name= company.Name,
                    Phone = company.Phone,
                    ZipCode = company.ZipCode,
                    Email = company.Email,
                    
                    City = company.City,
                    Address1 = company.Address1,
                    Address2 = company.Address2,
                    Country = company.Country,
                    PictureId= company.PictureId,
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
                var query1 =
                         from dd in _context.DayDish.Where(dd => dd.CompanyId == companyid && dd.Date == daydate)
                         join d in _context.Dishes.Where(dd => dd.CompanyId == companyid) on dd.DishId equals d.Id
                         join ud in _context.UserDayDish.Where(ud => ud.CompanyId == companyid && ud.Date == daydate) on dd.DishId equals ud.DishId
                         join cu in _context.Users on ud.UserId equals cu.Id
                         select new InvoiceItemModel
                         {
                             Code=d.Code,
                             Name = d.Name,
                             Quantity = ud.Quantity,
                             Price = ud.Price,
                             Amount = ud.Quantity * d.Price
                         };
                res.Items = query1.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CustomerInvoice User={0} ", UserId);
                return res; //to do
            }
            return res;
        }
        public IEnumerable<CustomerOrdersViewModel> CustomerOrders( DateTime daydate, int companyid)
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
                               Price = ud.Price,
                               Amount = ud.Quantity * d.Price
                           };
            var querysingle = query1.FirstOrDefault();
            var queryres = from ud in query1.ToList()
                           group ud by ud.UserId into grp
                           select new CustomerOrdersViewModel()
                           {
                               Date= daydate,
                               Amount= grp.Sum(it=>it.Amount),
                               DishesCount = grp.Sum(it => it.Quantity),
                               Details = grp,
                               UserId = grp.Key,
                               User= GetUserCompany(grp.Key)
        };
                         /*
            var res = new CustomerOrdersViewModel()
            {

                Details = query1
            };
            if (querysingle != null)
            {
                res.UserId = querysingle.UserId;
                res.UserName = querysingle.UserName;
                res.Date = querysingle.Date;
                res.User = GetUserCompany(res.UserId);
            }


            return res;
            */
            return queryres;
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
    }
}
