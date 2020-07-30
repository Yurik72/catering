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
        public DateTime DayDate { get; set; }
        public string OveralResult { get; set; }

        public string ErrorMessage { get; set; }
        public ICollection<DeliveryDishViewModel> Dishes { get; set; }
    }
    public class DeliveryDishViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public bool IsComplex { get; set; }

        public int DishNumber {get;set;}

        public int ComplexId { get; set; }
    }
}
