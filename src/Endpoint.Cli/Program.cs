using CommandLine;
using Endpoint.Core.Plugins;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Endpoint.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var pluginAssemblies = _resolvePluginsOptionValue(args);

            var mediator = BuildContainer(pluginAssemblies).GetService<IMediator>();

            ProcessArgs(mediator, args);
        }

        private static Assembly[] _resolvePluginsOptionValue(string[] args)
        {
            var result = _createParser().ParseArguments<PluginsOption>(args);

            var pluginsOptionValue = ((Parsed<PluginsOption>)result).Value;

            var pluginNames = pluginsOptionValue.Plugins?.Split(',') ?? new string[0];

            var assemblies = new List<Assembly>();

            foreach (var plugin in pluginNames)
            {
                //https://stackoverflow.com/questions/31859267/load-nuget-dependencies-at-runtime
                // Load from nuget?

                var pluginPath = @$"Plugins\Endpoint.Application.Plugin.{plugin}\bin\Debug\net5.0\Endpoint.Application.Plugin.{plugin}.dll";

                assemblies.Add(LoadPlugin(pluginPath));
            }

            return assemblies.ToArray();
        }

        private static Parser _createParser()
        {
            return new Parser(with =>
            {
                with.CaseSensitive = false;
                with.HelpWriter = Console.Out;
                with.IgnoreUnknownArguments = true;
            });
        }

        public static ServiceProvider BuildContainer(Assembly[] pluginAssemblies)
        {
            var services = new ServiceCollection();

            Dependencies.Configure(services, pluginAssemblies);

            return services.BuildServiceProvider();
        }

        public static void ProcessArgs(IMediator mediator, string[] args)
        {
            if (args.Length == 0 || args[0].StartsWith("-"))
            {
                args = new string[1] { "default" }.Concat(args).ToArray();
            }

            var verbs = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(type => type.GetCustomAttributes(typeof(VerbAttribute), true).Length > 0)
                .ToArray();

            _createParser().ParseArguments(args, verbs)
                .WithParsed(
                  (dynamic request) => mediator.Send(request));
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

            PluginLoadContext loadContext = new(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }
    }

    public class PluginsOption
    {
        [Option("plugins", Required = false)]
        public string Plugins { get; set; }
    }

}
