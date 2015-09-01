using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Locafi.Client.Model.Query.PropertyComparison
{
    public abstract class PropertyQueryBase<T> : IRestQuery<T>
    {
        /// <summary>
        /// Creates an OData compatible relative URI as Query String.
        /// May throw exceptions for unsupported types
        /// </summary>
        /// <typeparam name="TProperty">The type of the Property you are querying. Can be String, Guid, or DateTimeOffset</typeparam>
        /// <param name="propertyLambda">eg: s => s.PropertyName</param>
        /// <param name="value">The value being compared in the operation</param>
        /// <param name="op">The comparison operator </param>
        public void CreateQuery<TProperty>(Expression<Func<T, TProperty>> propertyLambda, TProperty value, ComparisonOperator op)
        {
            var propInfo = Validate(propertyLambda);

            switch (op)
            {
                case ComparisonOperator.Equals:
                    QueryString = $"{QueryStrings.FilterStart}{QueryStrings.Equals(propInfo.Name, ConvertToOdataValue(value))}";
                    break;
                case ComparisonOperator.Contains:
                    QueryString = $"{QueryStrings.FilterStart}{QueryStrings.Contains(propInfo.Name, ConvertToOdataValue(value))}";
                    break;
                case ComparisonOperator.GreaterThan:
                    QueryString = $"{QueryStrings.FilterStart}{QueryStrings.GreaterThan(propInfo.Name, ConvertToOdataValue(value))}";
                    break;
                case ComparisonOperator.LessThan:
                    QueryString = $"{QueryStrings.FilterStart}{QueryStrings.LessThan(propInfo.Name, ConvertToOdataValue(value))}";
                    break;
                default:
                    QueryString = $"{QueryStrings.FilterStart}{QueryStrings.Contains(propInfo.Name, ConvertToOdataValue(value))}";
                    break;
            }
        }
        
        private string ConvertToOdataValue<TProperty>(TProperty p)
        {
            var type = typeof (TProperty);
            if (type == typeof (string)) // strings must be surrounded like so: '<string_value>'
                return $"'{p}'";
            if (type == typeof (DateTimeOffset)) //datetimes are difficult
            {
                var d = (DateTimeOffset) (object) p;
                return d.ToString("o").Remove(19) + 'Z';
            } 
            if (type == typeof (Guid) || type == typeof(Guid?)) // guids are not surrounded by ''
                return $"{p}";
            throw new NotImplementedException($"{type.FullName} is not supported as a queriable property");
        }

        private static PropertyInfo Validate<TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            Type type = typeof (T);
            Type otherType = typeof (TProperty);
            // validate is not method
            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
            // validate is property
            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
            // validate is part of type
            if (!type.GetRuntimeProperties().Contains(propInfo))
                throw new ArgumentException(
                    $"Expresion '{propertyLambda}' refers to a property that is not from type {type}.");
            // validate value and property type are the same
            Type propertyType = propInfo.PropertyType;
            if (propertyType != otherType)
                throw new ArgumentException("Types must match");


            //validate is queriable type - string, Guid, DateTime

            if (propertyType == typeof (string) || propertyType == typeof (Guid) || propertyType == typeof(Guid?) || propertyType == typeof (DateTimeOffset))
            {
                return propInfo;
            }
            else
            {
                throw new ArgumentException($"{propertyType.FullName} is not supported");
            }
        }


        public string QueryString { get; private set; }
        public virtual string AsRestQuery()
        {
            return QueryString;
        }
    }
}
