using Locafi.Client.Model.Dto.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Query.PropertyComparison
{
    public class TemplateQuery : PropertyQueryBase<TemplateSummaryDto>
    {
        public static TemplateQuery NewQuery<TProperty>(Expression<Func<TemplateSummaryDto, TProperty>> propertyLambda,
            TProperty value, ComparisonOperator op, int take = 100, int skip = 0)
        {
            var query = new TemplateQuery();
            query.CreateQuery(propertyLambda, value, op, take, skip);
            return query;
        }
    }
}
