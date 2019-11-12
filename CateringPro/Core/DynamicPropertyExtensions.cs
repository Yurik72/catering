using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CateringPro.Core
{
    public static class DynamicPropertyExtensions
    {

        public static Action<TSource, TProperty> PropertySet<TSource, TProperty>(string propertyName)
        {
            var source = typeof(TSource);
            var propertyInfo = GetPropertyInfo(source, propertyName);
            return (Action<TSource, TProperty>)propertyInfo?.SetMethod?.CreateDelegate(typeof(Action<TSource, TProperty>));
        }
        public static Action<object, TProperty> PropertySet<TProperty>(this Type source, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(source, propertyName);
            if (propertyInfo == null)
                return null;
            var sourceObjectParam = Expression.Parameter(typeof(object));
            ParameterExpression propertyValueParam;
            Expression valueExpression;
            if (propertyInfo.PropertyType == typeof(TProperty))
            {
                propertyValueParam = Expression.Parameter(propertyInfo.PropertyType);
                valueExpression = propertyValueParam;
            }
            else
            {
                propertyValueParam = Expression.Parameter(typeof(TProperty));
                valueExpression = Expression.Convert(propertyValueParam, propertyInfo.PropertyType);
            }
            return (Action<object, TProperty>)Expression.Lambda(Expression.Call(Expression.Convert(sourceObjectParam, source), propertyInfo.SetMethod, valueExpression), sourceObjectParam, propertyValueParam).Compile();
        }

        public static Action<object, object> PropertySet(this Type source, string propertyName)
        {
            return source.PropertySet<object>(propertyName);
        }
        private static PropertyInfo GetPropertyInfo(Type source, string propertyName)
        {
            var propertyInfo = source.GetProperty(propertyName) ??
                                       source.GetProperty(propertyName, BindingFlags.NonPublic) ??
                                       source.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            return propertyInfo;
        }
        public static Action<object> StaticPropertySet(this Type source, string propertyName)
        {
            var propertyInfo = GetStaticPropertyInfo(source, propertyName);
            var valueParam = Expression.Parameter(typeof(object));
            var convertedValue = Expression.Convert(valueParam, propertyInfo.PropertyType);
            return (Action<object>)Expression.Lambda(Expression.Call(propertyInfo.SetMethod, convertedValue), valueParam).Compile();
        }
        private static PropertyInfo GetStaticPropertyInfo(Type source, string propertyName)
        {
            var propertyInfo = (source.GetProperty(propertyName, BindingFlags.Static) ??
                                       source.GetProperty(propertyName, BindingFlags.Static | BindingFlags.NonPublic)) ??
                                       source.GetProperty(propertyName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            return propertyInfo;
        }
    }
}
