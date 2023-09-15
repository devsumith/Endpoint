// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Artifacts;

public class ArtifactParser : IArtifactParser
{
    private readonly ILogger<ArtifactParser> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IObjectCache _cache;
    public ArtifactParser(ILogger<ArtifactParser> logger, IServiceProvider serviceProvider, IObjectCache cache)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<T> ParseAsync<T>(string valueOrDirectoryOrPath)
        where T: class
    {
        _logger.LogInformation("Parsing for Artifact. {typeName}", typeof(T).Name);

        var inner = typeof(IArtifactParsingStrategy<>).MakeGenericType(typeof(T));

        var type = typeof(IEnumerable<>).MakeGenericType(inner);

        var strategies = _cache.FromCacheOrService(() => _serviceProvider.GetRequiredService(type) as IEnumerable<IArtifactParsingStrategy>, typeof(T).FullName);

        var orderedStrategies = strategies!.OrderByDescending(x => x.GetPriority());

        foreach (var strategy in orderedStrategies)
        {
            var result = await strategy.ParseObjectAsync(this, valueOrDirectoryOrPath);

            if (result != null)
            {
                return result as T;
            }
        }

        throw new InvalidOperationException();
    }
}


