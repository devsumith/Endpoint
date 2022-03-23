using CommandLine;
using Endpoint.Core.Generators;
using Endpoint.Core.Options;
using Endpoint.Core.Strategies.Global;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Core.Commands
{
    internal class Default
    {
        [Verb("default")]
        internal class Request : CreateEndpointOptions, IRequest<Unit>
        {
            [Option("port")]
            public new int? Port { get; set; }

            [Option("properties")]
            public new string Properties { get; set; }

            [Option("name")]
            public new string Name { get; set; }

            [Option("resource")]
            public new string Resource { get; set; }

            [Option("monolith")]
            public new bool? Monolith { get; set; }

            [Option("minimal")]
            public new bool? Minimal { get; set; }

            [Option("db-context-name")]
            public new string DbContextName { get; set; }
            
            [Option("short-ids")]
            public new bool? ShortIdPropertyName { get; set; }
            
            [Option("numeric-ids")]
            public new bool? NumericIdPropertyDataType { get; set; }
                        
            [Option("directory")]
            public new string Directory { get; set; } = Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IEndpointGenerationStrategyFactory _endpointGenerationStrategyFactory;
            private readonly ILogger _logger;
            private readonly IConfiguration _configuration;

            public Handler(
                IEndpointGenerationStrategyFactory endpointGenerationStrategyFactory, 
                ILogger logger,
                IConfiguration configuration)
            {
                _endpointGenerationStrategyFactory = endpointGenerationStrategyFactory; ;
                _logger = logger;
                _configuration = configuration;
            }

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Default Handler");

                request.Name ??= _configuration["Default:Name"];

                request.Port ??= int.Parse(_configuration["Default:Port"]);
                
                request.Properties ??= _configuration["Default:Properties"];
                
                request.Resource ??= _configuration["Default:Resource"];
                
                request.Monolith ??= bool.Parse(_configuration["Default:Monolith"]);
                
                request.Minimal ??= bool.Parse(_configuration["Default:IsMinimalApi"]);
                
                request.DbContextName ??= _configuration["Default:DbContextName"];
                
                request.ShortIdPropertyName ??= bool.Parse(_configuration["Default:ShortIdPropertyName"]);
                
                request.NumericIdPropertyDataType ??= bool.Parse(_configuration["Default:NumericIdPropertyDataType"]);
                
                try
                {
                    int retries = 0;

                    string name = request.Name;

                    string originalName = request.Name;

                    while (true)
                    {
                        if (!Directory.Exists($"{request.Directory}{Path.DirectorySeparatorChar}{name}"))
                        {
                            request.Name = name;

                            EndpointGenerator.Generate(request, _endpointGenerationStrategyFactory);

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
