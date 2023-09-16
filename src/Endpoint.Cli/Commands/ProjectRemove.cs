// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("project-remove")]
public class ProjectRemoveRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ProjectRemoveRequestHandler : IRequestHandler<ProjectRemoveRequest>
{
    private readonly ILogger<ProjectRemoveRequestHandler> logger;
    private readonly IFileProvider fileProvider;
    private readonly ICommandService commandService;

    public ProjectRemoveRequestHandler(
        ILogger<ProjectRemoveRequestHandler> logger,
        ICommandService commandService,
        IFileProvider fileProvider)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(ProjectRemoveRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(ProjectRemoveRequestHandler));

        var solutionPath = fileProvider.Get("*.sln", request.Directory);

        var solutionDirectory = Path.GetDirectoryName(solutionPath);

        var solutionFileName = Path.GetFileNameWithoutExtension(solutionPath);

        foreach (var path in Directory.GetFiles(request.Directory, "*.csproj", SearchOption.AllDirectories))
        {
            commandService.Start($"dotnet sln {solutionFileName} remove {path}", solutionDirectory);
        }
    }
}
