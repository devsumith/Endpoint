namespace Endpoint.Core.Abstractions;

public interface ISyntaxGenerationStrategyFactory
{
    string CreateFor(object model, dynamic configuration = null);
}
