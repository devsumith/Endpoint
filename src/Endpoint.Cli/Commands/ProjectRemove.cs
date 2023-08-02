// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using Endpoint.Core.Services;

namespace Endpoint.Cli.Commands;

[Verb("project-remove")]
public class ProjectRemoveRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ProjectRemoveRequestHandler : IRequestHandler<ProjectRemoveRequest>
{
    private readonly ILogger<ProjectRemoveRequestHandler> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly ICommandService _commandService;

    public ProjectRemoveRequestHandler(
        ILogger<ProjectRemoveRequestHandler> logger,
        ICommandService commandService,
        IFileProvider fileProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(ProjectRemoveRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ProjectRemoveRequestHandler));

        var solutionPath = _fileProvider.Get("*.sln", request.Directory);

        var solutionDirectory = Path.GetDirectoryName(solutionPath);

        var solutionFileName = Path.GetFileNameWithoutExtension(solutionPath);

        foreach (var path in Directory.GetFiles(request.Directory, "*.csproj", SearchOption.AllDirectories))
        {
            _commandService.Start($"dotnet sln {solutionFileName} remove {path}", solutionDirectory);
        }
    }
}
