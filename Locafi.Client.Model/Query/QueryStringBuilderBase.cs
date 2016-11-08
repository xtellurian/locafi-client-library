using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Query
{
    public abstract class QueryStringBuilderBase<T>
    {
        protected string BuildSingleExpression<TProperty>(TProperty value, ComparisonOperator op, PropertyInfo propInfo, Expression<Func<T, TProperty>> propertyLambda)
        {
            var filter = "";
            switch (op)
            {
                case ComparisonOperator.Equals:
                    filter =
                        $"{QueryStrings.Filter.Equals(ConvertToOdataProperty(propInfo, propertyLambda), ConvertToOdataValue(value))}";
                    break;
                case ComparisonOperator.NotEquals:
                    filter =
                        $"{QueryStrings.Filter.NotEquals(ConvertToOdataProperty(propInfo, propertyLambda), ConvertToOdataValue(value))}";
                    break;
                case ComparisonOperator.Contains:
                    filter =
                        $"{QueryStrings.Filter.Contains(ConvertToOdataProperty(propInfo, propertyLambda), ConvertToOdataValue(value))}";
                    break;
                case ComparisonOperator.GreaterThan:
                    filter =
                        $"{QueryStrings.Filter.GreaterThan(ConvertToOdataProperty(propInfo, propertyLambda), ConvertToOdataValue(value))}";
                    break;
                case ComparisonOperator.LessThan:
                    filter =
                        $"{QueryStrings.Filter.LessThan(ConvertToOdataProperty(propInfo, propertyLambda), ConvertToOdataValue(value))}";
                    break;
                case ComparisonOperator.GreaterThanOrEqual:
                    filter =
                        $"{QueryStrings.Filter.GreaterThanOrEqual(ConvertToOdataProperty(propInfo, propertyLambda), ConvertToOdataValue(value))}";
                    break;
                case ComparisonOperator.LessThanOrEqual:
                    filter =
                        $"{QueryStrings.Filter.LessThanOrEqual(ConvertToOdataProperty(propInfo, propertyLambda), ConvertToOdataValue(value))}";
                    break;
                case ComparisonOperator.ContainedIn:
                    filter =
                        $"{QueryStrings.Filter.ContainedIn(ConvertToOdataValue(value), ConvertToOdataProperty(propInfo, propertyLambda))}";
                    break;
                default:
                    filter =
                        $"{QueryStrings.Filter.Contains(ConvertToOdataProperty(propInfo, propertyLambda), ConvertToOdataValue(value))}";
                    break;
            }
            return filter;
        }

        private string ConvertToOdataProperty<TProperty>(PropertyInfo propInfo, Expression<Func<T, TProperty>> propertyLambda)
        {
            // validate is a supported method
            MethodCallExpression method = propertyLambda.Body as MethodCallExpression;
            if (method != null)
            {
                if (method.Method.Name != "ToLower")
                    throw new ArgumentException($"Expression '{propertyLambda}' refers to a method that is not supported.");

                switch(method.Method.Name)
                {
                    case "ToLower": return "tolower(" + propInfo.Name + ")";
                    default:
                        throw new ArgumentException($"Expression '{propertyLambda}' refers to a method that is not supported.");
                }
            }

            return propInfo.Name;
        }

        private string ConvertToOdataValue<TProperty>(TProperty p)
        {
            if (p == null)
                return $"null";

            var type = typeof(TProperty);

            // bool's are not surrounded by '', and are lower case
            if (type == typeof(bool)
                || type == typeof(byte)
                || type == typeof(char)
                || type == typeof(decimal)
                || type == typeof(double)
                || type == typeof(float)
                || type == typeof(int)
                || type == typeof(long)
                || type == typeof(sbyte)
                || type == typeof(short)
                || type == typeof(uint)
                || type == typeof(ulong)
                || type == typeof(ushort)
                )
            {
                return $"{p.ToString().ToLower()}";
            }

            if (type == typeof(Guid) || type == typeof(Guid?)) // guid's are not surrounded by ''
                return $"{p}";

            if (type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?)) //datetimes are difficult
            {
                var d = (DateTimeOffset)(object)p;
                return d.ToString("o").Remove(19) + 'Z';
            }

            if (type == typeof(string) || type.GetTypeInfo().BaseType == typeof(Enum) || type.GetTypeInfo().BaseType == typeof(ValueType)) // strings must be surrounded like so: '<string_value>'
                return $"'{p}'";
            
            
            throw new NotImplementedException($"{type.FullName} is not supported as a queriable property");
        }

        protected PropertyInfo Validate<TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            Type type = typeof(T);
            Type otherType = typeof(TProperty);
            MemberExpression member;
            PropertyInfo propInfo;
            // validate is a supported method
            MethodCallExpression method = propertyLambda.Body as MethodCallExpression;
            if (method != null)
            {
                if (method.Method.Name != "ToLower")
                    throw new ArgumentException($"Expression '{propertyLambda}' refers to a method that is not supported.");

                member = method.Object as MemberExpression;
            }
            else
            {
                // validate is not method
                member = propertyLambda.Body as MemberExpression;
                if (member == null)
                    throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
            }

            // validate is property
            propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");

            // validate is part of type
            var basetype = type;
            while (!basetype.GetRuntimeProperties().Contains(propInfo))
            {
                basetype = basetype.GetTypeInfo().BaseType;
                if (basetype == null)
                {
                    throw new ArgumentException(
                        $"Expresion '{propertyLambda}' refers to a property that is not from type {type}.");
                }
            }

            // validate value and property type are the same
            Type propertyType = propInfo.PropertyType;
            if (propertyType != otherType)
                throw new ArgumentException("Types must match");


            //validate is queriable type - string, Guid, DateTime

            if (propertyType == typeof(string) || propertyType == typeof(Guid) || propertyType == typeof(Guid?) || propertyType == typeof(DateTimeOffset) || propertyType.GetTypeInfo().BaseType == typeof(Enum) || propertyType.GetTypeInfo().BaseType == typeof(ValueType))
            {
                return propInfo;
            }
            else
            {
                throw new ArgumentException($"{propertyType.FullName} is not supported");
            }
        }
    }
}
