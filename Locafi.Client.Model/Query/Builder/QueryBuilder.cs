using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Query.Builder
{
    public enum LogicalOperator
    {
        And,
        Or,
        Not,
        OpenGroup,
        CloseGroup
    }

    public class FilterExpression
    {
        public string Expression { get; set; }
        public LogicalOperator Operator { get; set; }
    }

    public class QueryBuilder <T> : QueryStringBuilderBase<T> where T : class 
    {
        private readonly IList<FilterExpression> _filterExpressions = new List<FilterExpression>();
        private int _skip;
        private int _take = 100; // default

        private QueryBuilder()
        {
            
        }
        public static QueryBuilder<T> NewQuery()
        {
            var builder = new QueryBuilder<T>();
            return builder;
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
            _filterExpressions.Add(new FilterExpression() { Expression = filterString, Operator = LogicalOperator.And });
            return this;
        }

        public QueryBuilder<T> Or<TProperty>(Expression<Func<T, TProperty>> propertyLambda, TProperty value, ComparisonOperator op)
        {
            var propertyInfo = Validate(propertyLambda);
            var filterString = BuildSingleExpression(value, op, propertyInfo);
            _filterExpressions.Add(new FilterExpression() { Expression = filterString, Operator = LogicalOperator.Or });
            return this;
        }

        public QueryBuilder<T> Not<TProperty>(Expression<Func<T, TProperty>> propertyLambda, TProperty value, ComparisonOperator op)
        {
            var propertyInfo = Validate(propertyLambda);
            var filterString = BuildSingleExpression(value, op, propertyInfo);
            _filterExpressions.Add(new FilterExpression() { Expression = filterString, Operator = LogicalOperator.Not });
            return this;
        }

        public QueryBuilder<T> AndOpenGroup()
        {
            _filterExpressions.Add(new FilterExpression() { Expression = " and (", Operator = LogicalOperator.OpenGroup });
            return this;
        }

        public QueryBuilder<T> OrOpenGroup()
        {
            _filterExpressions.Add(new FilterExpression() { Expression = " or (", Operator = LogicalOperator.OpenGroup });
            return this;
        }

        public QueryBuilder<T> OpenGroup()
        {
            _filterExpressions.Add(new FilterExpression() { Expression = "(", Operator = LogicalOperator.OpenGroup });
            return this;
        }

        public QueryBuilder<T> CloseGroup()
        {
            _filterExpressions.Add(new FilterExpression() { Expression = ")", Operator = LogicalOperator.CloseGroup });
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

        public QueryBuilder<T> TakeAll()
        {
            _take = -1;
            return this;
        }

        protected string BuildFilterExpression()
        {
            var filter = new StringBuilder();
            if (_filterExpressions.Count > 0)
            {
                // loop through all expressions and build the query
                filter.Append(QueryStrings.Filter.FilterStart);
                var numberOfExpressions = _filterExpressions.Count;
                for (int c = 0; c < numberOfExpressions; c++)
                {
                    // if it is not our first one then we will add the logical operator
                    if (c > 0 && c < numberOfExpressions)
                    {
                        // if its not a close group then add the operator
                        if (_filterExpressions[c].Operator != LogicalOperator.CloseGroup && _filterExpressions[c].Operator != LogicalOperator.OpenGroup  && _filterExpressions[c-1].Operator != LogicalOperator.OpenGroup)
                            filter.Append(" " + _filterExpressions[c].Operator.ToString().ToLower() + " ");
                        else
                        {
                            // just close the group and continue to the next part of the expression
                            filter.Append(_filterExpressions[c].Expression);
                            continue;
                        }
                    }

                    // add the expression
                    filter.Append(_filterExpressions[c].Expression);
                }
            }

            return filter.ToString();
        }

        public IRestQuery<T> Build()
        {
            var finalValue = $"?{QueryStrings.Page.TopAndSkip(_take, _skip)}";
            var filter = BuildFilterExpression();
            if (!string.IsNullOrEmpty(filter))
                finalValue += $"&{ filter }";
            return new UriQuery<T>(finalValue, _take, _skip);
        } 
    }
}
