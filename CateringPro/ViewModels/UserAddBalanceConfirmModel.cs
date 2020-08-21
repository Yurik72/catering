using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class UserAddBalanceConfirmModel
    {

       public  UserAddBalanceConfirmModel(CompanyUser user,UserFinance fin, UserFinIncome income)
        {
            User = user;
            Finance = fin;
            FinanceIncome = income;
            
        }

        public CompanyUser User { get; private set; }
        public UserFinance Finance {get;private set;}
        public UserFinIncome FinanceIncome { get; private set; }
    }
}
