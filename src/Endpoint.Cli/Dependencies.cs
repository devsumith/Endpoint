using Endpoint.Application;
using Endpoint.Application.Plugin.Spa;
using Endpoint.Core;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Endpoint.Cli
{
    public static class Dependencies
    {
        public static void Configure(IServiceCollection services, Assembly[] pluginAssemblies)
        {
            services.AddMediatR(typeof(Endpoint.Core.Constants), typeof(Dependencies), typeof(Marker));
            services.AddSharedServices();
            services.AddCoreServices();
            services.AddMediatR(typeof(SpaPluginSolutionTemplateGeneratedNotificationsHandler));
        }
    }
}
