// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Models.Artifacts.Projects.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("db-context-add")]
public class DbContextAddRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class DbContextAddRequestHandler : IRequestHandler<DbContextAddRequest>
{
    private readonly ILogger<DbContextAddRequestHandler> _logger;
    private readonly IInfrastructureProjectService _infrastructureProjectService;

    public DbContextAddRequestHandler(
        ILogger<DbContextAddRequestHandler> logger,
        IInfrastructureProjectService infrastructureProjectService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _infrastructureProjectService = infrastructureProjectService ?? throw new ArgumentNullException(nameof(infrastructureProjectService));
    }

    public async Task Handle(DbContextAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(DbContextAddRequestHandler));

        _infrastructureProjectService.DbContextAdd(request.Directory);


    }
}
