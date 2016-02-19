using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Search
{
    public interface ISearchResult<T>
    {
        IList<T> Entities { get; }
        IRestSearch<T> ContinuationSearch { get; }
    }
}
