using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Services;
using Endpoint.Core.Models.Artifacts.Solutions;

namespace Endpoint.Cli.Commands;


[Verb("solution-create-from-plant-uml")]
public class SolutionCreateFromPlantUmlRequest : IRequest<Unit> {
    [Option('n',"name")]
    public string Name { get; set; }

    [Option('p')]
    public string PlantUmlSourcePath { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SolutionCreateFromPlantUmlRequestHandler : IRequestHandler<SolutionCreateFromPlantUmlRequest, Unit>
{
    private readonly ILogger<SolutionCreateFromPlantUmlRequestHandler> _logger;
    private readonly ISolutionService _solutionService;
    private readonly IClipboardService _clipboardService;
    private readonly ICommandService _commandService;
    public SolutionCreateFromPlantUmlRequestHandler(
        ILogger<SolutionCreateFromPlantUmlRequestHandler> logger,
        ISolutionService solutionService,
        IClipboardService clipboardService,
        ICommandService commandService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _clipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task<Unit> Handle(SolutionCreateFromPlantUmlRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SolutionCreateFromPlantUmlRequestHandler));

        var value = await _clipboardService.GetTextAsync(cancellationToken);

        var model = _solutionService.CreateFromPlantUml(value, request.Name, request.Directory);

        _commandService.Start($"start {model.SolutionPath}", model.Directory);

        return new();
    }
}