using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using CateringPro.Core;
using System.Reflection;
using System.Text;
using CateringPro.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CateringPro.Repositories
{
    public class GenericModelRepository<TModel> : IGenericModelRepository<TModel> where TModel : CompanyDataOwnId
    {
        private readonly AppDbContext _context;
        private readonly SharedViewLocalizer _localizer;
        private readonly ILogger<GenericModelRepository<TModel>> _logger;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly IUserContext _usercontext;
        private readonly IServiceProvider _serviceProvider;
        public GenericModelRepository(AppDbContext context, SharedViewLocalizer localizer, 
            ILogger<GenericModelRepository<TModel>> logger, IHttpContextAccessor httpcontext,
            IServiceProvider serviceProvider,
            IUserContext usercontext=null)
        {
            _context = context;
            _localizer = localizer;
            _logger = logger;
            _httpcontext = httpcontext;
            _usercontext = usercontext;  // for unit tests
            _serviceProvider = serviceProvider;
        }
        private int CompanyId
        {
            get
            {
                if (_httpcontext.HttpContext != null && _httpcontext.HttpContext.User != null)
                    return _httpcontext.HttpContext.User.GetCompanyID();
                if (_usercontext != null)
                    return _usercontext.CompanyId;
                return -1;
            }
        }
        private string UserId
        {
            get
            {
                if (_httpcontext.HttpContext != null && _httpcontext.HttpContext.User != null)
                    return _httpcontext.HttpContext.User.GetUserId();
                if (_usercontext != null)
                    return _usercontext.UserId;
                return null;
            }
        }
        public Expression<Func<TModel, bool>> GetContainsFilter(string filter)
        {
            return _context.Set<TModel>().AsQueryable().GetContainsFilter(filter);
        }
        public IQueryable<TModel> Models => _context.Set<TModel>(); //include here

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
        private  IEnumerable<PropertyInfo> GetNameAttributes()
        {
           return  typeof(TModel).GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(DefaultNameAttribute)) || prop.Name=="Name");
             
        }
        private string GetModelFriendlyName(TModel src)
        {
            List<string> names = new List<string>();
            GetNameAttributes().ToList().ForEach(prop =>   names.Add(prop.GetValue(src).ToString()));
            return string.Join(",", names);
        }

        public DeleteDialogViewModel GetDeleteDialogViewModel(TModel src)
        {
            return new DeleteDialogViewModel() { Id = src.Id, Name = GetModelFriendlyName(src),ModelName=_localizer[typeof(TModel).Name] };
        }
        private void BuildViewBagRelations()
        {
            var navs = GetOneToManyNavigations().ToList();
            navs.ForEach(n => { 
            
            
            });

        }
        private IEnumerable<INavigation> GetOneToManyNavigations() {

            return _context.Model.FindEntityType(typeof(TModel)).GetNavigations().Where(n => !n.PropertyInfo.PropertyType.IsGenericType);
        }
        private object GetModelRepository(Type modelType)
        {
            Type generic = typeof(GenericModelRepository<>);
           
            Type[] typeArgs = { modelType };

            Type constructed = generic.MakeGenericType(typeArgs);
            return null;
        }
        

        

    }
}
