using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace CateringPro.Core
{
    public static class Linqextension
    {
        public static IQueryable<T> OrderByEx<T>(this IQueryable<T> source,  string propertyName,string order)
        {

            if (source == null || string.IsNullOrEmpty(propertyName))
                return source;
            if (string.IsNullOrEmpty(order))
                order = "ASC";
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");

            PropertyInfo pi = type.GetProperty(propertyName);
            if (pi == null)
                return source;
            Expression expr = Expression.Property(arg, pi);
            var lambda = Expression.Lambda(expr, arg);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), 
                order.ToUpper()=="DESC"?"OrderByDescending": "OrderBy", 
                new Type[] { type, pi.PropertyType }, 
                source.Expression,Expression.Quote(lambda));
            return source.Provider.CreateQuery<T>(resultExp);
            
        }
    }
}
