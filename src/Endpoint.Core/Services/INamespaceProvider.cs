namespace Endpoint.Core.Services;

public interface INamespaceProvider
{
    string Get(string directory, int depth = 0);
}
