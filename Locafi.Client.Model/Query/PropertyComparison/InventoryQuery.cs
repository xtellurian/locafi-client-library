using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Inventory;

namespace Locafi.Client.Model.Query.PropertyComparison
{
    public class InventoryQuery : PropertyQueryBase<InventorySummaryDto>
    {

        public static InventoryQuery NewQuery<TProperty>(Expression<Func<InventorySummaryDto, TProperty>> propertyLambda, 
            TProperty value, ComparisonOperator op, int take = 100, int skip = 0)
        {
            var query = new InventoryQuery();
            query.CreateQuery(propertyLambda, value,op,take,skip);
            return query;
        }
    }
}
