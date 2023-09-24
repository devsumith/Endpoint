// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Folders.Factories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;

[Verb("ng-domain-model-create")]
public class AngularDomainModelCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('p', "properties")]
    public string Properties { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularDomainModelCreateRequestHandler : IRequestHandler<AngularDomainModelCreateRequest>
{
    private readonly ILogger<AngularDomainModelCreateRequestHandler> logger;
    private readonly IFolderFactory folderFactory;
    private readonly IArtifactGenerator artifactGenerator;

    public AngularDomainModelCreateRequestHandler(
        ILogger<AngularDomainModelCreateRequestHandler> logger,
        IFolderFactory folderFactory,
        IArtifactGenerator artifactGenerator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.folderFactory = folderFactory ?? throw new ArgumentNullException(nameof(folderFactory));
    }

    public async Task Handle(AngularDomainModelCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(AngularDomainModelCreateRequestHandler));

        if (string.IsNullOrEmpty(request.Properties))
        {
            request.Properties = $"{request.Name}Id:string";
        }

        var model = await folderFactory.CreateAngularDomainModelAsync(request.Name, request.Properties);

        await artifactGenerator.GenerateAsync(model);
    }
}
