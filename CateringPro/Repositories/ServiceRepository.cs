using CateringPro.Data;
using CateringPro.Models;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public class ServiceRepository: IServiceRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        private readonly UserManager<CompanyUser> _userManager;
        private readonly IMemoryCache _cache;
        public ServiceRepository(AppDbContext context, ILogger<CompanyUser> logger,
            UserManager<CompanyUser> userManager, IMemoryCache cache)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _cache = cache;
        }

        public async Task<ServiceResponse> ProcessRequestAsync(ServiceRequest request)
        {
            switch (request.Type)
            {
                case "askfordelivery":
                    return await ProcessDeliveryRequestAsync(request);
                    
                default:
                    break;
            }
            return  ServiceResponse.GetFailResult();
        }
        public async Task<ServiceResponse> ProcessDeliveryRequestAsync(ServiceRequest request)
        {
            var uday =await  _context.UserDay.FirstOrDefaultAsync(ud => ud.UserId == request.UserId && ud.Date == request.DayDate);
            if(uday==null || !uday.IsConfirmed)
            {
                var fail = ServiceResponse.GetFailResult();
                fail.ErrorMessage="No confirmed orders on this day";
                return fail;
            }

            var dishes = await (from ud in _context.UserDayDish.Where(ud => ud.UserId == request.UserId && ud.Date == request.DayDate)
                                from d in _context.Dishes
                                where (ud.DishId == d.Id)
                                select new DeliveryDishViewModel() { ID = d.Id, Name = d.Name, IsComplex = ud.IsComplex ,ComplexId=(ud.ComplexId.HasValue? ud.ComplexId.Value:-1)})
                        .ToListAsync();
            if (dishes.Any(d => d.IsComplex))
            {
                var udaycomplex = await (from uc in _context.UserDayComplex.Where(ud => ud.UserId == request.UserId && ud.Date == request.DayDate)
                                         from c in _context.Complex
                                         from dc in _context.DishComplex
                                         where uc.ComplexId==c.Id && dc.ComplexId==c.Id
                                         select new { ComplexId=c.Id,DishId=dc.DishId,ComplexName=c.Name,Number=dc.DishCourse}
                                         )
                                        .ToListAsync();
                var dishesid_todelivery = udaycomplex.Where(it => request.DishesNum.Contains(it.Number+1));
                dishes = dishes.Where(d => dishesid_todelivery.Any(v => v.DishId == d.ID)).ToList();
            }
            var response = ServiceResponse.GetSuccessResult();
            response.Dishes = dishes;
            return response;
        }
    }
}
