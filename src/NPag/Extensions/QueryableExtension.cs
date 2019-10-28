using System.Linq;
using NPag.Expressions;
using NPag.Queries;
using NPag.Settings;

namespace NPag.Extensions
{
    public static class QueryableExtension
    {
        /// <summary>
        /// Where, pagination and sort
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="paginationQuery"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IQueryable<TModel> FilterBy<TModel>(this IQueryable<TModel> queryable,
            IPaginationQuery paginationQuery)
        {
            return queryable.Where(paginationQuery).TransformBy(paginationQuery);
        }
        
        /// <summary>
        /// Sort and paginate results
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="paginationQuery"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IQueryable<TModel> TransformBy<TModel>(this IQueryable<TModel> queryable,
            IPaginationQuery paginationQuery)
        {
            var pageSize = paginationQuery.PageSize > PaginationSettings.MaxPageSize
                ? PaginationSettings.MaxPageSize
                : paginationQuery.PageSize;

            if (!string.IsNullOrEmpty(paginationQuery.OrderBy))
            {
                queryable = SortExpressionFactory.SortBy(queryable, paginationQuery.OrderBy);
            }
            
            queryable = queryable
                .Skip(pageSize * paginationQuery.Page)
                .Take(pageSize);

            return queryable;
        }

        /// <summary>
        /// Filter results
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="paginationQuery"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IQueryable<TModel> Where<TModel>(this IQueryable<TModel> queryable,
            IPaginationQuery paginationQuery)
        {
            if (!string.IsNullOrEmpty(paginationQuery.Where))
            {
                queryable = WhereExpressionFactory.CreateFromString(queryable, paginationQuery.Where);
            }
            
            return queryable;
        }
    }
}