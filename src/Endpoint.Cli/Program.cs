using CommandLine;
using Endpoint.Application;
using Endpoint.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using System.Linq;

namespace Endpoint.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            Log.Information("Starting Endpoint");

            var container = BuildContainer();

            var mediator = container.GetService<IMediator>();

            var configuration = container.GetRequiredService<IConfiguration>();

            ProcessArgs(mediator, configuration, args);
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

        public static ServiceProvider BuildContainer()
        {
            var services = new ServiceCollection();

            Dependencies.Configure(services);

            return services.BuildServiceProvider();
        }

        public static void ProcessArgs(IMediator mediator, IConfiguration configuration, string[] args)
        {
            if (args.Length == 0 || args[0].StartsWith("-"))
            {
                args = new string[1] { configuration[CoreConstants.EnvironmentVariables.DefaultCommand] }.Concat(args).ToArray();
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
}
