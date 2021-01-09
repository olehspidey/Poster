namespace Poster.Core.Abstraction
{
    public interface IPoster
    {
        TService BuildService<TService>() where TService : class;
    }
}