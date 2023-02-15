// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Generators;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Options;
using Endpoint.Core.Strategies.Common;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nelibur.ObjectMapper;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Core.Commands;

public class Default
{
    [Verb("default")]
    public class Request : IRequest
    {
        [Option("port")]
        public int? Port { get; set; }

        [Option("properties")]
        public string Properties { get; set; }

        [Option("name")]
        public string Name { get; set; }

        [Option("resource")]
        public string Resource { get; set; }

        [Option("monolith")]
        public bool? Monolith { get; set; }

        [Option("minimal")]
        public bool? Minimal { get; set; }

        [Option("db-context-name")]
        public string DbContextName { get; set; }
        
        [Option("short-ids")]
        public bool? ShortIdPropertyName { get; set; }
        
        [Option("numeric-ids")]
        public bool? NumericIdPropertyDataType { get; set; }

        [Option("vs-code")]
        public bool? VsCode { get; set; }

        [Option("directory")]
        public string Directory { get; set; } = Environment.CurrentDirectory;
    }

    public class Handler : IRequestHandler<Request>
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

        public async Task Handle(Request request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Default Handler");

            request.Name ??= _configuration["Default:Name"];

            request.Port ??= int.Parse(_configuration["Default:Port"]);
            
            request.Properties ??= _configuration["Default:Properties"];
            
            request.Resource ??= _configuration["Default:Resource"];
            
            request.Monolith ??= bool.Parse(_configuration["Default:Monolith"]);
            
            request.Minimal ??= bool.Parse(_configuration["Default:Minimal"]);
            
            request.DbContextName ??= _configuration["Default:DbContextName"] ??= $"{((SyntaxToken)request.Name).PascalCase}DbContext";
            
            request.ShortIdPropertyName ??= bool.Parse(_configuration["Default:ShortIdPropertyName"]);
            
            request.NumericIdPropertyDataType ??= bool.Parse(_configuration["Default:NumericIdPropertyDataType"]);

            request.VsCode ??= bool.Parse(_configuration["Default:VsCode"]);

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

                        var options = TinyMapper.Map<CreateEndpointOptions>(request);

                        EndpointGenerator.Generate(options, _endpointGenerationStrategyFactory);

                
                    }

                    retries++;

                    name = $"{originalName}{retries}";
                }
            } 
            catch(Exception e)
            {
                _logger.LogError(e.Message);

        
            }
        }
    }
}

