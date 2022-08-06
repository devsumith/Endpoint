using Endpoint.Core;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CoreConstants), typeof(ApplicationConstants));

            return services;
        }
    }
}
