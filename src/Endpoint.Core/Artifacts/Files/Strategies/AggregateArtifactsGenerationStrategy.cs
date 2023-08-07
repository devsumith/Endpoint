// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Entities.Aggregate;
using System.IO;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class AggregateArtifactsGenerationStrategy : IArtifactGenerationStrategy<AggregatesModel>
{
    private readonly IFileSystem _fileSystem;
    public AggregateArtifactsGenerationStrategy(IServiceProvider serviceProvider, IFileSystem fileSystem)
    {

        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public int Priority => 0;

    public async Task GenerateAsync(IArtifactGenerator artifactGenerator, AggregatesModel model, dynamic context = null)
    {
        var aggregateDirectory = $"{model.Directory}{Path.DirectorySeparatorChar}{model.Aggregate.Name}Aggregate";

        _fileSystem.CreateDirectory(aggregateDirectory);

        _fileSystem.CreateDirectory($"{aggregateDirectory}{Path.DirectorySeparatorChar}Commands");

        _fileSystem.CreateDirectory($"{aggregateDirectory}{Path.DirectorySeparatorChar}Queries");

        await artifactGenerator
            .GenerateAsync(new ObjectFileModel<ClassModel>(model.Aggregate, model.Aggregate.UsingDirectives, model.Aggregate.Name, aggregateDirectory, ".cs"));

        await artifactGenerator
            .GenerateAsync(new ObjectFileModel<ClassModel>(model.AggregateDto, model.AggregateDto.UsingDirectives, model.AggregateDto.Name, aggregateDirectory, ".cs"));

        await artifactGenerator
            .GenerateAsync(new ObjectFileModel<ClassModel>(model.AggregateExtensions, model.AggregateExtensions.UsingDirectives, model.AggregateExtensions.Name, aggregateDirectory, ".cs"));


        foreach (var query in model.Queries)
        {
            await artifactGenerator
                .GenerateAsync(
                new ObjectFileModel<QueryModel>(
                    query,
                    query.UsingDirectives,
                    query.Name,
                    $"{aggregateDirectory}{Path.DirectorySeparatorChar}Queries",
                    ".cs"), new { Entity = model.Aggregate });
        }

        foreach (var command in model.Commands)
        {
            await artifactGenerator
                .GenerateAsync(
                new ObjectFileModel<CommandModel>(
                    command,
                    command.UsingDirectives,
                    command.Name,
                    $"{aggregateDirectory}{Path.DirectorySeparatorChar}Commands",
                    ".cs"), new { Entity = model.Aggregate });
        }
    }
}

