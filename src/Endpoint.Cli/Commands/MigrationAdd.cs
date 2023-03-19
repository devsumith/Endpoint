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
using System.Linq;

namespace Endpoint.Cli.Commands;


[Verb("migration-add")]
public class MigrationAddRequest : IRequest {
    [Option('n')]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class MigrationAddRequestHandler : IRequestHandler<MigrationAddRequest>
{
    private readonly ILogger<MigrationAddRequestHandler> _logger;
    private readonly ICommandService _commandService;
    private readonly IFileProvider _fileProvider;

    public MigrationAddRequestHandler(
        ILogger<MigrationAddRequestHandler> logger,
        ICommandService commandService,
        IFileProvider fileProvider
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    public async Task Handle(MigrationAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(MigrationAddRequestHandler));

        var projectPath = _fileProvider.Get("*.csproj", request.Directory);

        var projectName = Path.GetFileNameWithoutExtension(projectPath);

        var serviceName = projectName.Split('.').First();

        var schema = serviceName.Replace("Service", string.Empty);

        _commandService.Start($"dotnet ef migrations add {schema}_{request.Name}", request.Directory);

        _commandService.Start("dotnet ef database update", request.Directory);
    }
}
