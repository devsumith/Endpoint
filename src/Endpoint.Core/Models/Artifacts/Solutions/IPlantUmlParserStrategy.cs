namespace Endpoint.Core.Models.Artifacts.Solutions;

public interface IPlantUmlParserStrategy
{
    bool CanHandle(string plantUml);
    int Priority { get; }
    object Create(string plantUml, dynamic context = null);
}
