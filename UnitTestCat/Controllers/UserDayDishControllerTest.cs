using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using CateringPro.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;
using CateringPro.ViewModels;
using CateringPro.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using MyTested.AspNetCore.Mvc.Builders.Contracts.Base;
using MyTested.AspNetCore.Mvc.Builders.Base;
using System.Security.Claims;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using CateringPro.Data;
using CateringPro.Core;

namespace CateringPro.Test.Controllers
{
    public class UserDayDishControllerTest
    {
        //protected IEnumerable<UserDayComplex> _userdaycomplexes;
      //  protected List<UserDayComplex> _complextosave=new List<UserDayComplex>();
        public List<UserDayDish> _complextdishesosave = new List<UserDayDish>();

        [Fact]
        public void SaveSaveDayComplexShouldReturnOkResult()
        {
            List<UserDayComplex> tosave=new List<UserDayComplex>();
            List<UserDayDish> tosavedishes = new List<UserDayDish>();
            MyMvc
                .Controller<UserDayDishesController>()
                .WithUser(u => u.WithClaims(UserContextEx.GetClaims()))
                .WithData(db => db
                    .WithEntities(entities => tosave = CreateTestModels(
                        number: 10,
                        dbContext: entities, out tosavedishes).ToList()))
                .Calling(c =>
                //c.Index()
                c.SaveDayComplex(tosave, tosavedishes)
                )
                .ShouldReturn()
             .ActionResult(obj =>
                obj.Json(j => j.WithModelOfType<JSONResultResponse>().Passing(m => m.res == "OK")));


        }
        [Fact]
        public void SaveSaveDayComplexShouldReturnOutDateResult()
        {
            List<UserDayComplex> tosave = new List<UserDayComplex>();
            List<UserDayDish> tosavedishes = new List<UserDayDish>();
            MyMvc
                .Controller<UserDayDishesController>()
                .WithUser(u => u.WithClaims(UserContextEx.GetClaims()))
                .WithData(db => db
                    .WithEntities(entities => tosave = CreateTestModels(
                        number: 10,
                        dbContext: entities, out tosavedishes,DateTime.Now.AddDays(-1)).ToList()))
                .Calling(c =>
                //c.Index()
                c.SaveDayComplex(tosave, tosavedishes)
                )
                .ShouldReturn()
             .ActionResult(obj =>
                obj.Json(j => j.WithModelOfType<JSONResultResponse>().Passing(m => m.res == "FAIL" && m.reason=="OutDate")));


        }
        protected IEnumerable<UserDayComplex> CreateTestModels(int number, DbContext dbContext, DateTime? dayDate=default)
        {
            int companyId = TestStartup.CompanyId;
            string userid = TestStartup.UserId;
            if (!dayDate.HasValue)
                dayDate = DateTime.Now;
            DateTime day = dayDate.Value.ResetHMS().AddDays(+1);
            var dishes = Enumerable.Range(1, number).Select(n =>
                    new Dish
                    {
                        Id = n,
                        CompanyId = companyId
                    }).ToList();

            var complexes = Enumerable.Range(1, number).Select(n =>
                  new Complex
                  {
                      Id = n,
                      Price = n * 10,
                      CompanyId = companyId
                  }).ToList();

            var complex7 = complexes.First(c => c.Id == 7);
            var dish1 = dishes.First(c => c.Id == 1);
            var dish2 = dishes.First(c => c.Id == 2);
            var dish3 = dishes.First(c => c.Id == 3);
            var dish4 = dishes.First(c => c.Id == 4);
            var dishcomplexes = new List<DishComplex>();
            dishcomplexes.Add(new DishComplex() { CompanyId = companyId, DishId = dish1.Id, DishCourse = 0, ComplexId = complex7.Id });
            dishcomplexes.Add(new DishComplex() { CompanyId = companyId, DishId = dish2.Id, DishCourse = 0, ComplexId = complex7.Id });
            dishcomplexes.Add(new DishComplex() { CompanyId = companyId, DishId = dish3.Id, DishCourse = 1, ComplexId = complex7.Id });
            dishcomplexes.Add(new DishComplex() { CompanyId = companyId, DishId = dish4.Id, DishCourse = 1, ComplexId = complex7.Id });
            complex7.DishComplex = dishcomplexes;
            var userdaycomplexes = Enumerable.Range(1, 1).Select(n =>
                    new UserDayComplex
                    {
                        Price = complex7.Price,
                        Date = day,
                        UserId = userid,
                        CompanyId = companyId
                    }).ToList();
            _complextdishesosave.Add(new UserDayDish() { CompanyId = companyId, Date = day, DishId = dish1.Id, UserId = userid, Quantity = 1 });
            _complextdishesosave.Add(new UserDayDish() { CompanyId = companyId, Date = day, DishId = dish4.Id, UserId = userid, Quantity = 1 });

            dbContext.Add(new Company() { Id = companyId, OrderLeadTimeH=1,OrderThresholdTimeH=100 });
            dbContext.SaveChanges();

            dbContext.AddRange(dishes);
            dbContext.SaveChanges();

            dbContext.AddRange(complexes);
            dbContext.SaveChanges();

            //  _complextosave .Add(userdaycomplexes.First());
            return new UserDayComplex[] { userdaycomplexes.First() };
        }
        protected  IEnumerable<UserDayComplex> CreateTestModels(int number, DbContext dbContext,out List<UserDayDish> usedaydishes,DateTime? dayDate=default)
        {
            var res = CreateTestModels(number, dbContext, dayDate);
            int companyId = TestStartup.CompanyId;
            string userid = TestStartup.UserId;
            if (!dayDate.HasValue)
                dayDate = DateTime.Now;
            DateTime day = dayDate.Value.ResetHMS().AddDays(+1);
            var udd = new List<UserDayDish>();
            var dish1 = dbContext.Set<Dish>().First(c => c.Id == 1);
            var dish4 = dbContext.Set<Dish>().First(c => c.Id == 4);
            udd.Add(new UserDayDish() {CompanyId= companyId, Date = day, DishId = dish1.Id, UserId = userid, Quantity=1 });
            udd.Add(new UserDayDish() { CompanyId = companyId, Date = day, DishId = dish4.Id, UserId = userid, Quantity = 1 });

            usedaydishes = udd;

            //  _complextosave .Add(userdaycomplexes.First());
            return res;
        }
    }
}
