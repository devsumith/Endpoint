// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.Path;

namespace Endpoint.Cli.Commands;

[Verb("puml-image-create")]
public class PlantUmlImageCreateRequest : IRequest
{
    [Option('i', "input-path", Required = true)]
    public string InputPath { get; set; }

    [Option('p', "plantuml-path")]
    public string PlantUmlPath { get; set; } = Combine(GetFolderPath(UserProfile), ".dotnet", "tools", "plantuml.jar");

    [Option('j', "java-path")]
    public string JavaPath { get; set; } = $"\"{Combine(GetFolderPath(ProgramFilesX86), "Java", "jre-1.8", "bin", "java.exe")}\"";

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ImageCreateRequestHandler : IRequestHandler<PlantUmlImageCreateRequest>
{
    private readonly ILogger<ImageCreateRequestHandler> logger;
    private readonly ICommandService commandService;

    public ImageCreateRequestHandler(ILogger<ImageCreateRequestHandler> logger, ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(PlantUmlImageCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating Image", nameof(ImageCreateRequestHandler));

        commandService.Start($"{request.JavaPath} -jar {request.PlantUmlPath} {request.InputPath}", request.Directory);
    }
}