// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("migration-add")]
public class MigrationAddRequest : IRequest
{
    [Option('n')]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class MigrationAddRequestHandler : IRequestHandler<MigrationAddRequest>
{
    private readonly ILogger<MigrationAddRequestHandler> logger;
    private readonly ICommandService commandService;
    private readonly IFileProvider fileProvider;

    public MigrationAddRequestHandler(
        ILogger<MigrationAddRequestHandler> logger,
        ICommandService commandService,
        IFileProvider fileProvider)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    public async Task Handle(MigrationAddRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(MigrationAddRequestHandler));

        var projectPath = fileProvider.Get("*.csproj", request.Directory);

        var projectName = Path.GetFileNameWithoutExtension(projectPath);

        var serviceName = projectName.Split('.').First();

        var schema = serviceName.Replace("Service", string.Empty);

        commandService.Start($"dotnet ef migrations add {schema}_{request.Name}", request.Directory);

        commandService.Start("dotnet ef database update", request.Directory);
    }
}
