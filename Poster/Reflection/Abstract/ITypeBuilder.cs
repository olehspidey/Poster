namespace Poster.Reflection.Abstract
{
    /// <summary>
    /// Type builder abstraction that can create new instance of http service.
    /// </summary>
    public interface ITypeBuilder
    {
        /// <summary>
        /// Creates new instance of <see cref="T"/> http service.
        /// </summary>
        /// <typeparam name="T">Type of http service.</typeparam>
        /// <returns>New instance of <see cref="T"/> http service.</returns>
        T GetInstance<T>()
            where T : class;
    }
}