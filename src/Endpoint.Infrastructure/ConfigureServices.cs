using Endpoint.Core.Services;
using Endpoint.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<ITemplateLocator, EmbeddedResourceTemplateLocatorBase<Marker>>();
    }
}
