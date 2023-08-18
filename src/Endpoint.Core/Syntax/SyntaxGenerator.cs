// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Syntax;

public class SyntaxGenerator : ISyntaxGenerator
{
    private readonly ILogger<SyntaxGenerator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IObjectCache _cache;
    public SyntaxGenerator(ILogger<SyntaxGenerator> logger, IObjectCache cache, IServiceProvider serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<string> GenerateAsync<T>(T model, dynamic context = null)
    {
        var inner = typeof(IGenericSyntaxGenerationStrategy<>).MakeGenericType(model.GetType());

        var type = typeof(IEnumerable<>).MakeGenericType(inner);

        var strategies = _cache.FromCacheOrService(() => _serviceProvider.GetRequiredService(type) as IEnumerable<ISyntaxGenerationStrategy>, model.GetType().FullName);

        var orderedStrategies = strategies!.OrderByDescending(x => x.GetPriority());

        var result = string.Empty;

        foreach (var strategy in orderedStrategies)
        {
            result = await strategy.GenerateAsync(this, model, context);

            if (result != null)
            {
                return result;
            }
        }

        throw new Exception();
    }
}


