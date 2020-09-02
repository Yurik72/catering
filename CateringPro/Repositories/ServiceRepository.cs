using CateringPro.Core;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
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

        public Task<List<DeliveryDishViewModel>> GetDishesToDeliveryAsync(string userId, DateTime dayDate, bool includeDelievered = false,int[] сategoriesIds=default)
        {
            if (сategoriesIds == null)
                сategoriesIds = new int[0];
            return (from ud in _context.UserDayDish.Where(ud => ud.UserId == userId && ud.Date == dayDate && (!ud.IsDelivered || includeDelievered))
                          join  d in _context.Dishes on ud.DishId equals d.Id
                            join c in _context.Complex on ud.ComplexId equals c.Id
                            join dc in _context.DishComplex on new { DishId=d.Id, ComplexId=c.Id } equals new { DishId = dc.DishId, ComplexId=dc.ComplexId }
                    where (сategoriesIds.Length == 0 || сategoriesIds.Contains(c.CategoriesId))



                    select new DeliveryDishViewModel() { ID = d.Id, Name = d.Name, IsComplex = ud.IsComplex, DishNumber=dc.DishCourse, ComplexId = (ud.ComplexId.HasValue ? ud.ComplexId.Value : -1) })
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
                case "askforqueueconfirm":
                    return await ProcessQueueConfirmRequestAsync(request);
                case "askforqueueremove":
                    return await ProcessQueueRemoveRequestAsync(request);
                default:
                    break;
            }
            return  ServiceResponse.GetFailResult();
        }

        public async Task<ServiceResponse> PrevalidateRequestAsync(ServiceRequest request)
        {
            var fail = ServiceResponse.GetFailResult();
            //var user = await _userManager.FindByIdAsync(request.UserId);  //now by tag
            CompanyUser user = null;
            if (!string.IsNullOrEmpty(request.UserToken))
            {
                user=await _userManager.FindByCardTokenAsync(request.UserToken);  //now by tag
            }
            if (!string.IsNullOrEmpty(request.UserId) && user == null)
            {
                user = await _userManager.FindByIdAsync(request.UserId);  //now by tag
            }
            if (user == null && request.IsRequiredUser())
            {
                fail.ErrorMessage = "User Not Found";
                return fail;
            }
            //TODO
            if (_context.CompanyId <= 0 && user!=null)
                _context.SetCompanyID(user.CompanyId);
            request.CompanyId = _context.CompanyId;
            if (user != null)
            {
                //request.CompanyId = user.CompanyId;
                request.User = user;
                request.UserId = user.Id;


            }
           
            if (_context.CompanyId <= 0)
                _context.SetCompanyID(request.CompanyId);

            request.DayDate = request.DayDate.ResetHMS();
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
                _logger.LogError(ex,"Error save confirmation");
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
                var query = await (from q in _context.DeliveryQueues.Where(q => q.DayDate == request.DayDate.ResetHMS()
                                   && q.Id> request.LastQueueId)
                             join ud in _context.UserDayDish on new { q.DishId ,DayDate=q.DayDate,q.UserId } equals new { ud.DishId, DayDate=ud.Date, ud.UserId }
                             join d in _context.Dishes on q.DishId equals d.Id
                             join u in _context.CompanyUser on q.UserId equals u.Id
                             join c in _context.Complex on ud.ComplexId equals c.Id
                             where request.DishesNum.Contains(q.DishCourse)
                                   && ( request.ComplexCategoriesIds.Length==0 || request.ComplexCategoriesIds.Contains(c.CategoriesId))
                                   // select new { userid = u.Id, dishid = d.Id, dishname = d.Name }
                                   select new DeliveryQueueForGroup()
                                   {
                                       QueueId=q.Id,
                                       UserId = u.Id,
                                       UserName=u.GetChildUserName(),
                                       DishId=d.Id,
                                       DishName=d.Name,
                                       UserPictureId=u.PictureId,
                                       DishCource=q.DishCourse
                                   }
                                   ).Take(request.MaxQueue* request.DishesNum.Count())
                                   //.Any(value => request.DishesNum.Contains(value.DishCource)))
                                    .ToListAsync();
                
                var client_query = from q in query
                                   group q by new { q.UserId, q.UserName,q.UserPictureId } into grp
                                   select new ServiceResponse()
                                   {
                                       UserId = grp.Key.UserId,
                                       UserName= grp.Key.UserName,
                                       UserPictureId= grp.Key.UserPictureId,
                                       Dishes = from it in grp
                                                select new DeliveryDishViewModel()
                                                {
                                                    ID = it.DishId,
                                                    QueueId=it.QueueId,
                                                    Name = it.DishName
                                                }

                                   };
                
                var resp = ServiceQueueResponse.GetSuccessResult();
                resp.Queues = client_query.Take(request.MaxQueue).ToList();
                return resp;
            }
            catch(Exception ex)
            {
                return ServiceQueueResponse.GetFailResult();
            }
        }
        public async Task<ServiceResponse> ProcessQueueConfirmRequestAsync(ServiceRequest request)
        {
            // var res=ServiceResponse.GetSuccessResult(request);
            var queue = await _context.DeliveryQueues.Where(q => request.QueueIds.Contains(q.Id)).ToListAsync();
            var confirmed_dishesid = queue.Select(q => q.DishId);
            //var confirmed_dishes = queue.Where(d => request.DishesIds.Contains(d.DishId)).ToList();
            var user_daydishes = await _context.UserDayDish.Where(ud => ud.UserId == request.UserId && ud.Date == request.DayDate.ResetHMS() && ud.IsDelivered==false).ToListAsync();
            var user_daydishes_confirmed = user_daydishes.Where(q => confirmed_dishesid.Contains(q.DishId)).ToList();
            try
            {
                user_daydishes_confirmed.ForEach(d => d.IsDelivered = true);
                _context.UpdateRange(user_daydishes_confirmed);
                _context.RemoveRange(queue);
                await _context.SaveChangesAsync();
                return ServiceResponse.GetSuccessResult(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error save confirmation");
                return ServiceResponse.GetFailResult(request);
            }
           
            //return res;
        }
        public async Task<ServiceResponse> ProcessQueueRemoveRequestAsync(ServiceRequest request)
        {
            // var res=ServiceResponse.GetSuccessResult(request);
            var queue = await _context.DeliveryQueues.Where(q => request.QueueIds.Contains(q.Id)).ToListAsync();
           
            try
            {
              
                _context.RemoveRange(queue);
                await _context.SaveChangesAsync();
                return ServiceResponse.GetSuccessResult(request);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error remove queue", ex);
                return ServiceResponse.GetFailResult(request);
            }

            //return res;
        }
        public async Task<ServiceResponse> ProcessRegisterRequestAsync(ServiceRequest request)
        {
            var fail = ServiceResponse.GetFailResult(request);

          
           var dishes = await GetDishesToDeliveryAsync(request.UserId, request.DayDate, false,request.ComplexCategoriesIds);
           
            if (dishes.Count() == 0)
            {
                fail.ErrorMessage = " Нема страв до видачі";
                return fail;
            }
            var queue = await _context.DeliveryQueues.Where(dq => dq.UserId == request.UserId && dq.DayDate == request.DayDate.ResetHMS()).ToListAsync();
            var queue_to_add = dishes.Where(d => !queue.Any(q => q.DishId == d.ID)).
                Select((q,idx)=>new DeliveryQueue() { UserId= request.UserId,DishId=q.ID,DayDate=request.DayDate.ResetHMS(), CompanyId=request.CompanyId, DishCourse=q.DishNumber,TerminalId=request.TerminalId});
            //var queue= await _context.DeliveryQueues.FirstOrDefaultAsync(ud => ud.UserId == request.UserId && ud.DayDate == request.DayDate);
            try
            {
                if (queue_to_add.Count() > 0)
                {
                    await _context.AddRangeAsync(queue_to_add);
                    await _context.SaveChangesAsync();
                }
                var resp = ServiceResponse.GetSuccessResult(request);

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

            var dishes =await  GetDishesToDeliveryAsync(request.UserId, request.DayDate, false);
            if (dishes.Any(d => d.IsComplex))
            {
                var udaycomplex = await (from uc in _context.UserDayComplex.Where(ud => ud.UserId == request.UserId && ud.Date == request.DayDate.ResetHMS())
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
        public async Task<IEnumerable<UserCardViewModel>> GetUserCardsAsync(QueryModel queryModel)
        {
           return await _context.CompanyUser
                .Where(u=>
                        string.IsNullOrEmpty(queryModel.SearchCriteria)
                        ||
                        u.UserName.Contains(queryModel.SearchCriteria)
                        ||
                        u.ChildNameSurname.Contains(queryModel.SearchCriteria)
                        ||
                        u.Email.Contains(queryModel.SearchCriteria)
                        ||
                        u.NameSurname.Contains(queryModel.SearchCriteria)
                        )
                .Take(50)
                .Select(u => new UserCardViewModel()
               { 
                   UserId=u.Id,
                   UserName=u.NameSurname,
                   UserChildName=u.ChildNameSurname,
                   UserLogin=u.UserName,
                   UserEmail=u.Email,
                   CardToken=u.CardTag,
                   PictureId=u.PictureId
           
               }
               ).ToListAsync();
        }
        public async Task<UserCardViewModel> GetUserCardAsync(string cardToken)
        {
            if (string.IsNullOrEmpty(cardToken))
                return null;
            var user = await _userManager.FindByCardTokenAsync(cardToken);
            if (user == null)
                return null;
            var res = new UserCardViewModel()
            {
                UserId = user.Id,
                UserName = user.NameSurname,
                UserChildName = user.ChildNameSurname,
                UserLogin = user.UserName,
                UserEmail = user.Email,
                CardToken = user.CardTag,
                PictureId = user.PictureId

            };
            return res;

        }

        public async Task<IEnumerable<Categories>> GetAvailableCategories(DateTime daydate)
        {
            var query = await (from ud in _context.UserDayDish.Where(ud => ud.Date == daydate.ResetHMS())
                         join c in _context.Complex on ud.ComplexId equals c.Id
                         join cat in _context.Categories on c.CategoriesId equals cat.Id
                         select cat).Distinct().ToListAsync();
            return query;
        }

        public async Task<OrdersSnapshotViewModel> GetOrdersSnapshot(int? companyid,DateTime? daydate)
        {
            DateTime day = daydate.HasValue ? daydate.Value.ResetHMS() : DateTime.Now.ResetHMS();
            int cid = companyid.HasValue ? companyid.Value : 1;
            var res = new OrdersSnapshotViewModel();
            res.UserDays = await _context.UserDay.IgnoreQueryFilters().Where(ud => ud.Date == day && ud.CompanyId == cid).ToListAsync();
             res.UserDayComplexes = await _context.UserDayComplex.IgnoreQueryFilters().Where(ud => ud.CompanyId == cid && ud.Date == day).ToListAsync();
            var complexlistIds = res.UserDayComplexes.Select(uc => uc.ComplexId).ToList();
            res.Complexes = await _context.Complex.IgnoreQueryFilters().Where(c => complexlistIds.Contains(c.Id))
                .Include(c => c.DishComplex)
               .ToListAsync();
            res.UserDayDishes = await _context.UserDayDish.IgnoreQueryFilters().Where(ud => ud.Date == day && ud.CompanyId == cid).ToListAsync();

            return res;
        }
    }
}
