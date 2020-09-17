using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.ComponentModel.DataAnnotations;

namespace CateringPro.Core
{
    public class GroupResult<TItem>
    {
        public object Key { get; set; }
        public int Count { get; set; }
        public IEnumerable<TItem> Items { get; set; }
        public IEnumerable<GroupResult<TItem>> SubGroups { get; set; }
        public override string ToString()
        { return string.Format("{0} ({1})", Key, Count); }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DefaultNameAttribute : DataTypeAttribute
    {
        public DefaultNameAttribute():base(nameof(DefaultNameAttribute))
        {

        }

    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DefaultNameExAttribute : DataTypeAttribute
    {
        public DefaultNameExAttribute() : base(nameof(DefaultNameExAttribute))
        {

        }

    }
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
    public static IEnumerable<GroupResult<TElement>> GroupByMany<TElement>(
    this IEnumerable<TElement> elements,
    params Func<TElement, object>[] groupSelectors)
        {
            if (groupSelectors.Length > 0)
            {
                var selector = groupSelectors.First();

                //reduce the list recursively until zero
                var nextSelectors = groupSelectors.Skip(1).ToArray();
                return
                    elements.GroupBy(selector).Select(
                        g => new GroupResult<TElement>
                        {
                            Key = g.Key,
                            Count = g.Count(),
                            Items = g,
                            SubGroups = g.GroupByMany(nextSelectors)
                        });
            }
            else
            {
                return null;
            }
        }
        public static Expression<Func<T, bool>> GetContainsFilter<T>(this IQueryable<T> source, string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return null;
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new Type[] { typeof(string) }); // Contains Method

            Type type = typeof(T);
            var propsfilter = type.GetProperties().Where(p => p.CanRead && p.PropertyType.UnderlyingSystemType == typeof(string)).ToList();
            Expression<Func<T, bool>> finalBinaryExpression = null;

            Expression finalExpression = Expression.Constant(true);
            var glob_parameter = Expression.Parameter(type, ""); //property get
            BinaryExpression cummulative = null;
            Expression cummulativebody = null;
            foreach (var prop in propsfilter)
            {

                var member = Expression.Property(glob_parameter, prop.Name);
                var constant = Expression.Constant(filter);
                Expression body = Expression.Call(member, methodInfo, constant);


                if (finalBinaryExpression == null)
                {
                    finalBinaryExpression = Expression.Lambda<Func<T, bool>>(body, glob_parameter); ;
                    cummulativebody = body;

                }
                else
                {
                    if (cummulative == null)
                    {
                        cummulative = Expression.MakeBinary(ExpressionType.OrElse, cummulativebody, body);
                    }
                    else
                    {
                        cummulative = Expression.MakeBinary(ExpressionType.OrElse, cummulative, body);
                    }
                    finalBinaryExpression = Expression.Lambda<Func<T, bool>>(cummulative, glob_parameter);
                }
                /*
                var prop_count = bodies.Count();
                if (prop_count == 2)
                {
                    cummulative = Expression.MakeBinary(
                        ExpressionType.OrElse,
                         bodies[0], bodies[1]);
                   finalBinaryExpression = Expression.Lambda<Func<T, bool>>(
                        Expression.MakeBinary(
                        ExpressionType.OrElse,
                         bodies[0], bodies[1]), glob_parameter);
                    finalBinaryExpression = Expression.Lambda<Func<T, bool>>(
                        cummulative, glob_parameter);
                }
                else if (prop_count > 2)
                {

                    cummulative = Expression.MakeBinary(
                        ExpressionType.OrElse,
                         cummulative,body);
                    finalBinaryExpression = Expression.Lambda<Func<T, bool>>(
                       cummulative, glob_parameter);
                }
                */
                //var dynlambda= DynamicExpressionParser.ParseLambda(type, typeof(bool), "Code.Contains(@0) || Name.Contains(@0)", filter);

            }
            return finalBinaryExpression;
        }
    }
}
