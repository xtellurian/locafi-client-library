using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Search
{
    public class SearchResult<T> : ISearchResult<T>
    {
        public SearchResult(IEnumerable<T> entities, IRestSearch<T> continuationSearch)
        {
            Entities = entities.ToList();
            ContinuationSearch = continuationSearch;
        }

        public IList<T> Entities { get; }
        public IRestSearch<T> ContinuationSearch { get; }
    }
}
