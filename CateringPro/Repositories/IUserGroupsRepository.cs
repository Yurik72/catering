using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IUserGroupsRepository
    {
        IEnumerable<UserGroups> UserGroups { get; }

        UserGroups GetById(int? id);
        Task<UserGroups> GetByIdAsync(int? id);



        IEnumerable<UserGroups> GetAll();
        Task<IEnumerable<UserGroups>> GetAllAsync();

        void Add(UserGroups usergroups);
        void Update(UserGroups usergroups);
        void Remove(UserGroups usergroups);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
