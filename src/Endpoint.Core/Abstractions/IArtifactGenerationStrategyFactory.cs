using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Abstractions;

public interface IArtifactGenerationStrategyFactory
{
    void CreateFor(object model, dynamic context = null);
}


public interface IArtifactUpdateStrategyFactory
{
    void CreateFor(dynamic context = null, params object[] args);
}


public class ArtifactUpdateStrategyFactory : IArtifactUpdateStrategyFactory
{
    private readonly IEnumerable<IArtifactUpdateStrategy> _strategies;

    public ArtifactUpdateStrategyFactory(IEnumerable<IArtifactUpdateStrategy> strategies)
    {
        _strategies = strategies;
    }
    public void CreateFor(dynamic context = null, params object[] args)
    {
        var strategy = _strategies.Where(x => x.CanHandle(context,args)).OrderBy(x => x.Priority).FirstOrDefault();


        strategy.Update(context, args);
    }
}