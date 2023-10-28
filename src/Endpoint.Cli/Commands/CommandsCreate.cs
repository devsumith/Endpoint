// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Folders.Factories;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Documents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("commands-create")]
public class CommandsCreateRequest : IRequest
{
    [Option('a', "aggregateName")]
    public string Aggregate { get; set; } = "ToDo";

    [Option('p', "properties")]
    public string Properties { get; set; } = "Name:string,IsComplete:bool";

    [Option('r', "root-namespace")]
    public string RootNamespace { get; set; }

    [Option('d')]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class CommandsCreateRequestHandler : IRequestHandler<CommandsCreateRequest>
{
    private readonly IFolderFactory folderFactory;
    private readonly IClassFactory classFactory;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly IContext context;
    private readonly ILogger<CommandsCreateRequestHandler> logger;

    public CommandsCreateRequestHandler(IFolderFactory folderFactory, IArtifactGenerator artifactGenerator, IClassFactory classFactory, ILogger<CommandsCreateRequestHandler> logger, IContext context)
    {
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.folderFactory = folderFactory ?? throw new ArgumentNullException(nameof(folderFactory));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(CommandsCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating Commands. {aggregate}", request.Aggregate);

        context.Set(new DocumentModel()
        {
            RootNamespace = request.RootNamespace,
        });

        var aggregate = await classFactory.CreateEntityAsync(request.Aggregate, request.Properties);

        var model = await folderFactory.CreateAggregateCommandsAsync(aggregate, request.Directory);

        await artifactGenerator.GenerateAsync(model);
    }
}
