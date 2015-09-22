using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Snapshots;

namespace Locafi.Client.Model.Query.PropertyComparison
{
    public class SnapshotQuery : PropertyQueryBase<SnapshotSummaryDto>
    {
        public static SnapshotQuery NewQuery<TProperty>(Expression<Func<SnapshotSummaryDto, TProperty>> propertyLambda,
            TProperty value, ComparisonOperator op, int take = 100, int skip = 0)
        {
            var query = new SnapshotQuery();
            query.CreateQuery(propertyLambda, value, op, take, skip);
            return query;
        }
    }
}
