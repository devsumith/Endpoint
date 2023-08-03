// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Abstractions;

public class ArtifactGenerator : IArtifactGenerator
{
    private readonly IEnumerable<IArtifactGenerationStrategy> _strategies;
    private readonly ILogger<ArtifactGenerator> _logger;

    public ArtifactGenerator(
        IEnumerable<IArtifactGenerationStrategy> strategies,
        ILogger<ArtifactGenerator> logger
        )
    {
        _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task CreateAsync(object model, dynamic context = null)
    {
        var strategy = _strategies.Where(x => x.CanHandle(model, context))
        .OrderBy(x => x.Priority)
        .FirstOrDefault();

        await strategy.CreateAsync(model, context);
    }
}

