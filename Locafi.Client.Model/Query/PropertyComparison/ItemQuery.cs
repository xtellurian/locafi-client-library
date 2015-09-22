using System;
using System.Linq.Expressions;
using Locafi.Client.Model.Dto.Items;

namespace Locafi.Client.Model.Query.PropertyComparison
{
    public class ItemQuery : PropertyQueryBase<ItemSummaryDto>
    {
        public static ItemQuery NewQuery<TProperty>(Expression<Func<ItemSummaryDto, TProperty>> propertyLambda,
            TProperty value, ComparisonOperator op, int take = 100, int skip = 0)
        {
            var query = new ItemQuery();
            query.CreateQuery(propertyLambda, value, op, take, skip);
            return query;
        }
    }
}
