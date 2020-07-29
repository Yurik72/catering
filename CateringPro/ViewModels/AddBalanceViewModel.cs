using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class AddBalanceViewModel
    {
        public string UserId { get; set; }

        public decimal CurrentBalance  { get; set; }

        public decimal AmountToAdd { get; set; }
    }
}
