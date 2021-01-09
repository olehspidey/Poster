namespace Poster.Core.Expressions.Abstract
{
    using System.Linq.Expressions;
    using System.Reflection;

    public interface IMockExpressionBuilder
    {
        LambdaExpression GetSetupExpression(MethodInfo method, ParameterInfo[]? parameters = null);
    }
}