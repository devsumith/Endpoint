// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Cli.Commands;


[Verb("test-projects-remove")]
public class TestProjectsRemoveRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class TestProjectsRemoveRequestHandler : IRequestHandler<TestProjectsRemoveRequest>
{
    private readonly ILogger<TestProjectsRemoveRequestHandler> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly ICommandService _commandService;
    public TestProjectsRemoveRequestHandler(
        ILogger<TestProjectsRemoveRequestHandler> logger,
        IFileProvider fileProvider,
        ICommandService commandService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandService = commandService;
        _fileProvider = fileProvider;
    }

    public async Task Handle(TestProjectsRemoveRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(TestProjectsRemoveRequestHandler));

        var solutionPath = _fileProvider.Get("*.sln", request.Directory);

        var solutionFileName = Path.GetFileName(solutionPath);

        var solutionDirectory = Path.GetDirectoryName(solutionPath);

        foreach (var projectPath in Directory.GetFiles(solutionDirectory, "*.Tests.csproj", SearchOption.AllDirectories))
        {
            _commandService.Start($"dotnet sln {solutionFileName} remove {projectPath}", solutionDirectory);
        }

    }
}
