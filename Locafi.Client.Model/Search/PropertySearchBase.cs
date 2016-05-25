using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Search
{
    public abstract class PropertySearchBase<T> : IRestSearch<T>
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public SearchCollectionType SearchType { get; set; }
        public IList<SearchParameterDto> SearchParameters { get; set; }

        public SearchCollectionDto AsRestSearch()
        {
            return new SearchCollectionDto()
            {
                Skip = this.Skip,
                Take = this.Take,
                SearchType = this.SearchType,
                SearchParameterDtos = this.SearchParameters
            };
        }
    }
}
