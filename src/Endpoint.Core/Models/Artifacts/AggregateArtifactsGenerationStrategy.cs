using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Entities.Aggregate;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Core.Models.Artifacts;

public class AggregateArtifactsGenerationStrategy : ArtifactGenerationStrategyBase<AggregateModel>
{
    private readonly IFileSystem _fileSystem;
    public AggregateArtifactsGenerationStrategy(IServiceProvider serviceProvider, IFileSystem fileSystem) 
        :base(serviceProvider) { 
    
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, AggregateModel model, dynamic configuration = null)
    {
        var aggregateDirectory = $"{model.Directory}{Path.DirectorySeparatorChar}{model.Aggregate.Name}Aggregate";

        _fileSystem.CreateDirectory(aggregateDirectory);

        _fileSystem.CreateDirectory($"{aggregateDirectory}{Path.DirectorySeparatorChar}Commands");

        _fileSystem.CreateDirectory($"{aggregateDirectory}{Path.DirectorySeparatorChar}Queries");

        artifactGenerationStrategyFactory
            .CreateFor(new ObjectFileModel<ClassModel>(model.Aggregate, model.Aggregate.UsingDirectives, model.Aggregate.Name, aggregateDirectory, "cs"));

        artifactGenerationStrategyFactory
            .CreateFor(new ObjectFileModel<ClassModel>(model.AggregateDto, model.AggregateDto.UsingDirectives, model.AggregateDto.Name, aggregateDirectory, "cs"));

        artifactGenerationStrategyFactory
            .CreateFor(new ObjectFileModel<ClassModel>(model.AggregateExtensions, model.AggregateExtensions.UsingDirectives, model.AggregateExtensions.Name, aggregateDirectory, "cs"));


        foreach (var query in model.Queries)
        {
            artifactGenerationStrategyFactory
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
            artifactGenerationStrategyFactory
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
