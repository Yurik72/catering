using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class UserDayReportViewModel
    {
         public CompanyModel Company { get; set; }
        public IEnumerable<UserOrderDaysViewModel> Days { get; set; }

    }
    public class UserOrderDaysViewModel
    {
        public DateTime Day { get; set; }
        public IEnumerable<UserOneDayOrderViewModel> Users { get; set; }
    }
    public class UserOneDayOrderViewModel
    {
        public string UserId { get; set; }
    public string ChildNameSurname { get; set; }
    public int? GroupId { get; set; }
    public string GroupName { get; set; }
    public IEnumerable<UserDayReportDishesViewModel> Dishes { get; set; }
    }
    public class UserDayReportDishesViewModel
    {
       
        public int DishId { get; set; }
        public string DishName { get; set; }
        public int? ComplexKind { get; set; }
        public string? ComplexKindName { get; set; }
        public string ComplexName { get; set; }

    }
}
