namespace CateringPro.Test.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using CateringPro.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;
    using CateringPro.ViewModels;

    public class AddressControllerTest: GenericControllerTest<Address,AddressController>
    {

        /*
        [Fact]
        public void ListShouldGeneratePartialView()
        {
            MyMvc
                .Controller<AddressController>()
                .WithData(db => db
                    .WithEntities(entities => CreateTestAdresses(
                        number: 10,
                        dbContext: entities)))
                .Calling(c => c.ListItems(new QueryModel()))
                .ShouldReturn()
                .PartialView(view => view
                    .WithModelOfType<List<Address>>()
                    .Passing(m => m.Count == 10));
        }
        */
        /*
        [Fact]
        public void BrowseShouldReturnNotFoundWithNoGenreData()
        {
            MyMvc
                .Controller<AddressController>()
                .Calling(c => c.Browse(string.Empty))
                .ShouldReturn()
                .NotFound();
        }

        [Fact]
        public void BrowseShouldReturnViewGenre()
        {
            var genre = "Genre 1";

            MyMvc
                .Controller<AddressController>()
                .WithData(db => db
                    .WithEntities(entities => CreateTestGenres(
                        numberOfGenres: 3,
                        numberOfAlbums: 3,
                        dbContext: entities)))
                .Calling(c => c.Browse(genre))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<Genre>()
                    .Passing(model =>
                    {
                        Assert.Equal(genre, model.Name);
                        Assert.NotNull(model.Albums);
                        Assert.Equal(3, model.Albums.Count);
                    }));
        }

        [Fact]
        public void DetailsShouldReturnNotFoundWithNoData()
        {
            MyMvc
                .Controller<StoreController>()
                .Calling(c => c.Details(From.Services<IMemoryCache>(), int.MinValue))
                .ShouldReturn()
                .NotFound();
        }

        [Fact]
        public void DetailsShouldReturnAlbumDetails()
        {
            Genre[] genres = null;
            var albumId = 1;

            MyMvc
                .Controller<StoreController>()
                .WithOptions(options => options
                    .For<AppSettings>(settings => settings.CacheDbResults = true))
                .WithData(db => db
                    .WithEntities(entities => genres = CreateTestGenres(
                        numberOfGenres: 3,
                        numberOfAlbums: 3,
                        dbContext: entities)))
                .Calling(c => c.Details(From.Services<IMemoryCache>(), 1))
                .ShouldHave()
                .MemoryCache(cache => cache
                    .ContainingEntry(entry => entry
                        .WithKey("album_1")
                        .WithSlidingExpiration(TimeSpan.FromMinutes(10))
                        .WithValueOfType<Album>()
                        .Passing(a => a.AlbumId == 1)))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<Album>()
                    .Passing(model =>
                    {
                        Assert.NotNull(model.Genre);
                        var genre = genres.SingleOrDefault(g => g.GenreId == model.GenreId);
                        Assert.NotNull(genre);
                        Assert.NotNull(genre.Albums.SingleOrDefault(a => a.AlbumId == albumId));
                        Assert.NotNull(model.Artist);
                    }));
        }
        */
        /*
        private static Address[] CreateTestAdresses(int number, DbContext  dbContext)
        {
            var adresses = Enumerable.Range(1, number).Select(n =>
                  new Address
                  {
                      Id = n,

                  }).ToList();



            dbContext.AddRange(adresses);
            dbContext.SaveChanges();

            return adresses.ToArray();
        }
        */
        
    }
}