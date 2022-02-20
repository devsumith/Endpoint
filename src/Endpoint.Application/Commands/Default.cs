using CommandLine;
using Endpoint.Core.Generators;
using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Global;
using MediatR;
using System.Collections.Generic;
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


            [Option('m', "isMonolithArchitecture")]
            public bool Monolith { get; set; } = true;


            [Option('a', "minimalapi", Required = false)]
            public bool Minimal { get; set; } = true;

            [Option("dbContextName")]
            public string DbContextName { get; set; }

            
            [Option('s', "shortIdPropertyName")]
            public bool ShortIdPropertyName { get; set; }

            
            [Option('i', "numericIdPropertyDataType")]
            public bool NumericIdPropertyDataType { get; set; }

            
            [Option("plugins", Required = false)]
            public string Plugins { get; set; }

            
            [Option("prefix", Required = false)]
            public string Prefix { get; set; } = "app";


            [Option('d', "directory")]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IEndpointGenerationStrategyFactory _workspaceGenerationStrategyFactory;
            private readonly ISettingsProvider _settingsProvider;

            public Handler(IEndpointGenerationStrategyFactory workspaceGenerationStrategyFactory, ISettingsProvider settingsProvider)
            {
                _workspaceGenerationStrategyFactory = workspaceGenerationStrategyFactory; ;
                _settingsProvider = settingsProvider;
            }
            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var endpointDirectory = $"{request.Directory}{Path.DirectorySeparatorChar}{request.Name}";

                var settings = _settingsProvider.Get(endpointDirectory);

                if (settings == null)
                {
                    settings = new Models.Settings(
                        request.Name, 
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

                EndpointGenerator.Generate(settings, _workspaceGenerationStrategyFactory);

                return Task.FromResult(new Unit());
            }
        }
    }
}
