using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IAddressRepository
    {
        IEnumerable<Address> Addresses { get; }

        Address GetById(int? id);
        Task<Address> GetByIdAsync(int? id);



        IEnumerable<Address> GetAll();
        Task<IEnumerable<Address>> GetAllAsync();

        void Add(Address adr);
        void Update(Address adr);
        void Remove(Address adr);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
