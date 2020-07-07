using CateringPro.Models;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CateringPro.Core
{
    public static class ControllerExtensions
    {
        public static IQueryable<T> GetQueryList<T>(this Controller cont, IQueryable<T> dbSet, QueryModel querymodel, Expression<Func<T, bool>> predicate,int  pageRecords) where T : CompanyData
        {
            cont.ViewData["QueryModel"] = querymodel;
            var query = dbSet.WhereCompany(cont.User.GetCompanyID());
            if (!string.IsNullOrEmpty(querymodel.SearchCriteria))
            {
                query = query.Where(predicate);


            }
            if (!string.IsNullOrEmpty(querymodel.SortField))
            {
                query = query.OrderByEx(querymodel.SortField, querymodel.SortOrder);
            }
            if (querymodel.Page > 0)
            {
                query = query.Skip(pageRecords * querymodel.Page);
            }
            return query;
        }
    }
}
