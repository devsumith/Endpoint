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
        public static void Configure(IServiceCollection services, string[] plugins)
        {
            services.AddMediatR(typeof(Endpoint.Core.Constants), typeof(Dependencies), typeof(Marker));
            services.AddSharedServices();
            services.AddCoreServices();

            //https://stackoverflow.com/questions/31859267/load-nuget-dependencies-at-runtime

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

            PluginLoadContext loadContext = new (pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

    }

}
