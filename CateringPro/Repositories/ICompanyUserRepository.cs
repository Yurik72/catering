using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface ICompanyUserRepository
    {
        IEnumerable<CompanyUser> CompanyUser { get; }

        CompanyUser GetById(int? id);
        Task<CompanyUser> GetByIdAsync(int? id);



        IEnumerable<CompanyUser> GetAll();
        Task<IEnumerable<CompanyUser>> GetAllAsync();

        void Add(CompanyUser usergroups);
        void Update(CompanyUser usergroups);
        void Remove(CompanyUser usergroups);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
