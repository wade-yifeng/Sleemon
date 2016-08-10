namespace Sleemon.Common
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    public static class ExpressionBuilder
    {
        private static readonly Lazy<MethodInfo> _methodEnumerableContains = new Lazy<MethodInfo>((Func<MethodInfo>)(() => Enumerable.Single<MethodInfo>(Enumerable.Select(Enumerable.Where(Enumerable.Select(Enumerable.Where<MethodInfo>((IEnumerable<MethodInfo>)typeof(Enumerable).GetMethods(), (Func<MethodInfo, bool>)(methodInfo => methodInfo.Name == "Contains")), methodInfo =>
        {
            var fAnonymousType1 = new
            {
                methodInfo = methodInfo,
                parameterInfo = methodInfo.GetParameters()
            };
            return fAnonymousType1;
        }), param0 => param0.parameterInfo.Length == 2 && param0.parameterInfo[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>)), param0 => param0.methodInfo))));

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return ExpressionBuilder.Compose<Func<T, bool>>(first, second, new Func<Expression, Expression, Expression>(Expression.And));
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            Expression expression = ParameterRebinder.ReplaceParameters(Enumerable.ToDictionary(Enumerable.Select((IEnumerable<ParameterExpression>)first.Parameters, (f, i) =>
            {
                var fAnonymousType0 = new
                {
                    f = f,
                    s = second.Parameters[i]
                };
                return fAnonymousType0;
            }), p => p.s, p => p.f), second.Body);
            return Expression.Lambda<T>(merge(first.Body, expression), (IEnumerable<ParameterExpression>)first.Parameters);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return ExpressionBuilder.Compose<Func<T, bool>>(first, second, new Func<Expression, Expression, Expression>(Expression.Or));
        }

        public static Expression<Func<T, bool>> BuildComparisonExpression<T>(string propertyName, ComparisonOperandType comparisonOperandType, object value)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "p");
            MemberExpression memberExpression = Expression.Property((Expression)parameterExpression, propertyName);
            Expression expression = (Expression)Expression.Constant(value);
            if (TypeExtensions.IsNullableType(memberExpression.Type) && !TypeExtensions.IsNullableType(expression.Type))
                expression = (Expression)Expression.Convert(expression, memberExpression.Type);
            BinaryExpression binaryExpression;
            switch (comparisonOperandType)
            {
                case ComparisonOperandType.GreaterThan:
                    binaryExpression = Expression.GreaterThan((Expression)memberExpression, expression);
                    break;
                case ComparisonOperandType.GreaterThanOrEqual:
                    binaryExpression = Expression.GreaterThanOrEqual((Expression)memberExpression, expression);
                    break;
                case ComparisonOperandType.Equal:
                    binaryExpression = Expression.Equal((Expression)memberExpression, expression);
                    break;
                case ComparisonOperandType.NotEqual:
                    binaryExpression = Expression.NotEqual((Expression)memberExpression, expression);
                    break;
                case ComparisonOperandType.LessThanOrEqual:
                    binaryExpression = Expression.LessThanOrEqual((Expression)memberExpression, expression);
                    break;
                case ComparisonOperandType.LessThan:
                    binaryExpression = Expression.LessThan((Expression)memberExpression, expression);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("comparisonOperandType");
            }
            return Expression.Lambda<Func<T, bool>>((Expression)binaryExpression, new ParameterExpression[1]
            {
        parameterExpression
            });
        }

        public static Expression<Func<T, bool>> BuildConstraintExpression<T>(string propertyName, params object[] values)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "p");
            MemberExpression memberExpression = Expression.Property((Expression)parameterExpression, propertyName);
            Type type = memberExpression.Type;
            Array instance = Array.CreateInstance(type, values.Length);
            Array.Copy((Array)values, instance, values.Length);
            ConstantExpression constantExpression = Expression.Constant((object)instance);
            return Expression.Lambda<Func<T, bool>>((Expression)Expression.Call(ExpressionBuilder._methodEnumerableContains.Value.MakeGenericMethod(type), (Expression)constantExpression, (Expression)memberExpression), new ParameterExpression[1]
            {
        parameterExpression
            });
        }

        public static Expression<Func<T, bool>> BuildStringMethodExpression<T>(string propertyName, string methodName, string value)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "p");
            MemberExpression memberExpression = Expression.Property((Expression)parameterExpression, propertyName);
            if (memberExpression.Type != typeof(string))
                throw new InvalidOperationException(string.Format("property {0} of type {1} is not a string", (object)propertyName, (object)parameterExpression.Type.Name));
            MethodInfo method = Enumerable.Single<MethodInfo>(Enumerable.Where<MethodInfo>((IEnumerable<MethodInfo>)memberExpression.Type.GetMethods(), (Func<MethodInfo, bool>)(m => m.Name == methodName && m.GetParameters().Length == 1)));
            return Expression.Lambda<Func<T, bool>>((Expression)Expression.Call((Expression)memberExpression, method, (Expression[])new ConstantExpression[1]
            {
        Expression.Constant((object) value)
            }), new ParameterExpression[1]
            {
        parameterExpression
            });
        }
    }
}
