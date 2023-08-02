

namespace Endpoint.Core.Strategies.Api;

public interface IWebApplicationBuilderGenerationStrategy
{
    string Create(string @namespace, string dbContextName);
}

