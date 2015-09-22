using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Orders;

namespace Locafi.Client.Model.Query.PropertyComparison
{
    public class OrderQuery : PropertyQueryBase<OrderSummaryDto>
    {
        public static OrderQuery NewQuery<TProperty>(Expression<Func<OrderSummaryDto, TProperty>> propertyLambda,
           TProperty value, ComparisonOperator op, int take = 100, int skip = 0)
        {
            var query = new OrderQuery();
            query.CreateQuery(propertyLambda, value, op, take, skip);
            return query;
        }
    }
}
