using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class PlantUmlParserStrategyFactory: IPlantUmlParserStrategyFactory
{
    private readonly IEnumerable<IPlantUmlParserStrategy> _strategies;

    public PlantUmlParserStrategyFactory(IEnumerable<IPlantUmlParserStrategy> strategies)
    {
        _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
    }

    public dynamic CreateFor(string plantUml, dynamic context = null)
    {
        var strategy = _strategies.Where(x => x.CanHandle(plantUml)).OrderBy(x => x.Priority).FirstOrDefault();

        
        return strategy == null ? null : strategy.Create(plantUml, context);
    }
}
