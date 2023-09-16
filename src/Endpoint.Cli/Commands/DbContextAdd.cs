// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Projects.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("db-context-add")]
public class DbContextAddRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class DbContextAddRequestHandler : IRequestHandler<DbContextAddRequest>
{
    private readonly ILogger<DbContextAddRequestHandler> logger;
    private readonly IInfrastructureProjectService infrastructureProjectService;

    public DbContextAddRequestHandler(
        ILogger<DbContextAddRequestHandler> logger,
        IInfrastructureProjectService infrastructureProjectService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.infrastructureProjectService = infrastructureProjectService ?? throw new ArgumentNullException(nameof(infrastructureProjectService));
    }

    public async Task Handle(DbContextAddRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(DbContextAddRequestHandler));

        infrastructureProjectService.DbContextAdd(request.Directory);
    }
}
