using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Search
{
    public class ItemSearch : PropertySearchBase<ItemSummaryDto>
    {
        public ItemSearch()
        {
            Skip = 0;
            Take = 100;
            SearchParameters = new List<SearchParameter>();
        }
    }
}
