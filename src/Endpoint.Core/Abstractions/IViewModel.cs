namespace Endpoint.Core.Abstractions
{
    public interface IViewModel<T>
    {
        IViewModel<T> MapFrom(T model);
    }
}
