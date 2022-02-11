using Endpoint.Application;
using Endpoint.Core;
using Endpoint.Core.Plugins;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
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

            // TODO: Create an interface for plugins to add services to DI including MediatR

            foreach (var pluginAssembly in pluginAssemblies)
            {
                services.AddMediatR(pluginAssembly);
            }
        }



    }

}
