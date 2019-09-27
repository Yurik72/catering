using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace CateringPro.Core
{
    public static class RazorExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        public static async Task<IHtmlContent> InvokeComponentAsync<TModel,TResult>(this RazorPage<IEnumerable<TModel>> source, IViewComponentHelper comphelper, string componentname,  Expression<Func<TModel, TResult>> expression, object param)
        {
           // source.Component.
            return await comphelper.InvokeAsyncEx(componentname, expression,param);
        }
        public static async Task<IHtmlContent> InvokeAsyncEx<TModel, TResult>(this IViewComponentHelper source,string componentname, Expression<Func<TModel, TResult>> expression, object param)
        {
            
            return await source.InvokeAsync(componentname, new {field= GetPropertyName(expression), queryModel=param });
        }
        public static string GetPropertyName<TModel, TResult>(System.Linq.Expressions.Expression<Func<TModel, TResult>> property)
        {
            System.Linq.Expressions.LambdaExpression lambda = (System.Linq.Expressions.LambdaExpression)property;
            System.Linq.Expressions.MemberExpression memberExpression;

            if (lambda.Body is System.Linq.Expressions.UnaryExpression)
            {
                System.Linq.Expressions.UnaryExpression unaryExpression = (System.Linq.Expressions.UnaryExpression)(lambda.Body);
                memberExpression = (System.Linq.Expressions.MemberExpression)(unaryExpression.Operand);
            }
            else
            {
                memberExpression = (System.Linq.Expressions.MemberExpression)(lambda.Body);
            }

            return ((PropertyInfo)memberExpression.Member).Name;
        }
    }
}
