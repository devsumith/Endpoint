using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Services;
using System.IO;
using System.Collections.Generic;
using Endpoint.Core.Models.Syntax.Classes;
using static DotLiquid.Variable;
using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Abstractions;

namespace Endpoint.Cli.Commands;


[Verb("db-context-create-from-aggregate-model")]
public class DbContextCreateFromAggregateModelRequest : IRequest<Unit> {

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class DbContextCreateFromAggregateModelRequestHandler : IRequestHandler<DbContextCreateFromAggregateModelRequest, Unit>
{
    private readonly ILogger<DbContextCreateFromAggregateModelRequestHandler> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    
    public DbContextCreateFromAggregateModelRequestHandler(
        ILogger<DbContextCreateFromAggregateModelRequestHandler> logger,
        IFileProvider fileProvider,
        INamingConventionConverter namingConventionConverter,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
    }

    public async Task<Unit> Handle(DbContextCreateFromAggregateModelRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(DbContextCreateFromAggregateModelRequestHandler));

        var entities = new List<EntityModel>();

        var projectDirectory = Path.GetDirectoryName(_fileProvider.Get("*.csproj",request.Directory));

        var serviceName = Path.GetFileNameWithoutExtension(projectDirectory).Replace(".Core", string.Empty);

        var aggregateModelDirectory = $"{projectDirectory}{Path.DirectorySeparatorChar}AggregateModel";

        foreach(var folder in Directory.GetDirectories(aggregateModelDirectory))
        {
            var folderName = Path.GetFileNameWithoutExtension(folder);

            if(folderName.EndsWith("Aggregate")) 
                entities.Add(new EntityModel(folderName.Replace("Aggregate", string.Empty)));
        }


        var classModel = new DbContextModel(_namingConventionConverter, $"{serviceName}DbContext", entities, serviceName);


        var interfaceModel = classModel.ToInterface();

        _artifactGenerationStrategyFactory.CreateFor(new ObjectFileModel<InterfaceModel>(interfaceModel, interfaceModel.UsingDirectives, interfaceModel.Name, projectDirectory, "cs"));

        
        return new();
    }
}