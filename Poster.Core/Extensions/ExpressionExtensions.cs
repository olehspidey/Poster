namespace Poster.Core.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ExpressionExtensions
    {
        public static MethodInfo GetToStringMethodInfo(this ParameterExpression parameterExpression)
        {
            var toStringMethodInfo = parameterExpression.Type.GetMethod(nameof(ToString), Array.Empty<Type>());

            if (toStringMethodInfo == null)
                throw new ArgumentException($"Parameter expression type doesn't contain ToString() method",
                    nameof(parameterExpression));

            return toStringMethodInfo;
        }
        
        public static Expression GetStringValueExpression(this ParameterExpression parameterExpression)
        {
            if (parameterExpression.Type == typeof(string))
                return parameterExpression;
            
            var toStringExpression = Expression.Call(parameterExpression, parameterExpression.GetToStringMethodInfo());

            return toStringExpression;
        }    }
}