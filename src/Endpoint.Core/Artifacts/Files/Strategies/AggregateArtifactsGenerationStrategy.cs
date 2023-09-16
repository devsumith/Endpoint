// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Units;
using System.IO;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class AggregateArtifactsGenerationStrategy : GenericArtifactGenerationStrategy<AggregateModel>
{
    private readonly IFileSystem _fileSystem;
    public AggregateArtifactsGenerationStrategy(IServiceProvider serviceProvider, IFileSystem fileSystem)
    {

        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }



    public override async Task GenerateAsync(IArtifactGenerator artifactGenerator, AggregateModel model)
    {
        var directory = string.Empty;

        var aggregateDirectory = $"{directory}{Path.DirectorySeparatorChar}{model.Aggregate.Name}Aggregate";

        _fileSystem.Directory.CreateDirectory(aggregateDirectory);

        _fileSystem.Directory.CreateDirectory($"{aggregateDirectory}{Path.DirectorySeparatorChar}Commands");

        _fileSystem.Directory.CreateDirectory($"{aggregateDirectory}{Path.DirectorySeparatorChar}Queries");

        await artifactGenerator
            .GenerateAsync(new CodeFileModel<ClassModel>(model.Aggregate, model.Aggregate.Usings, model.Aggregate.Name, aggregateDirectory, ".cs"));

        await artifactGenerator
            .GenerateAsync(new CodeFileModel<ClassModel>(model.AggregateDto, model.AggregateDto.Usings, model.AggregateDto.Name, aggregateDirectory, ".cs"));

        await artifactGenerator
            .GenerateAsync(new CodeFileModel<ClassModel>(model.AggregateExtensions, model.AggregateExtensions.Usings, model.AggregateExtensions.Name, aggregateDirectory, ".cs"));


        foreach (var query in model.Queries)
        {
            await artifactGenerator
                .GenerateAsync(
                new CodeFileModel<QueryModel>(
                    query,
                    query.UsingDirectives,
                    query.Name,
                    $"{aggregateDirectory}{Path.DirectorySeparatorChar}Queries",
                    ".cs"));
        }

        foreach (var command in model.Commands)
        {
            await artifactGenerator
                .GenerateAsync(
                new CodeFileModel<CommandModel>(
                    command,
                    command.UsingDirectives,
                    command.Name,
                    $"{aggregateDirectory}{Path.DirectorySeparatorChar}Commands",
                    ".cs"));
        }
    }
}

