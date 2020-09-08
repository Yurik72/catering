using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using Microsoft.EntityFrameworkCore;
using CateringPro.Data;

namespace CateringPro.Repositories
{
    public class GenericModelRepository<TModel> : IGenericModelRepository<TModel> where TModel : CompanyDataOwnId
    {
        private readonly AppDbContext _context;

        public GenericModelRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TModel> Models => _context.Set<TModel>(); //include here

        public void Add(TModel model)
        {
            _context.Add(model);
        }

        public IEnumerable<TModel> GetAll()
        {
            return _context.Set<TModel>().ToList();
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await _context.Set<TModel>().ToListAsync();
        }

        public TModel GetById(int? id)
        {
            return _context.Set<TModel>().FirstOrDefault(p => p.Id == id);
        }

        public async Task<TModel> GetByIdAsync(int? id)
        {
            return await _context.Set<TModel>().FirstOrDefaultAsync(p => p.Id == id);
        }

        

        public void Remove(TModel adr)
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

        public void Update(TModel model)
        {
            _context.Update(model);
        }

    }
}
