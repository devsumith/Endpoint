// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Text;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Properties;
using Endpoint.Core.Syntax.TypeScript;
using Endpoint.Core.Syntax.Units;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Folders.Factories;

public class FolderFactory : IFolderFactory
{
    private readonly ILogger<FolderFactory> logger;
    private readonly IFileProvider fileProvider;
    private readonly IFileSystem fileSystem;
    private readonly INamingConventionConverter namingConventionConverter;
    private readonly IFileFactory fileFactory;
    private readonly IClassFactory classFactory;

    public FolderFactory(
        ILogger<FolderFactory> logger,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        INamingConventionConverter namingConventionConverter,
        IFileFactory fileFactory,
        IClassFactory classFactory)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(fileSystem));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
    }

    public FolderModel AggregagteCommands(ClassModel aggregate, string directory)
    {
        logger.LogInformation("Generating Aggregate Commands. {name}", aggregate.Name);

        var aggregateName = aggregate.Name;

        fileSystem.Directory.CreateDirectory(directory);

        var model = new FolderModel("Commands", directory);

        var microserviceName = Path.GetFileNameWithoutExtension(fileProvider.Get("*.csproj", directory)).Split('.').First();

        foreach (var routeType in new RouteType[] { RouteType.Create, RouteType.Delete, RouteType.Update })
        {
            // TODO: build command and subclasses fully here. Class factory
            // var command = new CommandModel(microserviceName, aggregate, _namingConventionConverter, routeType);
            var command = new CommandModel();

            model.Files.Add(new CodeFileModel<CommandModel>(command, command.UsingDirectives, command.Name, model.Directory, ".cs"));
        }

        return model;
    }

    public FolderModel AggregagteQueries(ClassModel aggregate, string directory)
    {
        var model = new FolderModel("Queries", directory);

        var microserviceName = Path.GetFileNameWithoutExtension(fileProvider.Get("*.csproj", directory)).Split('.').First();

        foreach (var routeType in new RouteType[] { RouteType.GetById, RouteType.Get, RouteType.Page })
        {
            var query = new QueryModel(); // (microserviceName, _namingConventionConverter, aggregate, routeType);

            model.Files.Add(new CodeFileModel<QueryModel>((QueryModel)query, (System.Collections.Generic.List<UsingModel>)query.UsingDirectives, (string)query.Name, model.Directory, ".cs"));
        }

        return model;
    }

    public FolderModel AngularDomainModel(string modelName, string properties, string directory)
    {
        var modelNameSnakeCase = namingConventionConverter.Convert(NamingConvention.SnakeCase, modelName);

        var model = new FolderModel(modelNameSnakeCase, directory);

        var typeScriptTypeModel = new TypeScriptTypeModel(modelNameSnakeCase);

        foreach (var property in properties.Split(','))
        {
            var propertyName = namingConventionConverter.Convert(NamingConvention.CamelCase, property.Split(':')[0]);

            var propertyType = property.Split(':')[1] switch
            {
                "guid" => "string",
                "int" => "number",
                _ => property.Split(':')[1]
            };

            typeScriptTypeModel.Properties.Add(PropertyModel.TypeScriptProperty(propertyName, propertyType));
        }

        model.Files.Add(new CodeFileModel<TypeScriptTypeModel>(typeScriptTypeModel, typeScriptTypeModel.Name, model.Directory, ".ts"));

        model.Files.Add(fileFactory.CreateTemplate("http-service", $"{modelNameSnakeCase}.service", model.Directory, ".ts", tokens: new TokensBuilder()
            .With("entityName", modelNameSnakeCase)
            .Build()));

        model.Files.Add(fileFactory.CreateTemplate("component-store", $"{modelNameSnakeCase}.store", model.Directory, ".ts", tokens: new TokensBuilder()
            .With("entityName", modelNameSnakeCase)
            .Build()));

        model.Files.Add(new ContentFileModel(
            new StringBuilder()
            .AppendLine($"export * from './{modelNameSnakeCase}';")
            .AppendLine($"export * from './{modelNameSnakeCase}.service';")
            .AppendLine($"export * from './{modelNameSnakeCase}.store';")
            .ToString(), "index", model.Directory, ".ts"));

        return model;
    }

    public async Task<FolderModel> CreateAggregateAsync(string aggregateName, string properties, string directory)
    {
        var aggregate = classFactory.CreateEntity(aggregateName, properties);

        var model = new FolderModel($"{aggregateName}Aggregate", directory);

        model.SubFolders.Add(AggregagteCommands(aggregate, model.Directory));

        model.SubFolders.Add(AggregagteQueries(aggregate, model.Directory));

        var aggregateDto = aggregate.CreateDto();

        var extensions = await classFactory.DtoExtensionsCreateAsync(aggregate);

        model.Files.Add(new CodeFileModel<ClassModel>(aggregate, aggregate.Usings, aggregate.Name, model.Directory, ".cs"));

        model.Files.Add(new CodeFileModel<ClassModel>(aggregateDto, aggregateDto.Usings, aggregateDto.Name, model.Directory, ".cs"));

        model.Files.Add(new CodeFileModel<ClassModel>(extensions, extensions.Usings, extensions.Name, model.Directory, ".cs"));

        return model;
    }
}

public class AggregateFolderModel : FolderModel
{
    public AggregateFolderModel(ClassModel aggregate, string directory)
        : base($"{aggregate.Name}Aggregate", directory)
    {
        Aggregate = aggregate;
    }

    public ClassModel Aggregate { get; set; }
}
