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

namespace CateringPro.Test.Controllers
{

    public static class BuilderContextExtension
    {
   
        public static TBuilder WithContext<TBuilder>(this IBaseTestBuilderWithComponentBuilder<TBuilder> builder ) where TBuilder : IBaseTestBuilder
        {
            var actualBuilder = (BaseTestBuilderWithComponentBuilder<TBuilder>)builder;


           
            return actualBuilder.Builder;
        }

    }
    public abstract class GenericControllerTest<TModel, TController> where TModel : CompanyDataOwnId, new()
        where TController : GeneralController<TModel>
    {
        protected  List<TModel> models;
        public GenericControllerTest()
        {

        }
        [Fact]
        public void ListItemsShouldGeneratePartialView()
        {
            MyMvc
                .Controller<TController>()
                .WithUser(u=>u.WithClaims(UserContextEx.GetClaims()))
                .WithData(db => db
                    .WithEntities(entities => CreateTestModels(
                        number: 10,
                        dbContext: entities)))
                .Calling(c => c.ListItems(new QueryModel()))
                .ShouldReturn()
                .PartialView(view => view
                    .WithModelOfType<List<TModel>>()
                    .Passing(m => m.Count == 10));
        }

        [Fact]
        public void SearchShouldReturnObjectResult()
        {
            MyMvc
                .Controller<TController>()
                .WithUser(u => u.WithClaims(UserContextEx.GetClaims()))
                .WithData(db => db
                    .WithEntities(entities => CreateTestModels(
                        number: 10,
                        dbContext: entities)))
                .Calling(c => c.Search("",true))
                .ShouldReturn()
                .Ok(obj => 
                        obj.WithModelOfType<IQueryable>()
                        .Passing(m => m.Count()==10));
        }
        [Fact]
        public void EditModalShouldReturnObjectResult()
        {

            MyMvc
             .Controller<TController>()
              .WithUser(u => u.WithClaims(UserContextEx.GetClaims()))
             .WithData(db => db
                 .WithEntities(entities => CreateTestModels(
                     number: 10,
                     dbContext: entities)))
             .Calling(c =>   c.EditModal(5, models.First(e=>e.Id==5)))
             .ShouldReturn()
             .ActionResult(obj =>
                obj.Json(j=>j.WithModelOfType<JSONResultResponse>().Passing(m=>m.res=="OK"))
             );
                 
        }
        [Fact]
        public void EditModalShouldReturnNotFoundWithWrongObject()
        {
            TModel[] items = new TModel[0];
            MyMvc
             .Controller<TController>()
              .WithUser(u => u.WithClaims(UserContextEx.GetClaims()))
             .WithData(db => db
                 .WithEntities(entities => items= CreateTestModels(
                     number: 10,
                     dbContext: entities)))
             .Calling(c => c.EditModal(100, items.First()))
             .ShouldReturn()
             .ActionResult(obj =>
                obj.NotFound()
             );

        }
        protected  virtual TModel[] CreateTestModels(int number, DbContext dbContext)
        {
            int companyId = TestStartup.CompanyId;
            models = Enumerable.Range(1, number).Select(n =>
                  new TModel
                  {
                      Id = n,
                      CompanyId= companyId
                  }).ToList();


            dbContext.Add(new Company() { Id = companyId });
            dbContext.SaveChanges();
            OnEntitiesAdd(dbContext, models);
            dbContext.AddRange(models);
            dbContext.SaveChanges();
            
            return models.ToArray();
        }
        protected virtual void OnEntitiesAdd(DbContext cont, List<TModel> ent)
        {

        }
    }
}
