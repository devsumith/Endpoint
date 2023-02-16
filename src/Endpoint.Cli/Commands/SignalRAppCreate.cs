// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Models.WebArtifacts.Services;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Cli.Commands;


[Verb("signalr-app-create")]
public class SignalRAppCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SignalRAppCreateRequestHandler : IRequestHandler<SignalRAppCreateRequest>
{
    private readonly ILogger<SignalRAppCreateRequestHandler> _logger;
    private readonly ISolutionService _solutionService;
    private readonly IAngularService _angularService;
    private readonly ISolutionModelFactory _solutionModelFactory;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly INamingConventionConverter _namingConventionConverter;

    public SignalRAppCreateRequestHandler(
        ILogger<SignalRAppCreateRequestHandler> logger,
        ISolutionService solutionService,
        IAngularService angularService,
        ISolutionModelFactory solutionModelFactory,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        INamingConventionConverter namingConventionConverter)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        _solutionModelFactory = solutionModelFactory;
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter)); ;
    }

    public async Task Handle(SignalRAppCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SignalRAppCreateRequestHandler));

        var solutionModel = _solutionModelFactory.Create(request.Name, $"{request.Name}.Api", "webapi", string.Empty, request.Directory);

        _artifactGenerationStrategyFactory.CreateFor(solutionModel);

        var temporaryAppName = $"{_namingConventionConverter.Convert(NamingConvention.SnakeCase,request.Name)}_app";

        _angularService.CreateWorkspace(temporaryAppName, "app", "application", "app", solutionModel.SrcDirectory, false);

        System.IO.Directory.Move(Path.Combine(solutionModel.SrcDirectory, temporaryAppName), Path.Combine(solutionModel.SrcDirectory, $"{request.Name}.App"));

    }
}
