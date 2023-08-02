
using Endpoint.Core.Options;


namespace Endpoint.Core.Strategies;

public interface IAdditionalResourceGenerator
{
    void CreateFor(AddResourceOptions options);
}

