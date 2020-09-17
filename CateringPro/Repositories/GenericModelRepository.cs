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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using Microsoft.ApplicationInsights.Common;

namespace CateringPro.Repositories
{
    public class GenericModelRepository<TModel> : IGenericModelRepository<TModel> where TModel : CompanyDataOwnId
    {
        public class SelectListResult: ISelectListResult
        {
            public SelectList SelectList { get;set;}
            public string SourceField { get; set; }

            public object SourceValue { get; set; }
        }
        private readonly AppDbContext _context;
        private readonly SharedViewLocalizer _localizer;
        private readonly ILogger<GenericModelRepository<TModel>> _logger;
        private readonly IHttpContextAccessor _httpcontext;
        private  IUserContext _usercontext;
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
        public void SetUserContext(IUserContext cont)
        {
            _usercontext = cont;
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
        private  IEnumerable<PropertyInfo> GetNameProps()
        {
           return  typeof(TModel).GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(DefaultNameAttribute)) || prop.Name=="Name");
             
        }
        private IEnumerable<PropertyInfo> GetNameExProps()
        {
            return typeof(TModel).GetProperties().Where(
                 prop => Attribute.IsDefined(prop, typeof(DefaultNameExAttribute)) || prop.Name == "Name");

        }
        public string GetModelFriendlyName(TModel src)
        {
            List<string> names = new List<string>();
            GetNameProps().ToList().ForEach(prop =>   names.Add(prop.GetValue(src).ToString()));
            return string.Join(",", names);
        }
        public string GetModelFriendlyNameEx(TModel src)
        {
            List<string> names = new List<string>();
            GetNameExProps().ToList().ForEach(prop => names.Add(prop.GetValue(src).ToString()));
            return string.Join(",", names);
        }
        private object GetFieldValue(TModel src, PropertyInfo prop)
        {
            return prop.GetValue(src);
        }
        private object GetFieldValue(TModel src, string propname)
        {
            return GetFieldValue(src, typeof(TModel).GetProperty(propname));
        }
        public DeleteDialogViewModel GetDeleteDialogViewModel(TModel src)
        {
            return new DeleteDialogViewModel() { Id = src.Id, Name = GetModelFriendlyName(src),ModelName=_localizer[typeof(TModel).Name] };
        }
        public SelectList GetSelectList(string relationField,object relationvalue)
        {
            var res = new SelectList(Models, "Id", GetNameProps().First().Name, relationvalue);
            return res;
        }

        private List<ISelectListResult> BuildViewBagRelations(TModel src)
        {
            var res = new List<ISelectListResult>();
            var navs = GetOneToManyNavigations().ToList();
            navs.ForEach(n => {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        var repotype = n.ForeignKey.PrincipalEntityType.ClrType;
                        if (repotype.IsSubclassOf(typeof(CompanyDataOwnId)))
                        {
                            var sl = new SelectListResult();
                            var relrepo = GetModelRepository(serviceScope, n.ForeignKey.PrincipalEntityType.ClrType);
                            var relfield = n.ForeignKey.Properties.First().Name;
                            sl.SelectList  = relrepo.GetSelectList(relfield, GetFieldValue(src, relfield));
                            res.Add(sl);
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }
            });
            return res;

        }
        public List<ISelectListResult> GetSelectList(TModel src)
        {
              return  BuildViewBagRelations(src);
        }
        private IEnumerable<INavigation> GetOneToManyNavigations() {

            return _context.Model.FindEntityType(typeof(TModel)).GetNavigations().Where(n => !n.PropertyInfo.PropertyType.IsGenericType);
        }
        private IGenericModelRepositoryBase GetModelRepository(IServiceScope scope, Type modelType)
        {
            Type generic = typeof(IGenericModelRepository<>);
           
            Type[] typeArgs = { modelType };

            Type constructed = generic.MakeGenericType(typeArgs);

             
            return scope.ServiceProvider.GetRequiredService(constructed) as IGenericModelRepositoryBase; 
        }

        public virtual async Task<bool> UpdateEntityAsync(TModel entity)
        {
            return await UpdateEntityAsync(entity, null);
        }
        public virtual  async Task<bool> UpdateEntityAsync(TModel entity, EntityWrap<TModel> wrap)
        {
            var res = false;
            try
            {
                if (wrap != null)
                    PreApplyWraps(entity,  wrap);
                AssignCompantAttr(entity);
                _context.Update(entity);
                var entry = _context.Entry(entity);
                if (entry.State == EntityState.Modified)
                {
                    if (entry.OriginalValues.GetValue<int>("CompanyId") != this.CompanyId)  //something wrong with hack
                    {
                        throw new Exception("Fobidden");
                    }
                }
                if (wrap != null)
                    PostApplyWraps(entity,  wrap);
                await _context.SaveChangesAsync();
                res = await PostUpdateEntityAsync(entity);
            }
            catch (DbUpdateConcurrencyException exdb)
            {
                _logger.LogError(exdb, "Update {0}", entity.GetType().Name);
                if (_context.Find(entity.GetType(), entity.Id) == null)
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update {0}", entity.GetType().Name);
                return false; //to do
            }
            return res;
        }
        public virtual async Task<bool> PostUpdateEntityAsync(TModel entity)
        {
            return true;
        }
        public  void AssignCompantAttr(TModel entity)
        {
            entity.CompanyId = this.CompanyId;
        }
        private void PreApplyWraps(TModel entity,  EntityWrap<TModel> wrap) 
        {
            foreach (var dels in wrap.CollectionList)
                TrackCollection(entity, dels);
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                if (entry.State != EntityState.Deleted)
                    entry.State = EntityState.Detached;
            }
        }
        private  void TrackCollection(TModel entity, IList deletedcol)
        {

            foreach (var entry in _context.ChangeTracker.Entries())
            {
                if (deletedcol.Contains(entry.Entity))
                {
                    entry.State = EntityState.Deleted;

                }

            }
        }
        private  void PostApplyWraps(TModel entity,  EntityWrap<TModel> wrap) 
        {
            // return;
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                if (wrap.ExclusionList.Contains(entry.Entity.GetType()))
                {
                    entry.State = EntityState.Unchanged;

                    //_context.Entry(entry.Entity).State = EntityState.Detached;
                }
                if (wrap.CompanyList.Contains(entry.Entity.GetType()))
                {
                    AssignCompantAttr(entry.Entity as TModel);
                    
                }

            }

        }
    }
}
