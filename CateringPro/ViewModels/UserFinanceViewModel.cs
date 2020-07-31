using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class UserFinanceViewModel
    {
        public UserFinanceViewModel()
        {
            Incomes = new HashSet<UserFinIncome>();
            Outcomes = new HashSet<UserFinOutCome>();
        }
        public UserFinance Finance { get; set; }

        public ICollection<UserFinIncome> Incomes { get; set; }

        public ICollection<UserFinOutCome> Outcomes { get; set; }
    }
}
