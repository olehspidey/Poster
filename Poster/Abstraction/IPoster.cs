namespace Poster.Abstraction
{
    /// <summary>
    /// Represents Poster main interface that is responsible for http services building.
    /// </summary>
    public interface IPoster
    {
        /// <summary>
        /// Builds http service.
        /// </summary>
        /// <typeparam name="TService">Type of http service.</typeparam>
        /// <returns>New instance of http service.</returns>
        TService BuildService<TService>()
            where TService : class;
    }
}