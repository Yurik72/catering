using CateringPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IServiceRepository
    {
        Task<ServiceResponse> ProcessRequestAsync(ServiceRequest request);
    }
}
