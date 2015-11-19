using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.SkuGroups;

namespace Locafi.Client.Model.Query.PropertyComparison
{
    public class SkuGroupNameQuery : PropertyQueryBase<SkuGroupNameDetailDto>
    {
        public static SkuGroupNameQuery NewQuery<TProperty>(Expression<Func<SkuGroupNameDetailDto, TProperty>> propertyLambda,
            TProperty value, ComparisonOperator op, int take = 100, int skip = 0)
        {
            var query = new SkuGroupNameQuery();
            query.CreateQuery(propertyLambda, value, op, take, skip);
            return query;
        }
    }
}
