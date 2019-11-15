using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CateringPro.Core;
using CateringPro.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CateringPro.Repositories
{
    public class EntityWrap<TEntity> where TEntity : class
    {
        public TEntity Src { get; set; }
        private HashSet<Type> excl_list = new HashSet<Type>();

        public HashSet<Type> ExclusionList { get { return excl_list; } }

        private HashSet<Type> company_list = new HashSet<Type>();

        public HashSet<Type> CompanyList { get => company_list; }

        // private HashSet<Expression<Func<TEntity, IEnumerable<TProperty>>>> collection_list = new HashSet<Expression<Func<TEntity, IEnumerable<TProperty>>>>();
     
        private HashSet<IList> colsdeleted = new HashSet<IList>();


       public HashSet<IList> CollectionList { get => colsdeleted; }

    }
    public class ColEntry<TEntity, TProperty> 
        where TEntity : class
        where TProperty : class
    {

    }
    public static class CompanyExtension
    {
        
        public static EntityWrap<TEntity> TrackCollection<TEntity,T>(this TEntity src, IQueryable<T> origin, ICollection<T> current ) where TEntity : CompanyData
            where T :CompanyDataOwnId
        {
            var res = new EntityWrap<TEntity>() { Src = src };
            res.TrackCollection(origin, current);
            
           
            return res;
        }
        public static EntityWrap<TEntity> TrackCollection<TEntity,T>(this EntityWrap<TEntity> src, IEnumerable<T> origin, ICollection<T> current) where TEntity : CompanyData
             where T : CompanyDataOwnId
        {

            var deleted = origin.Where(o=>current.FirstOrDefault(c=>c.Id==o.Id)==null).ToList();
             src.CollectionList.Add(deleted);

            return src;
        }
        public static EntityWrap<TEntity> ExcludeTrack<TEntity>(this TEntity src, Type tp) where TEntity : CompanyData
        {
            var res = new EntityWrap<TEntity>() { Src=src};
            res.ExclusionList.Add(tp);
            return res;
        }
        public static EntityWrap<TEntity> ExcludeTrack<TEntity>(this EntityWrap<TEntity> src, Type tp) where TEntity : CompanyData
        {
            
            src.ExclusionList.Add(tp);
            return src;
        }
        public static EntityWrap<TEntity> IncludeCompany<TEntity>(this TEntity src, Type tp) where TEntity : CompanyData
        {
            var res = new EntityWrap<TEntity>() { Src = src };
            res.CompanyList.Add(tp);
            return res;
        }
        public static EntityWrap<TEntity> IncludeCompany<TEntity>(this EntityWrap<TEntity> src, Type tp) where TEntity : CompanyData
        {

            src.CompanyList.Add(tp);
            return src;
        }
        public static void AssignCompantAttr<TEntity>(this Controller ctl, TEntity entity) where TEntity : CompanyData
        {
            entity.CompanyId = ctl.User.GetCompanyID();
        }

        public static async Task<IActionResult> UpdateCompanyDataAsync<TEntity>(this Controller ctl, EntityWrap<TEntity> wrap, AppDbContext _context, ILogger<CompanyUser> _logger, Action<TEntity> postSaveAction = null) where TEntity : CompanyDataOwnId
        {
            return await ctl.UpdateCompanyDataAsync(wrap.Src, _context, _logger, postSaveAction, wrap);
        }
        public static async Task<IActionResult> UpdateCompanyDataAsync<TEntity>(this Controller ctl, TEntity entity, AppDbContext _context, ILogger<CompanyUser> _logger,Action<TEntity> postSaveAction=null, EntityWrap<TEntity> wrap=null) where TEntity : CompanyDataOwnId
        {
            if (!ctl.ModelState.IsValid)
                return ctl.PartialView(entity);
            bool res = await ctl.UpdateDBCompanyDataAsync(entity, _context, _logger,wrap);
            if (!res)
                return ctl.NotFound();
            if (postSaveAction != null)
                postSaveAction(entity);
            return ctl.Json(new { res = "OK" });

        }
        public static async Task<bool> UpdateDBCompanyDataAsync<TEntity>(this Controller ctl, TEntity entity, AppDbContext _context, ILogger<CompanyUser> _logger, EntityWrap<TEntity> wrap = null) where TEntity : CompanyDataOwnId
        {
            try
            {
                if (wrap != null)
                    ctl.PreApplyWraps(entity, _context, wrap);
                ctl.AssignCompantAttr(entity);
                _context.Update(entity);
                var entry = _context.Entry(entity);
                if (entry.State == EntityState.Modified)
                {
                    if(entry.OriginalValues.GetValue<int>("CompanyId")!= ctl.User.GetCompanyID())  //something wrong with hack
                    {
                        throw new Exception("Fobidden");
                    }
                }
                if (wrap != null)
                    ctl.PostApplyWraps(entity, _context, wrap);
                await _context.SaveChangesAsync();
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
            return true;
        }
        private static void PreApplyWraps<TEntity>(this Controller ctl, TEntity entity, AppDbContext _context, EntityWrap<TEntity> wrap) where TEntity : CompanyDataOwnId
        {
            foreach (var dels in wrap.CollectionList)
                ctl.TrackCollection(_context, entity, dels);
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                if (entry.State != EntityState.Deleted)
                    entry.State = EntityState.Detached;
            }
        }
        private static void PostApplyWraps<TEntity>(this Controller ctl, TEntity entity, AppDbContext _context, EntityWrap<TEntity> wrap) where TEntity : CompanyDataOwnId
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
                    ctl.AssignCompantAttr(entry.Entity as CompanyData);
                }
                
            }

        }
        private static void TrackCollection<TEntity>(this Controller ctl, AppDbContext _context, TEntity entity, IList deletedcol) where TEntity : CompanyData
        {
            // var compiled = navigationPath.Compile();
            // object res = compiled.DynamicInvoke(entity);
            //_context.Entry(entity).Collection(navigationPath).Load();

            
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                if (deletedcol.Contains(entry.Entity))
                {
                    entry.State = EntityState.Deleted;
                    
                }
                
            }
        }
    }
}
