using System;
using System.Collections.Generic;
using System.Linq;
using CateringPro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CateringPro.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context, IServiceProvider service)
        {
            context.Database.EnsureCreated();

            var roleManager = service.GetRequiredService<RoleManager<CompanyRole>>();
            var userManager = service.GetRequiredService<UserManager<CompanyUser>>();


            CreateAdminRole(context, roleManager, userManager);
            CreateRole("CompanyAdmin", context, roleManager);
            CreateRole("GroupAdmin", context, roleManager);
            CreateRole("KitchenAdmin", context, roleManager);
            CreateRole("UserAdmin", context, roleManager);

            if (context.Dishes.IgnoreQueryFilters().Any())
            {
                return;
            }

            ClearDatabase(context);
            CreateAdminRole(context, roleManager, userManager);
            CreateRole("CompanyAdmin", context, roleManager);
            CreateRole("GroupAdmin", context, roleManager);
            CreateRole("KitchenAdmin", context, roleManager);
            CreateRole("UserAdmin", context, roleManager);

            SeedDatabase(context, roleManager, userManager);
        }
        private static void CreateRole(string name,AppDbContext context, RoleManager<CompanyRole> _roleManager)
        {
            if (_roleManager.RoleExistsAsync(name).Result)
                return;
            var role = new CompanyRole()
            {
                Name = name
            };
            _roleManager.CreateAsync(role).Wait();
        }
        private static void CreateAdminRole(AppDbContext context, RoleManager<CompanyRole> _roleManager, UserManager<CompanyUser> _userManager)
        {
            bool roleExists = _roleManager.RoleExistsAsync("Admin").Result;
            if (roleExists)
            {
                return;
            }
            
            var role = new CompanyRole()
            {
                Name = "Admin"
            };
            _roleManager.CreateAsync(role).Wait();

            var user = new CompanyUser()
            {
                UserName = "admin",
                Email = "admin@default.com",
                CompanyId=1
                
            };

            string adminPassword = "Password123";
            var userResult =  _userManager.CreateAsync(user, adminPassword).Result;

            if (userResult.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "Admin").Wait();
            }
        }

        private static void SeedDatabase(AppDbContext _context, RoleManager<CompanyRole> _roleManager, UserManager<CompanyUser> _userManager)
        {
            var cat1 = new Categories { Code = "", Name = "Standard", Description = "The Bakery's Standard pizzas all year around.", CompanyId = 1 };
            var cat2 = new Categories { Code = "", Name = "Spcialities", Description = "The Bakery's Speciality pizzas only for a limited time.", CompanyId = 1 };
            var cat3 = new Categories { Code = "", Name = "News", Description = "The Bakery's New pizzas on the menu.", CompanyId = 1 };
            var cat4 = new Categories { Code = "1", Name = "Завтраки", Description = "The Bakery's New pizzas on the menu.", CompanyId = 1 };
            var cat5 = new Categories { Code = "2", Name = "Обед", Description = "The Bakery's New pizzas on the menu.", CompanyId = 1 };

            var usrGr1 = new UserGroups { Code = "", Name = "Group1", Description = "Test group descr", CompanyId = 1 };
            var usrGr2 = new UserGroups { Code = "", Name = "Group2", Description = "Group test", CompanyId = 1 };
            var usrGr3 = new UserGroups { Code = "", Name = "Group3", Description = "The Bakery's New pizzas on the menu.", CompanyId = 1 };
            var usrGr4 = new UserGroups { Code = "1", Name = "Завтраки", Description = "The Bakery's New pizzas on the menu.", CompanyId = 1 };
            var usrGr5 = new UserGroups { Code = "2", Name = "Обед", Description = "The Bakery's New pizzas on the menu.", CompanyId = 1 };


            var comp1 = new Company { Code = "BASE", Name = "Default" };
            var comp2 = new Company { Code = "CABACHOK", Name = "Кабачок" };
            var comps = new List<Company>() { comp1, comp2 };
            var cats = new List<Categories>()
            {
                cat1, cat2, cat3,cat4,cat5
            };

            var usrs = new List<UserGroups>()
            {
                usrGr1, usrGr2, usrGr3,usrGr4,usrGr5
            };


            var d1=new Dish { Code = "1", Name = "Борщ", Price = 2,  Description = "...", CategoriesId=4,CompanyId=1 };
            var d2 = new Dish { Code = "2", Name = "Котлета", Price = 2, Description = "A normal pizza with a taste from the forest.", CategoriesId = 4, CompanyId = 1 };
            var d3 = new Dish { Code = "3", Name = "Запеканка", Price = 2, Description = "A normal pizza with a taste from the forest.", CategoriesId = 3, CompanyId = 1 };
            var d4 = new Dish { Code = "4", Name = "Омлет", Price = 2, Description = "A normal pizza with a taste from the forest.", CategoriesId = 3, CompanyId = 1 };


            var dishes = new List<Dish>()
            {
                d1, d2, d3,d4
            };
            var dc1 = new DishCategory { CategoryId=4,DishId=3};
            var dc2 = new DishCategory { CategoryId = 5, DishId = 3 };
            var dc3 = new DishCategory { CategoryId = 5, DishId = 1 };
            var dishcaetgories = new List<DishCategory>()
            {
                dc1,dc2,dc3

            };


            var user1 = new CompanyUser { UserName = "user1@gmail.com", Email = "user1@gmail.com" };
            var user2 = new CompanyUser { UserName = "user2@gmail.com", Email = "user2@gmail.com" };
            var user3 = new CompanyUser { UserName = "user3@gmail.com", Email = "user3@gmail.com" };
            var user4 = new CompanyUser { UserName = "user4@gmail.com", Email = "user4@gmail.com" };
            var user5 = new CompanyUser { UserName = "user5@gmail.com", Email = "user5@gmail.com" };

            string userPassword = "Password123";

            var users = new List<CompanyUser>()
            {
                user1, user2, user3, user4, user5
            };

            foreach (var user in users)
            {
                _userManager.CreateAsync(user, userPassword).Wait();
            }



            var ing1 = new Ingredients { Name = "Cheese" };
            var ing2 = new Ingredients { Name = "Flour" };
            var ing3 = new Ingredients { Name = "Tomatoe sauce" };
            var ing4 = new Ingredients { Name = "Lettuce" };
            var ing5 = new Ingredients { Name = "Mushrooms" };
            var ing6 = new Ingredients { Name = "Kebab" };
            var ing7 = new Ingredients { Name = "Shrimp" };
            var ing8 = new Ingredients { Name = "Pineapple" };
            var ing9 = new Ingredients { Name = "Ham" };
            var ing10 = new Ingredients { Name = "Broccoli" };
            var ing11 = new Ingredients { Name = "Onions" };
            var ing12 = new Ingredients { Name = "Olives" };
            var ing13 = new Ingredients { Name = "Bananas" };
            var ing14 = new Ingredients { Name = "Chicken" };
            var ing15 = new Ingredients { Name = "Tomatoes" };
            var ing16 = new Ingredients { Name = "Minced Meat" };

            var ings = new List<Ingredients>()
            {
                ing1, ing2, ing3, ing4, ing5, ing6, ing7, ing8, ing9, ing10, ing11, ing12, ing13, ing14, ing15, ing16
            };



            
            _context.Companies.AddRange(comps);
            _context.SaveChanges();
            _context.Categories.AddRange(cats);
            _context.UserGroups.AddRange(usrs);


            _context.Ingredients.AddRange(ings);
           // _context.PizzaIngredients.AddRange(pizIngs);

            _context.Dishes.AddRange(dishes);
           // _context.DishCategory.AddRange(dishcaetgories);
            

            _context.SaveChanges();
        }

        private static void ClearDatabase(AppDbContext _context)
        {
            

            var users = _context.Users.ToList();
            var userRoles = _context.UserRoles.ToList();

            foreach (var user in users)
            {
                if (!userRoles.Any(r => r.UserId == user.Id))
                {
                    _context.Users.Remove(user);
                }
            }



            var categories = _context.Categories.ToList();
            _context.Categories.RemoveRange(categories);

            _context.SaveChanges();

            //var groupusers = _context.UserGroups.ToList();
            //_context.UserGroups.RemoveRange(groupusers);

            //_context.SaveChanges();
        }
    }
}
