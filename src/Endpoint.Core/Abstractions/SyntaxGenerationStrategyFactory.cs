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
        var strategy = _strategies.Where(x => x.CanHandle(model)).OrderBy(x => x.Priority).FirstOrDefault();

        return strategy.Create(model);
    }
}
