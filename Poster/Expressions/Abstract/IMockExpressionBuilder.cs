namespace Poster.Expressions.Abstract
{
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Represents abstraction that can build expressions for <see cref="Moq.Mock"/> method invocation.
    /// </summary>
    public interface IMockExpressionBuilder
    {
        /// <summary>
        /// Builds expression for <see cref="Moq.Mock"/> setup method.
        /// </summary>
        /// <param name="method">Http service method.</param>
        /// <param name="parameters">Parameters of http service methods.</param>
        /// <returns>Expression for <see cref="Moq.Mock"/> setup method.</returns>
        LambdaExpression GetSetupExpression(MethodInfo method, ParameterInfo[]? parameters = null);
    }
}