using Locafi.Client.Model.Dto.Reasons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Query.PropertyComparison
{
    public class ReasonQuery : PropertyQueryBase<ReasonDetailDto>
    {
        public static ReasonQuery NewQuery<TProperty>(Expression<Func<ReasonDetailDto, TProperty>> propertyLambda,
            TProperty value, ComparisonOperator op, int take = 100, int skip = 0)
        {
            var query = new ReasonQuery();
            query.CreateQuery(propertyLambda, value, op, take, skip);
            return query;
        }
    }
}
