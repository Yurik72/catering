using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IUserContext
    {
        string UserId { get; }
        int CompanyId { get; }
    }
}
