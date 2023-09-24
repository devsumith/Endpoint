// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Folders.Factories;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Documents.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("commands-create")]
public class CommandCreateRequest : IRequest
{
    [Option('a', "aggregateName")]
    public string Aggregate { get; set; } = "ToDo";

    [Option('p', "properties")]
    public string Properties { get; set; } = "Name:string,IsComplete:bool";

    [Option('d')]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class CommandCreateRequestHandler : IRequestHandler<CommandCreateRequest>
{
    private readonly IFolderFactory folderFactory;
    private readonly IClassFactory classFactory;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly ILogger<CommandCreateRequestHandler> logger;

    public CommandCreateRequestHandler(IFolderFactory folderFactory, IArtifactGenerator artifactGenerator, IClassFactory classFactory, ILogger<CommandCreateRequestHandler> logger)
    {
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.folderFactory = folderFactory ?? throw new ArgumentNullException(nameof(folderFactory));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
    }

    public async Task Handle(CommandCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating Commands. {aggregate}", request.Aggregate);

        var aggregate = await classFactory.CreateEntityAsync(request.Aggregate, request.Properties);

        var model = await folderFactory.CreateAggregateCommandsAsync(aggregate, request.Directory);

        await artifactGenerator.GenerateAsync(model);
    }
}
