using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class PaymentRedirectViewModel
    {
        public string UserName { get; set; }

        public string ChildName { get; set; }
        public decimal Amount { get; set; }
        public bool IsConfrimed { get; set; }
    }
}
