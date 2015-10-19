using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Query
{
    public class QueryResult<T> : IQueryResult<T>
    {
        public QueryResult(IEnumerable<T> entities, IRestQuery<T> continuationQuery)
        {
            Entities = entities.ToList();
            ContinuationQuery = continuationQuery;
        }

        public IList<T> Entities { get; }
        public IRestQuery<T> ContinuationQuery { get; }
    }
}
