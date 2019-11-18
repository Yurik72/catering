using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IMassEmailRepository
    {
        Task<bool> SaveMassEMailAsync(MassEmail mail);
    }
}
