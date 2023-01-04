using Endpoint.Application;
using Endpoint.Core;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(CoreConstants), typeof(ApplicationConstants));

        return services;
    }
}
