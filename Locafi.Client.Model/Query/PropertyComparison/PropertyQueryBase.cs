using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Locafi.Client.Model.Query.PropertyComparison
{
    public abstract class PropertyQueryBase<T> : QueryStringBuilderBase<T>, IRestQuery<T>
    {
        /// <summary>
        /// Creates an OData compatible relative URI as Query String.
        /// May throw exceptions for unsupported types
        /// </summary>
        /// <typeparam name="TProperty">The type of the Property you are querying. Can be String, Guid, or DateTimeOffset</typeparam>
        /// <param name="propertyLambda">eg: s => s.PropertyName</param>
        /// <param name="value">The value being compared in the operation</param>
        /// <param name="op">The comparison operator </param>
        /// <param name="skip">The number of results to skip</param>
        /// <param name="take">The numvber of result to take</param>
        public void CreateQuery<TProperty>(Expression<Func<T, TProperty>> propertyLambda, TProperty value, ComparisonOperator op,  int take = 100, int skip = 0)
        {
            Take = take;
            Skip = skip;
            var propInfo = Validate(propertyLambda);

            FilterString = $"{QueryStrings.Filter.FilterStart}{BuildSingleExpression(value, op, propInfo)}";
        }


        public string FilterString { get; private set; }
        public virtual string AsRestQuery()
        {
            return $"?{QueryStrings.Page.TopAndSkip(Take, Skip)}&{FilterString}";
        }

        public int Take { get; set; }
        public int Skip { get; set; }
    }
}
