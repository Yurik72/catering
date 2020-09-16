using CateringPro.Models;
using CateringPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IGenericModelRepository<TModel> where TModel : CompanyDataOwnId
    {
        IQueryable<TModel> Models { get; }

        TModel GetById(int? id);
        Task<TModel> GetByIdAsync(int? id);



        IEnumerable<TModel> GetAll();
        Task<IEnumerable<TModel>> GetAllAsync();

        void Add(TModel model);
        void Update(TModel model);
        void Remove(TModel model);

        void SaveChanges();
        Task SaveChangesAsync();
        Expression<Func<TModel, bool>> GetContainsFilter(string filter);
        DeleteDialogViewModel GetDeleteDialogViewModel(TModel src);
    }
}
