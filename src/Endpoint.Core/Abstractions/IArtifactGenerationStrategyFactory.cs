// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Abstractions;


public class ArtifactUpdateGenerator : IArtifactUpdateGenerator
{
    private readonly IEnumerable<IArtifactUpdateStrategy> _strategies;

    public ArtifactUpdateGenerator(IEnumerable<IArtifactUpdateStrategy> strategies)
    {
        _strategies = strategies;
    }
    public void CreateFor(dynamic context = null, params object[] args)
    {
        var strategy = _strategies.Where(x => x.CanHandle(context, args)).OrderBy(x => x.Priority).FirstOrDefault();


        strategy.Update(context, args);
    }
}
