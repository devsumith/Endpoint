// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("spec-create")]
public class SpecCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SpecCreateRequestHandler : IRequestHandler<SpecCreateRequest>
{
    private readonly ILogger<SpecCreateRequestHandler> logger;
    private readonly INamingConventionConverter namingCoventionConverter;
    private readonly IFileFactory fileFactory;
    private readonly IArtifactGenerator artifactGenerator;

    public SpecCreateRequestHandler(
        ILogger<SpecCreateRequestHandler> logger,
        INamingConventionConverter namingCoventionConverter,
        IFileFactory fileFactory,
        IArtifactGenerator artifactGenerator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.namingCoventionConverter = namingCoventionConverter ?? throw new ArgumentNullException(nameof(namingCoventionConverter));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(SpecCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(SpecCreateRequestHandler));

        var fileName = namingCoventionConverter.Convert(NamingConvention.SnakeCase, request.Name.Remove("Service"));

        if (request.Name.EndsWith("Service"))
        {
            fileName = $"{fileName}.service";
        }

        var model = fileFactory.CreateTemplate("Angular.Services.Default.Spec", $"{fileName}.spec", request.Directory, ".ts", tokens: new TokensBuilder()
            .With("name", request.Name)
            .With("fileName", fileName)
            .Build());

        await artifactGenerator.GenerateAsync(model);
    }
}
