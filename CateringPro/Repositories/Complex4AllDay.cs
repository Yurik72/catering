using CateringPro.Data;
using CateringPro.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{

    public interface IPromotion
    {
        string Name { get; }
        decimal DiscountValue { get; }

        int DiscountType { get; }

        IEnumerable<int> CategoriesId { get; }

    }


    public class Promotion:IPromotion
    {

        public string Name { get; set; }
        public decimal DiscountValue { get; set; }
        public int DiscountType { get; set; }

        public IEnumerable<int> CategoriesId { get; set; }
    }
    public class Complex4AllDay: IDiscountPlugin
    {
        private AppDbContext _context;
        private IEnumerable<IPromotion> promos;
        public Complex4AllDay(/*AppDbContext context*/)
        {
            //_context = context;
            // config = Configuration.GetSection("Complex4AllDay").Get<Complex4AllDayConfig>();
        }
        public void LoadConfig(IConfiguration config)
        {
            promos = config.GetSection("Complex4AllDay:Promotions").Get<List<Promotion>>();
        }
        public void SetContext(AppDbContext context)
        {
            _context = context;
        }
        public void CalculateComplexDayDiscount(List<UserDayComplex> daycomplex, List<UserDayDish> userDayDishes)
        {

            var triggered = promos.Where(p => !p.CategoriesId.Except(daycomplex.Select(dc => dc.Complex.CategoriesId)).Any()).ToList();
            if (triggered.Count() == 0)
                return;
           var discount_amont= triggered.Select(t => new { DiscountAmount = t.DiscountType == 1 ?/*absolute*/t.DiscountValue : /*percent*/daycomplex.Sum(dc => dc.Price) * t.DiscountValue / 100 }).Max(t=>t.DiscountAmount);
            // now find max discount and calculate
        }
        public decimal GetComplexDayDiscount(List<UserDayComplex> daycomplex)
        {
            //
            var triggered = promos.Where(p => !p.CategoriesId.Except(daycomplex.Select(dc => dc.Complex.CategoriesId)).Any()).ToList();
           // var db = _context.Discounts.ToList();
           // db.ForEach(d =>
           //{
           //    d.DiscountJson = JsonSerializer.Deserialize<DiscountJson>(d.Categories);
           //});
           // var triggered = db.Where(p => !p.DiscountJson.CategoriesId.Except(daycomplex.Select(dc => dc.Complex.CategoriesId)).Any()).ToList();

            if (triggered.Count() == 0)
                return 0;

            var discount_amont = triggered.Select(t => new { DiscountAmount = t.DiscountType == 1 ?/*absolute*/t.DiscountValue : /*percent*/daycomplex.Sum(dc => dc.Complex.Price) * t.DiscountValue / 100 }).Max(t => t.DiscountAmount);
           // var discount_amont = triggered.Select(t => new { DiscountAmount = t.Type == 1 ?/*absolute*/t.Value : /*percent*/daycomplex.Sum(dc => dc.Complex.Price) * t.Value / 100 }).Max(t => t.DiscountAmount);
           
            return discount_amont;
        }
        public decimal GetComplexDayDiscount(List<UserDayComplex> daycomplex, int companyId)
        {
            //
           //var triggered = promos.Where(p => !p.CategoriesId.Except(daycomplex.Select(dc => dc.Complex.CategoriesId)).Any()).ToList();
            if(daycomplex.Count == 0)
            {
                return 0;
            }
            DateTime date = daycomplex.FirstOrDefault().Date;
            var db = _context.Discounts.WhereCompany(companyId)/*.Where(d =>  d.DateFrom >= date && date <= d.DateTo)*/.ToList();
            db = db.Where(d => (d.DateFrom != null &&  date >= d.DateFrom ) || d.DateFrom == null).ToList();
            db = db.Where(d => (d.DateTo != null && date <= d.DateTo) || d.DateTo == null).ToList();
            db.ForEach(d =>
            {
                d.DiscountJson = JsonSerializer.Deserialize<DiscountJson>(d.Categories);
            });
            var triggered = db.Where(p => !p.DiscountJson.CategoriesId.Except(daycomplex.Select(dc => dc.Complex.CategoriesId)).Any()).ToList();

            if (triggered.Count() == 0)
                return 0;

            //var discount_amont = triggered.Select(t => new { DiscountAmount = t.DiscountType == 1 ?/*absolute*/t.DiscountValue : /*percent*/daycomplex.Sum(dc => dc.Complex.Price) * t.DiscountValue / 100 }).Max(t => t.DiscountAmount);
            var discount_amont = triggered.Select(t => new { DiscountAmount = t.Type == 1 ?/*absolute*/t.Value : /*percent*/daycomplex.Sum(dc => dc.Complex.Price) * t.Value / 100 }).Max(t => t.DiscountAmount);

            return discount_amont;
        }

    }
}
