namespace Poster.Core.Expressions.Abstract
{
    using System.Linq.Expressions;
    using System.Reflection;

    public interface IHttpClientExpressionBuilder
    {
        LambdaExpression GetExpressionForGenericTask(MethodInfo serviceMethod);

        LambdaExpression GetExpressionForNonGenericTask(MethodInfo serviceMethod);
    }
}