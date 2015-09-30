using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Query
{
    public static class QueryExtensions
    {
        public static IRestQuery<T> Next<T>(this IRestQuery<T> query)
        {
            query.Skip = query.Skip + query.Take; // go to next batch of entites
            return query;
        }

        public static IQueryResult<T> AsQueryResult<T>(this IList<T> entities, IRestQuery<T> query)
        {
            if (entities.Count == query.Take)
                // there might be more entities to get from the server - generate the next query
            {
                var nextQuery = query.Next();
                return new QueryResult<T>(entities, nextQuery);
            }
            else
            {
                // there are no more entities
                return new QueryResult<T>(entities, null);
            }
        } 
    }
}
