namespace Poster.Core.Expressions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Abstract;
    using Moq;

    public class MockExpressionBuilder : IMockExpressionBuilder
    {
        public LambdaExpression GetSetupExpression(MethodInfo method, ParameterInfo[]? parameters = null)
        {
            parameters ??= method.GetParameters();
            
            var parameter = Expression.Parameter(method.DeclaringType);
            var arguments = parameters
                .Select(p => Expression.Call(null, GetIsAnyMethodInfo().MakeGenericMethod(p.ParameterType)));
            var lambdaExpression = Expression.Lambda(Expression.Call(parameter, method, arguments), parameter);

            return lambdaExpression;
        }
        
        private MethodInfo GetIsAnyMethodInfo()
        {
            var methodInfo = typeof(It).GetMethod(nameof(It.IsAny));

            if (methodInfo == null)
                throw new Exception($"{nameof(It)} class doesn't contain {nameof(It.IsAny)} method");

            return methodInfo;
        }
    }
}