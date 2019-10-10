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
            string[] nestedProps = propertyName.Split('.');
            Expression expr=null;

            PropertyInfo propinfo = null;

            if (nestedProps.Count() > 1) //nested
            {
                Expression mbr = arg;
                Type typeofparent = null;
                for (int i = 0; i < nestedProps.Length; i++)
                {
                    typeofparent = mbr.Type;
                    mbr = Expression.PropertyOrField(mbr, nestedProps[i]);
                    
                }
                //LambdaExpression pred = Expression.Lambda(
                //               Expression.Equal(mbr, Expression.Constant(value)), arg);
                expr = mbr;
                propinfo= typeofparent.GetProperty(nestedProps[nestedProps.Length-1]);
                if (propinfo == null)
                    return source;
            }
            else
            {
                propinfo = type.GetProperty(propertyName);
                if (propinfo == null)
                    return source;
                expr = Expression.Property(arg, propinfo);
            }
          
            
            var lambda = Expression.Lambda(expr, arg);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), 
                order.ToUpper()=="DESC"?"OrderByDescending": "OrderBy", 
                new Type[] { type, propinfo.PropertyType }, 
                source.Expression,Expression.Quote(lambda));
            return source.Provider.CreateQuery<T>(resultExp);
            
        }
    }
}
