using Endpoint.Application;
using Endpoint.Core;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Cli
{
    public static class Dependencies
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddMediatR(typeof(CoreConstants), typeof(ApplicationConstants));
            services.AddSharedServices();
            services.AddCoreServices();
        }
    }
}
