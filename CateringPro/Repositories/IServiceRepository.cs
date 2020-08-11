using CateringPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IServiceRepository
    {
        Task<UserCardViewModel> GetUserCardAsync(string cardToken);
        Task<IEnumerable<UserCardViewModel>> GetUserCardsAsync(QueryModel queryModel);
        Task<ServiceResponse> ProcessRequestAsync(ServiceRequest request);
    }
}
