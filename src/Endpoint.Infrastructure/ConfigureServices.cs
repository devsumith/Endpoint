using Endpoint.Core.Services;
using Endpoint.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<ITemplateLocator, TemplateLocator>();
    }
}
