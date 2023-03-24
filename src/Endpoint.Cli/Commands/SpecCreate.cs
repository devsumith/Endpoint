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

namespace Endpoint.Cli.Commands;


[Verb("spec-create")]
public class SpecCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SpecCreateRequestHandler : IRequestHandler<SpecCreateRequest>
{
    private readonly ILogger<SpecCreateRequestHandler> _logger;
    private readonly INamingConventionConverter _namingCoventionConverter;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    public SpecCreateRequestHandler(
        ILogger<SpecCreateRequestHandler> logger,
        INamingConventionConverter namingCoventionConverter,
        IFileModelFactory fileModelFactory,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _namingCoventionConverter = namingCoventionConverter ?? throw new ArgumentNullException(nameof(namingCoventionConverter));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
    }

    public async Task Handle(SpecCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SpecCreateRequestHandler));

        var fileName = _namingCoventionConverter.Convert(NamingConvention.SnakeCase, request.Name.Remove("Service"));

        if(request.Name.EndsWith("Service"))
        {
            fileName = $"{fileName}.service";
        }

        var model = _fileModelFactory.CreateTemplate("Angular.Services.Default.Spec", $"{fileName}.spec", request.Directory, "ts", tokens: new TokensBuilder()
            .With("name", request.Name)
            .With("fileName", fileName)
            .Build());

        _artifactGenerationStrategyFactory.CreateFor(model);

    }
}
