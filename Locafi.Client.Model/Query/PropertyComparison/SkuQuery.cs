using System;
using System.Linq.Expressions;
using Locafi.Client.Model.Dto.Skus;

namespace Locafi.Client.Model.Query.PropertyComparison
{
    public class SkuQuery : PropertyQueryBase<SkuSummaryDto>
    {
        public static SkuQuery NewQuery<TProperty>(Expression<Func<SkuSummaryDto, TProperty>> propertyLambda,
            TProperty value, ComparisonOperator op, int take = 100, int skip = 0)
        {
            var query = new SkuQuery();
            query.CreateQuery(propertyLambda, value, op, take, skip);
            return query;
        }
    }
}
