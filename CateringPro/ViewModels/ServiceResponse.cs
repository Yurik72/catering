using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class ServiceResponse
    {
        public ServiceResponse()
        {
            Dishes = new HashSet<DeliveryDishViewModel>();
        }
        public static ServiceResponse GetFailResult()
        {
            return new ServiceResponse() { OveralResult = "fail" };
        }
        public static ServiceResponse GetSuccessResult()
        {
            return new ServiceResponse() { OveralResult = "success" };
        }
        public bool IsSuccess() { return OveralResult == "success"; }
        public DateTime DayDate { get; set; }
        public string OveralResult { get; set; }

        public string ErrorMessage { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }

        public int? UserPictureId { get; set; }

        public IEnumerable<DeliveryDishViewModel> Dishes { get; set; }
    }
    public class ServiceQueueResponse : ServiceResponse
    {
        public ServiceQueueResponse()
        {
            Queues = new HashSet<ServiceResponse>();
        }
        public static new ServiceQueueResponse GetFailResult()
        {
            return new ServiceQueueResponse() { OveralResult = "fail" };
        }
        public static new ServiceQueueResponse GetSuccessResult()
        {
            return new ServiceQueueResponse() { OveralResult = "success" };
        }

        public ICollection<ServiceResponse> Queues { get; set; }
    }

    public class DeliveryDishViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public bool IsComplex { get; set; }

        public int DishNumber { get; set; }

        public int ComplexId { get; set; }
        public int QueueId { get; set; }
    }
    public class DeliveryQueueForGroup
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DishName { get; set; }
        public int DishId { get; set; }

        public int QueueId { get; set; }
        public int? UserPictureId { get; set; }
    }

}