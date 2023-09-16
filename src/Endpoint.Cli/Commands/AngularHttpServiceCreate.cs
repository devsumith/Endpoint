// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<AngularHttpServiceCreateRequestHandler> logger;
    private readonly IFileFactory fileFactory;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly INamingConventionConverter namingConventionConverter;

    public AngularHttpServiceCreateRequestHandler(
        ILogger<AngularHttpServiceCreateRequestHandler> logger,
        IFileFactory fileFactory,
        IArtifactGenerator artifactGenerator,
        INamingConventionConverter namingConventionConverter)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public async Task Handle(AngularHttpServiceCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(AngularHttpServiceCreateRequestHandler));

        var entityName = namingConventionConverter.Convert(NamingConvention.SnakeCase, request.EntityName);

        var model = fileFactory.CreateTemplate("http-service", $"{entityName}.service", request.Directory, ".ts", tokens: new TokensBuilder()
            .With("entityName", request.EntityName)
            .Build());

        await artifactGenerator.GenerateAsync(model);
    }
}
