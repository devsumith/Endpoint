// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Endpoint.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax;

public class SyntaxGenerator : ISyntaxGenerator
{
    private readonly ILogger<SyntaxGenerator> logger;
    private readonly IServiceProvider serviceProvider;
    private readonly IObjectCache cache;

    public SyntaxGenerator(ILogger<SyntaxGenerator> logger, IObjectCache cache, IServiceProvider serviceProvider)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<string> GenerateAsync<T>(T model)
    {
        var inner = typeof(IGenericSyntaxGenerationStrategy<>).MakeGenericType(model.GetType());

        var type = typeof(IEnumerable<>).MakeGenericType(inner);

        var strategies = cache.FromCacheOrService(() => serviceProvider.GetRequiredService(type) as IEnumerable<ISyntaxGenerationStrategy>, $"{GetType().Name}{model.GetType().FullName}");

        var orderedStrategies = strategies!.OrderByDescending(x => x.GetPriority());

        var result = string.Empty;

        foreach (var strategy in orderedStrategies)
        {
            result = await strategy.GenerateAsync(this, model);

            if (result != null)
            {
                return result;
            }
        }

        throw new Exception();
    }
}
