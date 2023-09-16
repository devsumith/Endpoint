// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("signalr-hub-client-create")]
public class SignalRHubClientCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SignalRHubClientCreateRequestHandler : IRequestHandler<SignalRHubClientCreateRequest>
{
    private readonly ILogger<SignalRHubClientCreateRequestHandler> logger;
    private readonly ICommandService commandService;
    private readonly IFileFactory fileFactory;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly INamingConventionConverter namingConventionConverter;

    public SignalRHubClientCreateRequestHandler(
        ILogger<SignalRHubClientCreateRequestHandler> logger,
        IFileFactory fileFactory,
        ICommandService commandService,
        IArtifactGenerator artifactGenerator,
        INamingConventionConverter namingConventionConverter)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public async Task Handle(SignalRHubClientCreateRequest request, CancellationToken cancellationToken)
    {
        var nameSnakeCase = namingConventionConverter.Convert(NamingConvention.SnakeCase, request.Name);

        logger.LogInformation("Handled: {0}", nameof(SignalRHubClientCreateRequestHandler));

        var tokens = new TokensBuilder().With("name", request.Name).Build();

        foreach (var model in new FileModel[]
        {
            fileFactory.CreateTemplate("Angular.Services.HubClientService.Service", $"{nameSnakeCase}-hub-client.service", request.Directory, ".ts", tokens: tokens),
            fileFactory.CreateTemplate("Angular.Services.HubClientService.Spec", $"{nameSnakeCase}-hub-client.service.spec", request.Directory, ".ts", tokens: tokens),
        })
        {
            await artifactGenerator.GenerateAsync(model);
        }
    }
}
