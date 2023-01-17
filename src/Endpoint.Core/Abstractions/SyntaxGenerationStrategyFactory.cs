using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Abstractions;

public class SyntaxGenerationStrategyFactory : ISyntaxGenerationStrategyFactory
{
    private readonly IEnumerable<ISyntaxGenerationStrategy> _strategies;

    public SyntaxGenerationStrategyFactory(IEnumerable<ISyntaxGenerationStrategy> strategies)
    {
        _strategies = strategies;
    }
    public string CreateFor(object model, dynamic configuration = null)
    {
        var orderedStrategies = _strategies.OrderByDescending(x => x.Priority);

        var strategies = orderedStrategies.Where(x => x.CanHandle(model, configuration));

        var strategy = strategies.FirstOrDefault();

        return strategy.Create(model, configuration);
    }
}
