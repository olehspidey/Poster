namespace Poster.Expressions.Abstract
{
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents abstraction that can build expression for <see cref="Http.Clients.Abstract.IHttpClient"/> method invocation.
    /// </summary>
    public interface IHttpClientExpressionBuilder
    {
        /// <summary>
        /// Builds expression for <see cref="Task{TResult}"/> http service methods.
        /// </summary>
        /// <param name="serviceMethod">Http service method.</param>
        /// <returns>Expression for <see cref="Task{TResult}"/> http service methods.</returns>
        LambdaExpression GetExpressionForGenericTask(MethodInfo serviceMethod);

        /// <summary>
        /// Builds expression for <see cref="Task"/> http service methods.
        /// </summary>
        /// <param name="serviceMethod">Http service method.</param>
        /// <returns>Expression for <see cref="Task"/> http service methods.</returns>
        LambdaExpression GetExpressionForNonGenericTask(MethodInfo serviceMethod);
    }
}