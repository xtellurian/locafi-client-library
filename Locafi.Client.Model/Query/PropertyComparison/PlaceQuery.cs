using System;
using System.Linq.Expressions;
using Locafi.Client.Model.Dto.Places;

namespace Locafi.Client.Model.Query.PropertyComparison
{
    public class PlaceQuery: PropertyQueryBase<PlaceSummaryDto>
    {
        public static PlaceQuery NewQuery<TProperty>(Expression<Func<PlaceSummaryDto, TProperty>> propertyLambda,
            TProperty value, ComparisonOperator op, int take = 100, int skip = 0)
        {
            var query = new PlaceQuery();
            query.CreateQuery(propertyLambda, value, op, take, skip);
            return query;
        }
    }

}
