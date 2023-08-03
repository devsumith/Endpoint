// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Abstractions;

public class SyntaxGenerator : ISyntaxGenerator
{
    private readonly IEnumerable<ISyntaxGenerationStrategy> _strategies;
    private readonly ILogger<SyntaxGenerator> _logger;

    public SyntaxGenerator(IEnumerable<ISyntaxGenerationStrategy> strategies, ILogger<SyntaxGenerator> logger)
    {
        _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> CreateAsync(object model, dynamic context = null)
    {
        var orderedStrategies = _strategies.OrderByDescending(x => x.Priority);

        var strategies = orderedStrategies.Where(x => x.CanHandle(model, context));

        var strategy = strategies.FirstOrDefault();

        return await strategy.CreateAsync(model, context);
    }
}

