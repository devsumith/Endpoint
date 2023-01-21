using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.Services;

public interface IDependencyInjectionService
{
     void Add(string interfaceName, string className, string directory, ServiceLifetime? serviceLifetime = null);

    void AddHosted(string hostedServiceName, string directory);
    void AddConfigureServices(string layer, string directory);
}

