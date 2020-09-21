using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class UserFinanceReportViewModel
    {
        public DateTime Date { get; set; }
        public string  GroupName { get; set; }
        public string ChildNameSurname { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalWtithoutDiscount { get; set; }
    }
}
