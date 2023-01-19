using CommandLine;
using Endpoint.Core.Models.Artifacts.Projects.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("db-context-add")]
public class DbContextAddRequest : IRequest<Unit> {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class DbContextAddRequestHandler : IRequestHandler<DbContextAddRequest, Unit>
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

    public async Task<Unit> Handle(DbContextAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(DbContextAddRequestHandler));

        _infrastructureProjectService.DbContextAdd(request.Directory);

        return new();
    }
}