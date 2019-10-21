using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
namespace CateringPro.Repositories
{
    public interface IInvoiceRepository
    {
        CompanyModel GetOwnCompany(int companyid);

        CompanyModel GetUserCompany(string UserId);
        InvoiceModel CustomerInvoice(string UserId, DateTime daydate, int companyid);
        IEnumerable<CustomerOrdersViewModel> CustomerOrders( DateTime daydate, int companyid);
    }
}
