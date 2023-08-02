// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Entities.Aggregate;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Core.Models.Artifacts;

public class AggregateArtifactsGenerationStrategy : ArtifactGenerationStrategyBase<AggregatesModel>
{
    private readonly IFileSystem _fileSystem;
    public AggregateArtifactsGenerationStrategy(IServiceProvider serviceProvider, IFileSystem fileSystem)
        : base(serviceProvider)
    {

        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public override void Create(IArtifactGenerator artifactGenerator, AggregatesModel model, dynamic context = null)
    {
        var aggregateDirectory = $"{model.Directory}{Path.DirectorySeparatorChar}{model.Aggregate.Name}Aggregate";

        _fileSystem.CreateDirectory(aggregateDirectory);

        _fileSystem.CreateDirectory($"{aggregateDirectory}{Path.DirectorySeparatorChar}Commands");

        _fileSystem.CreateDirectory($"{aggregateDirectory}{Path.DirectorySeparatorChar}Queries");

        artifactGenerator
            .CreateFor(new ObjectFileModel<ClassModel>(model.Aggregate, model.Aggregate.UsingDirectives, model.Aggregate.Name, aggregateDirectory, "cs"));

        artifactGenerator
            .CreateFor(new ObjectFileModel<ClassModel>(model.AggregateDto, model.AggregateDto.UsingDirectives, model.AggregateDto.Name, aggregateDirectory, "cs"));

        artifactGenerator
            .CreateFor(new ObjectFileModel<ClassModel>(model.AggregateExtensions, model.AggregateExtensions.UsingDirectives, model.AggregateExtensions.Name, aggregateDirectory, "cs"));


        foreach (var query in model.Queries)
        {
            artifactGenerator
                .CreateFor(
                new ObjectFileModel<QueryModel>(
                    query,
                    query.UsingDirectives,
                    query.Name,
                    $"{aggregateDirectory}{Path.DirectorySeparatorChar}Queries",
                    "cs"), new { Entity = model.Aggregate });
        }

        foreach (var command in model.Commands)
        {
            artifactGenerator
                .CreateFor(
                new ObjectFileModel<CommandModel>(
                    command,
                    command.UsingDirectives,
                    command.Name,
                    $"{aggregateDirectory}{Path.DirectorySeparatorChar}Commands",
                    "cs"), new { Entity = model.Aggregate });
        }

    }
}

