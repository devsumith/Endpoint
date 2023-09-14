// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
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
    private readonly ILogger<WorkerCreateRequestHandler> _logger;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly IClassFactory _classFactory;

    public WorkerCreateRequestHandler(
        ILogger<WorkerCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator,
        IClassFactory classFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
    }

    public async Task Handle(WorkerCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating Worker. {name}", request.Name);

        var model = await _classFactory.CreateWorkerAsync(request.Name);

        var fileModel = new CodeFileModel<ClassModel>(model, model.Name, request.Directory, CSharpFile);

        await _artifactGenerator.GenerateAsync(fileModel);
    }
}
