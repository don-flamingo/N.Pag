using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using NPag.Exceptions;

namespace NPag.Expressions
{
    public static class WhereExpressionFactory
    {
        public static IQueryable<T> CreateFromString<T>(IQueryable<T> queryable, string where)
        {
            try
            {
                var whereExpression = (Expression<Func< T, bool >>) DynamicExpressionParser.ParseLambda(typeof(T), typeof(bool), where);
                return queryable.Where(whereExpression);
            }
            catch (Exception e)
            {
                throw new NPagException($"Where is invalid, check it and try again Predicate ex. Property == Value. current: {where}", e);
            }
        }
        
        public static Expression<Func<T, bool>> CreateFromString<T>(string where)
        {
            try
            {
                return  (Expression<Func< T, bool >>) DynamicExpressionParser.ParseLambda(typeof(T), typeof(bool), where);
            }
            catch (Exception e)
            {
                throw new NPagException($"Where is invalid, check it and try again Predicate ex. Property == Value. current: {where}", e);
            }
        }
    }
}