using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Search
{
    public static class SearchExtensions
    {
        public static IRestSearch<T> Next<T>(this IRestSearch<T> query)
        {
            query.Skip = query.Skip + query.Take; // go to next batch of entites
            return query;
        }

        public static ISearchResult<T> AsSearchResult<T>(this IList<T> entities, IRestSearch<T> query)
        {
            if (entities.Count == query.Take)
            // there might be more entities to get from the server - generate the next query
            {
                var nextQuery = query.Next();
                return new SearchResult<T>(entities, nextQuery);
            }
            else
            {
                // there are no more entities
                return new SearchResult<T>(entities, null);
            }
        }
    }
}
