using CateringPro.Data;
using CateringPro.Migrations;
using CateringPro.Models;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task<List<DeliveryDishViewModel>> GetDishesToDeliveryAsync(string userId, DateTime dayDate, bool includeDelievered = false)
        {
            return await (from ud in _context.UserDayDish.Where(ud => ud.UserId == userId && ud.Date == dayDate && (ud.IsDelivered || includeDelievered))
                          join  d in _context.Dishes on ud.DishId equals d.Id
                         
                          select new DeliveryDishViewModel() { ID = d.Id, Name = d.Name, IsComplex = ud.IsComplex, ComplexId = (ud.ComplexId.HasValue ? ud.ComplexId.Value : -1) })
                       .ToListAsync();
        }
        public async Task<List<DeliveryQueue>> GetQueueToDeliveryAsync(string userId, DateTime dayDate)
        {
            return await _context.DeliveryQueues.Where(q => q.UserId == userId && q.DayDate == dayDate).ToListAsync();
        }
        public async Task<ServiceResponse> ProcessRequestAsync(ServiceRequest request)
        {
            var res = await PrevalidateRequestAsync(request);
            if (!res.IsSuccess())
                return res;
            switch (request.Type)
            {
                case "askfordelivery":
                    return await ProcessDeliveryRequestAsync(request);
                case "askforregister":
                    return await ProcessRegisterRequestAsync(request);
                case "askforconfirm":
                    return await ProcessConfirmRequestAsync(request);
                case "askforqueue":
                    return await ProcessQueueRequestAsync(request);
                    
                default:
                    break;
            }
            return  ServiceResponse.GetFailResult();
        }

        public async Task<ServiceResponse> PrevalidateRequestAsync(ServiceRequest request)
        {
            var fail = ServiceResponse.GetFailResult();
            var user = await _userManager.FindByIdAsync(request.UserId);


            if (user == null && request.IsRequiredUser())
            {
                fail.ErrorMessage = "User Not Found";
                return fail;
            }
            //TODO
            if (user != null)
            {
                request.CompanyId = user.CompanyId;
                request.User = user;
            }
            else
                request.CompanyId = _context.CompanyId;
            if (_context.CompanyId <= 0)
                _context.SetCompanyID(request.CompanyId);
            return ServiceResponse.GetSuccessResult();
        }
        public async Task<ServiceResponse> ProcessConfirmRequestAsync(ServiceRequest request)
        {
            var queue = await GetQueueToDeliveryAsync(request.UserId, request.DayDate);
            var dishes = await GetDishesToDeliveryAsync(request.UserId, request.DayDate);
            var confirmed_queue = queue.Where(q => request.DishesIds.Contains(q.DishId)).ToList();
            //var confirmed_dishes = queue.Where(d => request.DishesIds.Contains(d.DishId)).ToList();
            var  user_daydishes = await _context.UserDayDish.Where(ud => ud.UserId == request.UserId && ud.Date == request.DayDate && !ud.IsDelivered).ToListAsync();
            var user_daydishes_confirmed= user_daydishes.Where(q => request.DishesIds.Contains(q.DishId)).ToList();
            try
            {
                user_daydishes_confirmed.ForEach(d => d.IsDelivered = true);
                _context.UpdateRange(user_daydishes_confirmed);
                _context.RemoveRange(confirmed_queue);
                await _context.SaveChangesAsync();
                return ServiceResponse.GetSuccessResult();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error save confirmation", ex);
            }
            return ServiceResponse.GetFailResult();
        }
        public async Task<ServiceResponse> ProcessQueueRequestAsync(ServiceRequest request)
        {
            /* not working on EF core
            var query=from q in _context.DeliveryQueues.Where(q=>q.DayDate==request.DayDate)
                      from ud in _context.UserDayDish.Where(ud =>  ud.Date == request.DayDate)
                      from d in _context.Dishes
                      from u in _context.CompanyUser
                      where (ud.DishId==q.DishId && d.Id==ud.DishId && u.Id==q.UserId)
                      group new {userid=u.Id, dishid = d.Id, dishname = d.Name }  by u.Id into grp
                      select new ServiceResponse()
                      { UserId=grp.Key,
                          Dishes =from it in grp 
                                   select new DeliveryDishViewModel() { 
                                   ID=it.dishid,
                                   Name=it.dishname
                                   }
                                  
                      };
            */
            try
            {
                var query = (from q in _context.DeliveryQueues.Where(q => q.DayDate == request.DayDate)
                             join ud in _context.UserDayDish.Where(ud => ud.Date == request.DayDate) on q.DishId equals ud.DishId
                             join d in _context.Dishes on q.DishId equals d.Id
                             join u in _context.CompanyUser on q.UserId equals u.Id
                                    
                                   // select new { userid = u.Id, dishid = d.Id, dishname = d.Name }
                                   select new DeliveryQueueForGroup()
                                   {
                                       QueueId=q.Id,
                                       UserId = u.Id,
                                       UserName=u.ChildNameSurname,
                                       DishId=d.Id,
                                       DishName=d.Name
                                   }
                                   ).ToList();
                
                var client_query = from q in query
                                   group q by new { q.UserId, q.UserName } into grp
                                   select new ServiceResponse()
                                   {
                                       UserId = grp.Key.UserId,
                                       UserName= grp.Key.UserName,
                                       Dishes = from it in grp
                                                select new DeliveryDishViewModel()
                                                {
                                                    ID = it.DishId,
                                                    QueueId=it.QueueId,
                                                    Name = it.DishName
                                                }

                                   };
                
                var resp = ServiceQueueResponse.GetSuccessResult();
                resp.Queues = client_query.ToList();
                return resp;
            }
            catch(Exception ex)
            {
                return ServiceQueueResponse.GetFailResult();
            }
        }
        public async Task<ServiceResponse> ProcessRegisterRequestAsync(ServiceRequest request)
        {
            var fail = ServiceResponse.GetFailResult();

          
           var dishes =await GetDishesToDeliveryAsync(request.UserId, request.DayDate, false);
           
            if (dishes.Count() == 0)
            {
                fail.ErrorMessage = "No dishes to delivery";
                return fail;
            }
            var queue = await _context.DeliveryQueues.Where(dq => dq.UserId == request.UserId && dq.DayDate == request.DayDate).ToListAsync();
            var queue_to_add = dishes.Where(d => !queue.Any(q => q.DishId == d.ID)).
                Select((q,idx)=>new DeliveryQueue() { UserId= request.UserId,DishId=q.ID,DayDate=request.DayDate, CompanyId=request.CompanyId, DishCourse=q.DishNumber,TerminalId=request.TerminalId});
            //var queue= await _context.DeliveryQueues.FirstOrDefaultAsync(ud => ud.UserId == request.UserId && ud.DayDate == request.DayDate);
            try
            {
                await _context.AddRangeAsync(queue_to_add);
                await _context.SaveChangesAsync();
                var resp = ServiceResponse.GetSuccessResult();
                resp.Dishes = dishes;
                return resp;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding delivery Queue", ex);
                fail.ErrorMessage = "Internal Error";
                return fail;
            }
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

            var dishes = await GetDishesToDeliveryAsync(request.UserId, request.DayDate, false);
            if (dishes.Any(d => d.IsComplex))
            {
                var udaycomplex = await (from uc in _context.UserDayComplex.Where(ud => ud.UserId == request.UserId && ud.Date == request.DayDate)
                                         join  c in _context.Complex on uc.ComplexId equals c.Id
                                         join dc in _context.DishComplex on uc.ComplexId equals dc.ComplexId
                                        
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
