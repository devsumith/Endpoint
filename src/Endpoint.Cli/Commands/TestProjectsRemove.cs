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

[Verb("test-projects-remove")]
public class TestProjectsRemoveRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class TestProjectsRemoveRequestHandler : IRequestHandler<TestProjectsRemoveRequest>
{
    private readonly ILogger<TestProjectsRemoveRequestHandler> logger;
    private readonly IFileProvider fileProvider;
    private readonly ICommandService commandService;

    public TestProjectsRemoveRequestHandler(
        ILogger<TestProjectsRemoveRequestHandler> logger,
        IFileProvider fileProvider,
        ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.commandService = commandService;
        this.fileProvider = fileProvider;
    }

    public async Task Handle(TestProjectsRemoveRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(TestProjectsRemoveRequestHandler));

        var solutionPath = fileProvider.Get("*.sln", request.Directory);

        var solutionFileName = Path.GetFileName(solutionPath);

        var solutionDirectory = Path.GetDirectoryName(solutionPath);

        foreach (var projectPath in Directory.GetFiles(solutionDirectory, "*.Tests.csproj", SearchOption.AllDirectories))
        {
            commandService.Start($"dotnet sln {solutionFileName} remove {projectPath}", solutionDirectory);
        }
    }
}
