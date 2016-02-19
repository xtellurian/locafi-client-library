using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Search
{
    public interface IRestSearch<T>
    {
        int Take { get; set; }
        int Skip { get; set; }
        SearchCollectionType SearchType{ get; set; }
        IList<SearchParameter> SearchParameters { get; set; }

        SearchCollectionDto AsRestSearch();
    }
}
