using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Services;

public class ObjectCache : IObjectCache
{
    private static ConcurrentDictionary<string, object> cache = new ConcurrentDictionary<string, object>();

    private readonly ILogger<ObjectCache> logger;

    public ObjectCache(ILogger<ObjectCache> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public TResponse FromCacheOrService<TResponse>(Func<TResponse> action, string key)
    {
        logger.LogInformation("Retrieving object from cache or service. {key}", key);

        var cached = Get(key);

        if (cached == null)
        {
            logger.LogInformation("Cache miss. {key}", key);

            cached = action();

            Add(cached, key);
        }

        return (TResponse)cached;
    }

    private object Get(string key)
    {
        cache.TryGetValue(key, out object value);

        return value;
    }

    private void Add(object objectToCache, string key)
    {
        logger.LogInformation("Adding item to cache. {key}", key);

        cache[key] = objectToCache;
    }
}
