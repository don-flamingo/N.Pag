using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using N.Pag.Exceptions;

namespace N.Pag.Expressions
{
    public static class SortExpressionFactory
    {
        private const string ASC = "-asc";
        private const string DESC = "-desc";

        // Code from: https://www.red-gate.com/simple-talk/dotnet/net-framework/dynamic-linq-queries-with-expression-trees/
        // Fixes from: https://stackoverflow.com/questions/31955025/generate-ef-orderby-expression-by-string
        public static IQueryable<T> SortBy<T>(IQueryable<T> queryable, string orderBy)
        {
            var propertiesNames = orderBy.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            var type = typeof(T);

            foreach (var propertyName in propertiesNames)
            {
                try
                {
                    var isDesc = propertyName.Contains(DESC);
                    var cleanPropertyName = GetCleanPropertyName(isDesc, propertyName);
                    var parameter = Expression.Parameter(type);

                    cleanPropertyName = cleanPropertyName.Trim();

                    queryable = IsNestedProperty<T>(cleanPropertyName) 
                        ? SortByNestedProperty(cleanPropertyName, parameter, queryable, isDesc) 
                        : SortByProperty(parameter, cleanPropertyName, queryable, isDesc);
                }
                catch (ArgumentException e)
                {
                    throw new NPagException($"One of propery name is invalid. If you want sort data in desc order, You must add '{DESC}' to property. ex: Name{DESC}", e);
                }
            }

            return queryable;
        }

        private static bool IsNestedProperty<T>(string cleanPropertyName)
        {
            return cleanPropertyName.Contains(".");
        }

        private static IQueryable<T> SortByNestedProperty<T>(string cleanPropertyName, ParameterExpression parameter,
            IQueryable<T> navigation, bool isDesc)
        {
            var nestedPropertiesNames = cleanPropertyName.Split('.').ToList();
            var parameters = new List<ParameterExpression> {parameter};

            for (var i = 0; i < nestedPropertiesNames.Count; i++)
            {
                if (i == nestedPropertiesNames.Count - 1)
                    break;

                var nestedProperty = typeof(T).GetProperty(nestedPropertiesNames[i], BindingFlags.IgnoreCase |  BindingFlags.Public | BindingFlags.Instance);
                if (nestedProperty == null)
                    throw new ArgumentException(
                        $"Invalid nested property name: {nestedPropertiesNames[i]}");

                var nestedParameter = Expression.Parameter(nestedProperty.PropertyType);
                parameters.Add(nestedParameter);
            }

            Expression propertyReference = null;         
            for (var i = 0; i < parameters.Count; i++)
            {
                propertyReference = Expression.Property(
                    propertyReference ?? parameters[i] ,
                    nestedPropertiesNames[i]);  
            }

            if (propertyReference.Type.IsValueType)
            {
                propertyReference = Expression.Convert(propertyReference, typeof(object));
            }

            var expression = Expression.Lambda<Func<T, object>>
                (propertyReference, parameter);

            return isDesc 
                ? navigation.OrderByDescending(expression) 
                : navigation.OrderBy(expression);
        }
        
        private static IQueryable<T> SortByProperty<T>(ParameterExpression parameter, string cleanPropertyName,
            IQueryable<T> navigation, bool isDesc)
        {
            Expression propertyReference = Expression.Property(parameter,
                cleanPropertyName);

            if (propertyReference.Type.IsValueType)
            {
                propertyReference = Expression.Convert(propertyReference, typeof(object));
            }

            var expression = Expression.Lambda<Func<T, object>>
                (propertyReference, parameter);

            return isDesc 
                ? navigation.OrderByDescending(expression) 
                : navigation.OrderBy(expression);
        }

        private static string GetCleanPropertyName(bool isDesc, string propertyName)
        {
            return propertyName.Replace(isDesc ? DESC : ASC, "");
        }
    }
}
