using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Query.Builder
{
    public class QueryBuilder <T> : QueryStringBuilderBase<T> where T : class 
    {
        private readonly IList<string> _filterExpressions = new List<string>();
        private int _skip;
        private int _take = 100; // default
        private QueryBuilder()
        {
            
        }
        public static QueryBuilder<T> NewQuery<TProperty>(Expression<Func<T, TProperty>> propertyLambda, TProperty value, ComparisonOperator op)
        {
            var builder = new QueryBuilder<T>();
            return builder.And(propertyLambda, value, op);
        } 
        public QueryBuilder<T> And<TProperty>(Expression<Func<T, TProperty>> propertyLambda, TProperty value, ComparisonOperator op)
        {
            var propertyInfo = Validate(propertyLambda);
            var filterString = BuildSingleExpression(value, op, propertyInfo);
            _filterExpressions.Add(filterString);
            return this;
        }

        public QueryBuilder<T> Skip(int skip)
        {
            _skip = skip;
            return this;
        }

        public QueryBuilder<T> Take(int take)
        {
            _take = take;
            return this;
        } 

        protected string BuildFilterExpression()
        {
            var filter = new StringBuilder();
            filter.Append(QueryStrings.Filter.FilterStart);
            var numberOfExpressions = _filterExpressions.Count;
            for (int c = 0; c < numberOfExpressions; c++)
            {
                filter.Append(_filterExpressions[c]);
                if (c < numberOfExpressions - 1) filter.Append(" and ");
            }

            return filter.ToString();
        }

        public IRestQuery<T> Build()
        {
            var finalValue = $"?{QueryStrings.Page.TopAndSkip(_take, _skip)}&{BuildFilterExpression()}";
            return new UriQuery<T>(finalValue, _take, _skip);
        } 
    }
}
