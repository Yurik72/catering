﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            
            return await source.InvokeAsync(componentname, new {field= GetPropertyName(expression),displayname= GetDisplayName(expression), queryModel=param });
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
            string parent = "";
            if(typeof(TModel)!= memberExpression.Expression.Type)  /// sub property // to do hierahly cycle
            {
                var submemember = memberExpression.Expression as System.Linq.Expressions.MemberExpression;
                if (submemember != null)
                    parent = submemember.Member.Name+".";
            }

            return parent+((PropertyInfo)memberExpression.Member).Name;
        }
        public static string GetDisplayName<TModel, TResult>(System.Linq.Expressions.Expression<Func<TModel, TResult>> property)
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
            var dispnameattr = memberExpression.Member.CustomAttributes.SingleOrDefault(at => at.AttributeType == typeof(DisplayNameAttribute));
            if (dispnameattr != null && dispnameattr.ConstructorArguments.Count>0)
                return dispnameattr.ConstructorArguments[0].Value.ToString();
            return ((PropertyInfo)memberExpression.Member).Name;
        }
    }
}
