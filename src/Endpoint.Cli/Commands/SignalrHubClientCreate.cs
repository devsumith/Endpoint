// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Services;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;

namespace Endpoint.Cli.Commands;


[Verb("signalr-hub-client-create")]
public class SignalRHubClientCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SignalRHubClientCreateRequestHandler : IRequestHandler<SignalRHubClientCreateRequest>
{
    private readonly ILogger<SignalRHubClientCreateRequestHandler> _logger;
    private readonly ICommandService _commandService;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly INamingConventionConverter _namingConventionConverter;

    public SignalRHubClientCreateRequestHandler(
        ILogger<SignalRHubClientCreateRequestHandler> logger,
        IFileModelFactory fileModelFactory,
        ICommandService commandService,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        INamingConventionConverter namingConventionConverter)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public async Task Handle(SignalRHubClientCreateRequest request, CancellationToken cancellationToken)
    {
        var nameSnakeCase = _namingConventionConverter.Convert(NamingConvention.SnakeCase, request.Name);

        _logger.LogInformation("Handled: {0}", nameof(SignalRHubClientCreateRequestHandler));

        var tokens = new TokensBuilder().With("name", request.Name).Build();

        foreach(var model in new FileModel[]
        {
            _fileModelFactory.CreateTemplate("Angular.Services.HubClientService.Service", $"{nameSnakeCase}-hub-client.service", request.Directory, "ts", tokens: tokens),
            _fileModelFactory.CreateTemplate("Angular.Services.HubClientService.Spec", $"{nameSnakeCase}-hub-client.service.spec", request.Directory, "ts", tokens: tokens)
        })
        {
            _artifactGenerationStrategyFactory.CreateFor(model);
        }        
    }
}
