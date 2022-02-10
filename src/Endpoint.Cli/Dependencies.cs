using Endpoint.Core.Core;
using Endpoint.SharedKernal;
using Endpoint.SharedKernal.Plugins;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

namespace Endpoint.Cli
{
    public static class Dependencies
    {
        public static void Configure(IServiceCollection services, string[] plugins)
        {
            services.AddMediatR(typeof(Constants), typeof(Dependencies), typeof(Marker));
            services.AddSharedServices();
            services.AddCoreServices();

            foreach (var plugin in plugins)
            {
                var pluginPath = @$"Plugins\Endpoint.Application.Plugin.{plugin}\bin\Debug\net5.0\Endpoint.Application.Plugin.{plugin}.dll";

                Assembly pluginAssembly = LoadPlugin(pluginPath);

                services.AddMediatR(pluginAssembly);
            }
        }

        static Assembly LoadPlugin(string relativePath)
        {
            string root = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));

            string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));

            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

    }

}
