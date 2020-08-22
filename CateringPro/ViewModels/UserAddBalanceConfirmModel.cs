using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class UserAddBalanceConfirmModel
    {

       public  UserAddBalanceConfirmModel(CompanyUser user,UserFinance fin, UserFinIncome income, CompanyUser parent)
        {
            User = user;
            Finance = fin;
            FinanceIncome = income;
            ParentUser = parent;
        }

        public CompanyUser User { get; private set; }

        public CompanyUser ParentUser { get; private set; }
        public UserFinance Finance {get;private set;}
        public UserFinIncome FinanceIncome { get; private set; }
    }
}
