namespace Endpoint.Core.Models.Artifacts.Solutions;

public interface IPlantUmlParserStrategyFactory
{
    dynamic CreateFor(string plantUml, dynamic context = null);
}
