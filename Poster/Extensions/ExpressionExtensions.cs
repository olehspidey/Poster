namespace Poster.Core.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Represents class with <see cref="ParameterExpression"/> extensions.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Gets expression that will cast to string when it is not <see cref="string"/> type.
        /// </summary>
        /// <param name="parameterExpression">Parameter expression.</param>
        /// <returns>Expression that will cast to string when it is not <see cref="string"/> type.</returns>
        public static Expression GetStringValueExpression(this ParameterExpression parameterExpression)
        {
            if (parameterExpression.Type == typeof(string))
                return parameterExpression;

            var toStringExpression = Expression.Call(parameterExpression, parameterExpression.GetToStringMethodInfo());

            return toStringExpression;
        }

        private static MethodInfo GetToStringMethodInfo(this ParameterExpression parameterExpression)
        {
            var toStringMethodInfo = parameterExpression.Type.GetMethod(nameof(ToString), Array.Empty<Type>());

            if (toStringMethodInfo == null)
            {
                throw new ArgumentException(
                    "Parameter expression type doesn't contain ToString() method",
                    nameof(parameterExpression));
            }

            return toStringMethodInfo;
        }
    }
}