// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Syntax;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Commands;

[Verb("default")]
public class DefaultRequest : IRequest
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

public class DefaultHandler : IRequestHandler<DefaultRequest>
{
    private readonly ILogger<DefaultHandler> logger;
    private readonly IConfiguration configuration;

    public DefaultHandler(
        ILogger<DefaultHandler> logger,
        IConfiguration configuration)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task Handle(DefaultRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Default Handler");

        request.Name ??= configuration["Default:Name"];

        request.Port ??= int.Parse(configuration["Default:Port"]);

        request.Properties ??= configuration["Default:Properties"];

        request.Resource ??= configuration["Default:Resource"];

        request.Monolith ??= bool.Parse(configuration["Default:Monolith"]);

        request.Minimal ??= bool.Parse(configuration["Default:Minimal"]);

        request.DbContextName ??= configuration["Default:DbContextName"] ??= $"{((SyntaxToken)request.Name).PascalCase}DbContext";

        request.ShortIdPropertyName ??= bool.Parse(configuration["Default:ShortIdPropertyName"]);

        request.NumericIdPropertyDataType ??= bool.Parse(configuration["Default:NumericIdPropertyDataType"]);

        request.VsCode ??= bool.Parse(configuration["Default:VsCode"]);

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

                    // var options = TinyMapper.Map<CreateEndpointOptions>(request);

                    // EndpointGenerator.Generate(options, _endpointGenerator);
                }

                retries++;

                name = $"{originalName}{retries}";
            }
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }
    }
}
