
using System;
using System.Linq.Expressions;
using Locafi.Client.Model.Dto.Users;

namespace Locafi.Client.Model.Query.PropertyComparison
{
    public class UserQuery : PropertyQueryBase<UserSummaryDto>
    {
        public static UserQuery NewQuery<TProperty>(Expression<Func<UserSummaryDto, TProperty>> propertyLambda,
    TProperty value, ComparisonOperator op, int take = 100, int skip = 0)
        {
            var query = new UserQuery();
            query.CreateQuery(propertyLambda, value, op, take, skip);
            return query;
        }
    }
}
