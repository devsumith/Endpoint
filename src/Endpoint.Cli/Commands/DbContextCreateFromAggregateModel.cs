// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Entities;
using Endpoint.Core.Syntax.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("db-context-create-from-aggregate-model")]
public class DbContextCreateFromAggregatesModelRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class DbContextCreateFromAggregatesModelRequestHandler : IRequestHandler<DbContextCreateFromAggregatesModelRequest>
{
    private readonly ILogger<DbContextCreateFromAggregatesModelRequestHandler> logger;
    private readonly IFileProvider fileProvider;
    private readonly INamingConventionConverter namingConventionConverter;
    private readonly IArtifactGenerator artifactGenerator;

    public DbContextCreateFromAggregatesModelRequestHandler(
        ILogger<DbContextCreateFromAggregatesModelRequestHandler> logger,
        IFileProvider fileProvider,
        INamingConventionConverter namingConventionConverter,
        IArtifactGenerator artifactGenerator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(DbContextCreateFromAggregatesModelRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(DbContextCreateFromAggregatesModelRequestHandler));

        var entities = new List<EntityModel>();

        var projectDirectory = Path.GetDirectoryName(fileProvider.Get("*.csproj", request.Directory));

        bool isInsideCoreProject = projectDirectory.EndsWith(".Core");

        var serviceName = Path.GetFileNameWithoutExtension(projectDirectory).Remove(isInsideCoreProject ? ".Core" : ".Infrastructure");

        var aggregatesModelDirectory = isInsideCoreProject ? $"{projectDirectory}{Path.DirectorySeparatorChar}AggregatesModel" : null;

        foreach (var folder in Directory.GetDirectories(aggregatesModelDirectory))
        {
            var folderName = Path.GetFileNameWithoutExtension(folder);

            if (folderName.EndsWith("Aggregate"))
            {
                entities.Add(new EntityModel(folderName.Remove("Aggregate")));
            }
        }

        var classModel = new DbContextModel(namingConventionConverter, $"{serviceName}DbContext", entities, serviceName);

        var interfaceModel = classModel.ToInterface();

        Directory.CreateDirectory($"{projectDirectory.Replace("Core", "Infrastructure")}{Path.DirectorySeparatorChar}Data");

        await artifactGenerator.GenerateAsync(
            new CodeFileModel<InterfaceModel>(
                interfaceModel,
                interfaceModel.Usings,
                interfaceModel.Name,
                projectDirectory,
                ".cs"));

        await artifactGenerator.GenerateAsync(
            new CodeFileModel<ClassModel>(
                classModel,
                classModel.Usings,
                classModel.Name,
                $"{projectDirectory.Replace("Core", "Infrastructure")}{Path.DirectorySeparatorChar}Data",
                ".cs"));
    }
}
