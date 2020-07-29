using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IUserFinRepository
    {
        bool MakeOrderPayment(DateTime daydate, int companyId);
        Task<bool> MakeOrderPaymentAsync(DateTime daydate, int companyId);
    }
}
