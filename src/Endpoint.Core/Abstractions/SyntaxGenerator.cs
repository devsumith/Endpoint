// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Abstractions;

public class SyntaxGenerator : ISyntaxGenerator
{
    private readonly IEnumerable<ISyntaxGenerationStrategy> _strategies;

    public SyntaxGenerator(IEnumerable<ISyntaxGenerationStrategy> strategies)
    {
        _strategies = strategies;
    }
    public string CreateFor(object model, dynamic context = null)
    {
        var orderedStrategies = _strategies.OrderByDescending(x => x.Priority);

        var strategies = orderedStrategies.Where(x => x.CanHandle(model, context));

        var strategy = strategies.FirstOrDefault();

        return strategy.Create(model, context);
    }
}

