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

namespace CateringPro.Test.Controllers
{

    public static class BuilderContextExtension
    {
   
        public static TBuilder WithContext<TBuilder>(this IBaseTestBuilderWithComponentBuilder<TBuilder> builder ) where TBuilder : IBaseTestBuilder
        {
            var actualBuilder = (BaseTestBuilderWithComponentBuilder<TBuilder>)builder;

           // actualBuilder.TestContext.HttpContext = httpContext;

            return actualBuilder.Builder;
        }

    }
    public class GenericControllerTest<TModel,TController> where TModel : CompanyDataOwnId, new() 
        where TController: GeneralController<TModel>
    {

        [Fact]
        public void ListItemsShouldGeneratePartialView()
        {
            MyMvc
                .Controller<TController>().WithContext()
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
                .WithData(db => db
                    .WithEntities(entities => CreateTestModels(
                        number: 10,
                        dbContext: entities)))
                .Calling(c => c.Search("",true))
                .ShouldReturn()
                .Ok(obj => obj
                    .WithModelOfType<IQueryable>()
                    .Passing(m => m.Count()==10));
        }
        [Fact]
        public void EditModalShouldReturnObjectResult()
        {

            MyMvc
             .Controller<TController>()
             .WithData(db => db
                 .WithEntities(entities => CreateTestModels(
                     number: 10,
                     dbContext: entities)))
             .Calling(c => c.EditModal(1, new TModel() { Id = 1 }))
             .ShouldReturn()
             .ActionResult(obj =>
                obj.Json()
             );
                 
        }
        protected  virtual TModel[] CreateTestModels(int number, DbContext dbContext)
        {
            int companyId = TestStartup.CommanyId;
            var models = Enumerable.Range(1, number).Select(n =>
                  new TModel
                  {
                      Id = n,
                      CompanyId= companyId
                  }).ToList();


            dbContext.Add(new Company() { Id = companyId });
            dbContext.AddRange(models);
            dbContext.SaveChanges();

            return models.ToArray();
        }
    }
}
