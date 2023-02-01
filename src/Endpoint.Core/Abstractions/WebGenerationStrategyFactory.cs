// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.WebArtifacts;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Abstractions;

public class WebGenerationStrategyFactory : IWebGenerationStrategyFactory
{
    private readonly IEnumerable<IWebGenerationStrategy> _strategies;
    public WebGenerationStrategyFactory(IEnumerable<IWebGenerationStrategy> strategies)
    {
        _strategies = strategies;
    }
    public void CreateFor(LitWorkspaceModel model, dynamic context = null)
    {
        var strategy = _strategies.Where(x => x.CanHandle(model, context))
            .OrderBy(x => x.Priority)
            .FirstOrDefault();

        strategy.Create(model);
    }
}

