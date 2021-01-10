namespace Poster.Reflection.Abstract
{
    using System.Collections.Generic;
    using System.Reflection;
    using Moq;

    /// <summary>
    /// Represents abstraction of runtime method body replacer.
    /// </summary>
    public interface IMethodReplacer
    {
        /// <summary>
        /// Replaces methods bodies in runtime.
        /// </summary>
        /// <param name="methods">Methods for body replacement.</param>
        /// <param name="mock"><see cref="Mock"/> object.</param>
        /// <typeparam name="T">Type of http service.</typeparam>
        void ReplaceMethodsBodies<T>(IReadOnlyCollection<MethodInfo> methods, Mock<T> mock)
            where T : class;
    }
}