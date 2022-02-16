using Endpoint.Application;
using Endpoint.Application.Spa;
using Endpoint.Core;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Cli
{
    public static class Dependencies
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddMediatR(typeof(Endpoint.Core.CoreConstants), typeof(Dependencies), typeof(Marker));
            services.AddSharedServices();
            services.AddCoreServices();
            services.AddMediatR(typeof(SpaPluginSolutionTemplateGeneratedNotificationsHandler));
        }
    }
}
