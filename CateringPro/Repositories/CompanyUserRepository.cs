using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using Microsoft.EntityFrameworkCore;
using CateringPro.Data;

namespace CateringPro.Repositories
{
    public class UserGroupsRepository : IUserGroupsRepository
    {
        private readonly AppDbContext _context;

        public UserGroupsRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserGroups> UserGroups => _context.UserGroups; //include here

        public void Add(UserGroups usergroups)
        {
            _context.Add(usergroups);
        }

        public IEnumerable<UserGroups> GetAll()
        {
            return _context.UserGroups.ToList();
        }

        public async Task<IEnumerable<UserGroups>> GetAllAsync()
        {
            return await _context.UserGroups.ToListAsync();
        }

        public UserGroups GetById(int? id)
        {
            return _context.UserGroups.FirstOrDefault(p => p.Id == id);
        }

        public async Task<UserGroups> GetByIdAsync(int? id)
        {
            return await _context.UserGroups.FirstOrDefaultAsync(p => p.Id == id);
        }

        

        public void Remove(UserGroups usergroups)
        {
            _context.Remove(usergroups);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(UserGroups usergroups)
        {
            _context.Update(usergroups);
        }

    }
}
