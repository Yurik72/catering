using CateringPro.Models;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface ISelectListResult
    {
         SelectList SelectList { get; set; }
         string SourceField { get; set; }

         object SourceValue { get; set; }
    }
    public interface IGenericModelRepositoryBase
    {
        SelectList GetSelectList(string relationField, object relationvalue);
       
    }
    public interface IGenericModelRepository<TModel>: IGenericModelRepositoryBase where TModel : CompanyDataOwnId
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
        List<ISelectListResult> GetSelectList(TModel src);
        Task<bool> UpdateEntityAsync(TModel entity);
        Task<bool> UpdateEntityAsync(TModel entity, EntityWrap<TModel> wrap);
        string GetModelFriendlyNameEx(TModel src);
        string GetModelFriendlyName(TModel src);
        void SetUserContext(IUserContext cont);
    }
}
