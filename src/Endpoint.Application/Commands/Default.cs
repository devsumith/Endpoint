using CommandLine;
using Endpoint.Core.Generators;
using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Global;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Core.Commands
{
    internal class Default
    {
        [Verb("default")]
        internal class Request : IRequest<Unit>
        {
            [Option('p', "port")]
            public int Port { get; set; } = 5000;

            [Option("properties", Required = false)]
            public string Properties { get; set; }

            [Option('n', "name")]
            public string Name { get; set; } = "DefaultEndpoint";

            [Option('r', "resource")]
            public string Resource { get; set; } = "Foo";

            [Option('m', "is-monolithic-architecture")]
            public bool Monolith { get; set; } = true;

            [Option('a', "minimal-api", Required = false)]
            public bool Minimal { get; set; }

            [Option("db-context-name")]
            public string DbContextName { get; set; }
            
            [Option('s', "short-id-propeperty-name")]
            public bool ShortIdPropertyName { get; set; }
            
            [Option('i', "numeric-id-property-data-type")]
            public bool NumericIdPropertyDataType { get; set; }
            
            [Option("plugins", Required = false)]
            public string Plugins { get; set; }
            
            [Option("prefix", Required = false)]
            public string Prefix { get; set; } = "app";

            [Option('d', "directory")]
            public string Directory { get; set; } = Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IEndpointGenerationStrategyFactory _endpointGenerationStrategyFactory;
            private readonly ISettingsProvider _settingsProvider;
            private readonly ILogger _logger;

            public Handler(
                IEndpointGenerationStrategyFactory endpointGenerationStrategyFactory, 
                ISettingsProvider settingsProvider,
                ILogger logger)
            {
                _endpointGenerationStrategyFactory = endpointGenerationStrategyFactory; ;
                _settingsProvider = settingsProvider;
                _logger = logger;
            }

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Default Handler");

                try
                {
                    int retries = 0;

                    string name = request.Name;

                    string originalName = request.Name;

                    while (true)
                    {
                        if (!Directory.Exists($"{request.Directory}{Path.DirectorySeparatorChar}{name}"))
                        {
                            var endpointDirectory = $"{request.Directory}{Path.DirectorySeparatorChar}{name}";

                            var settings = _settingsProvider.Get(endpointDirectory);

                            if (settings == null)
                            {
                                settings = new Settings(
                                    name,
                                    request.DbContextName,
                                    new AggregateRootModel(request.Resource, request.NumericIdPropertyDataType, request.ShortIdPropertyName, request.Properties),
                                    request.Directory,
                                    !request.Monolith,
                                    request.Plugins?.Split(',').ToList(),
                                    request.ShortIdPropertyName ? IdFormat.Short : IdFormat.Long,
                                    request.NumericIdPropertyDataType ? IdDotNetType.Int : IdDotNetType.Guid,
                                    request.Prefix,
                                    request.Minimal);
                            }

                            EndpointGenerator.Generate(settings, _endpointGenerationStrategyFactory);

                            return Task.FromResult(new Unit());
                        }

                        retries++;

                        name = $"{originalName}{retries}";
                    }
                } 
                catch(Exception e)
                {
                    _logger.LogError(e.Message);

                    return Task.FromResult(new Unit());
                }
            }
        }
    }
}
