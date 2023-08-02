// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;

namespace Endpoint.Cli.Commands;


[Verb("ng-http-service-create")]
public class AngularHttpServiceCreateRequest : IRequest
{
    [Option('n', "entity-name")]
    public string EntityName { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularHttpServiceCreateRequestHandler : IRequestHandler<AngularHttpServiceCreateRequest>
{
    private readonly ILogger<AngularHttpServiceCreateRequestHandler> _logger;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly INamingConventionConverter _namingConventionConverter;

    public AngularHttpServiceCreateRequestHandler(
        ILogger<AngularHttpServiceCreateRequestHandler> logger,
        IFileModelFactory fileModelFactory,
        IArtifactGenerator artifactGenerator,
        INamingConventionConverter namingConventionConverter)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public async Task Handle(AngularHttpServiceCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AngularHttpServiceCreateRequestHandler));

        var entityName = _namingConventionConverter.Convert(NamingConvention.SnakeCase, request.EntityName);

        var model = _fileModelFactory.CreateTemplate("http-service", $"{entityName}.service", request.Directory, "ts", tokens: new TokensBuilder()
            .With("entityName", request.EntityName)
            .Build());

        _artifactGenerator.CreateFor(model);
    }
}
