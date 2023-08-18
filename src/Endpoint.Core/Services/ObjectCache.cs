using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Endpoint.Core.Services;

public class ObjectCache: IObjectCache
{
    private readonly ILogger<ObjectCache> _logger;

    private static ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

    public ObjectCache(ILogger<ObjectCache> logger){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private object Get(string key) {

        _cache.TryGetValue(key, out object value);

        return value;
    }

    private void Add(object objectToCache, string key)
    {
        _logger.LogInformation("Adding item to cache. {key}", key);

        _cache[key] = objectToCache;
    }

    public TResponse FromCacheOrService<TResponse>(Func<TResponse> action, string key)
    {
        _logger.LogInformation("Retrieving object from cache or service. {key}", key);

        var cached = Get(key);

        if (cached == null)
        {

            _logger.LogInformation("Cache miss. {key}", key);

            cached = action();

            Add(cached, key);
        }

        return (TResponse)cached;
    }
}

