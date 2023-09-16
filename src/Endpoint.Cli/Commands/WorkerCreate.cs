// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using MediatR;
using Microsoft.Extensions.Logging;
using static Endpoint.Core.Constants.FileExtensions;

namespace Endpoint.Cli.Commands;

[Verb("worker-create")]
public class WorkerCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class WorkerCreateRequestHandler : IRequestHandler<WorkerCreateRequest>
{
    private readonly ILogger<WorkerCreateRequestHandler> logger;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly IClassFactory classFactory;

    public WorkerCreateRequestHandler(
        ILogger<WorkerCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator,
        IClassFactory classFactory)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
    }

    public async Task Handle(WorkerCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating Worker. {name}", request.Name);

        var model = await classFactory.CreateWorkerAsync(request.Name);

        var fileModel = new CodeFileModel<ClassModel>(model, model.Name, request.Directory, CSharpFile);

        await artifactGenerator.GenerateAsync(fileModel);
    }
}
