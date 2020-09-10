﻿using CateringPro.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
        private IEnumerable<IPromotion> promos;
        public Complex4AllDay()
        {
           // config = Configuration.GetSection("Complex4AllDay").Get<Complex4AllDayConfig>();
        }
        public void LoadConfig(IConfiguration config)
        {
            promos = config.GetSection("Complex4AllDay:Promotions").Get<List<Promotion>>();
        }
        public void CalculateComplexDayDiscount(List<UserDayComplex> daycomplex, List<UserDayDish> userDayDishes)
        {

            var triggered = promos.Where(p => p.CategoriesId.Except(daycomplex.Select(dc => dc.Complex.CategoriesId)).Count() == 0).ToList();
            if (triggered.Count() == 0)
                return;
           var discount_amont= triggered.Select(t => new { DiscountAmount = t.DiscountType == 1 ?/*absolute*/t.DiscountValue : /*percent*/daycomplex.Sum(dc => dc.Price) * t.DiscountValue / 100 }).Max(t=>t.DiscountAmount);
            // now find max discount and calculate
        }
    }
}
