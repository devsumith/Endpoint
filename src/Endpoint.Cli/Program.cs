using CommandLine;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Endpoint.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var pluginsOptionValue = _resolvePluginsOptionValue(args);

            var mediator = BuildContainer(pluginsOptionValue).GetService<IMediator>();

            ProcessArgs(mediator, args);
        }

        private static string[] _resolvePluginsOptionValue(string[] args)
        {
            var result = _createParser().ParseArguments<PluginsOption>(args);

            var pluginsOptionValue = ((Parsed<PluginsOption>)result).Value;

            return pluginsOptionValue.Plugins?.Split(',') ?? new string[0];
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

        public static ServiceProvider BuildContainer(string[] args)
        {
            var services = new ServiceCollection();

            Dependencies.Configure(services, args);

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
    }

    public class PluginsOption
    {
        [Option("plugins", Required = false)]
        public string Plugins { get; set; }
    }

}
