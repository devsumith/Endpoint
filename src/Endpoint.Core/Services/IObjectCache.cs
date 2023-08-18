namespace Endpoint.Core.Services;

public interface IObjectCache
{
    TResponse FromCacheOrService<TResponse>(Func<TResponse> action, string key);

}

