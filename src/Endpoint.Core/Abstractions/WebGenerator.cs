// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Abstractions;

public class WebGenerator : IWebGenerator
{
    private readonly IEnumerable<IWebGenerationStrategy> _strategies;
    public WebGenerator(IEnumerable<IWebGenerationStrategy> strategies)
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

