// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Endpoint.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts;

public class ArtifactGenerator : IArtifactGenerator
{
    private readonly ILogger<ArtifactGenerator> logger;
    private readonly IObjectCache cache;
    private readonly IServiceProvider serviceProvider;

    public ArtifactGenerator(
        ILogger<ArtifactGenerator> logger,
        IServiceProvider serviceProvider,
        IObjectCache cache)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task GenerateAsync(object model)
    {
        logger.LogInformation("Generating artifact for model. {type}", model.GetType());

        var inner = typeof(IGenericArtifactGenerationStrategy<>).MakeGenericType(model.GetType());

        var type = typeof(IEnumerable<>).MakeGenericType(inner);

        var strategies = cache.FromCacheOrService(() => serviceProvider.GetRequiredService(type) as IEnumerable<IArtifactGenerationStrategy>, $"{GetType().Name}{model.GetType().FullName}");

        var orderedStrategies = strategies!.OrderByDescending(x => x.GetPriority());

        foreach (var strategy in orderedStrategies)
        {
            if (await strategy.GenerateAsync(this, model))
            {
                break;
            }
        }
    }
}
