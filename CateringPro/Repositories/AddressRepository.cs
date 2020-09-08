using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using Microsoft.EntityFrameworkCore;
using CateringPro.Data;

namespace CateringPro.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _context;

        public AddressRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Address> Addresses => _context.Addresses; //include here

        public void Add(Address adr)
        {
            _context.Add(adr);
        }

        public IEnumerable<Address> GetAll()
        {
            return _context.Addresses.ToList();
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _context.Addresses.ToListAsync();
        }

        public Address GetById(int? id)
        {
            return _context.Addresses.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Address> GetByIdAsync(int? id)
        {
            return await _context.Addresses.FirstOrDefaultAsync(p => p.Id == id);
        }

        

        public void Remove(Address adr)
        {
            _context.Remove(adr);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Address adr)
        {
            _context.Update(adr);
        }

    }
}
