// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Enums;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Entities.Aggregate;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.TypeScript;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using Octokit.Internal;
using System.IO;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Artifacts.Folders.Factories;

public class FolderFactory : IFolderFactory
{
    private readonly ILogger<FolderFactory> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly IFileModelFactory _fileModelFactory;

    public FolderFactory(
        ILogger<FolderFactory> logger,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        INamingConventionConverter namingConventionConverter,
        IFileModelFactory fileModelFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(fileSystem));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
    }

    public FolderModel AggregagteCommands(string aggregateName, string directory)
    {
        var model = new FolderModel("Commands", directory);

        var microserviceName = Path.GetFileNameWithoutExtension(_fileProvider.Get("*.csproj", directory)).Split('.').First();

        foreach (var routeType in new RouteType[] { RouteType.Create, RouteType.Delete, RouteType.Update })
        {
            var command = new CommandModel(microserviceName, new ClassModel(aggregateName), routeType);

            model.Files.Add(new ObjectFileModel<CommandModel>(command, command.UsingDirectives, command.Name, model.Directory, "cs"));
        }

        return model;
    }

    public FolderModel AggregagteQueries(string aggregateName, string directory)
    {
        var model = new FolderModel("Queries", directory);

        var microserviceName = Path.GetFileNameWithoutExtension(_fileProvider.Get("*.csproj", directory)).Split('.').First();

        foreach (var routeType in new RouteType[] { RouteType.GetById, RouteType.Get, RouteType.Page })
        {
            var query = new QueryModel(microserviceName, _namingConventionConverter, new ClassModel(aggregateName), routeType);

            model.Files.Add(new ObjectFileModel<QueryModel>(query, query.UsingDirectives, query.Name, model.Directory, "cs"));
        }

        return model;
    }

    public FolderModel AngularDomainModel(string modelName, string properties, string directory)
    {
        var modelNameSnakeCase = _namingConventionConverter.Convert(NamingConvention.SnakeCase, modelName);

        var model = new FolderModel(modelNameSnakeCase, directory);

        var typeScriptTypeModel = new TypeScriptTypeModel(modelNameSnakeCase);

        foreach(var property in properties.Split(','))
        {
            var propertyName = _namingConventionConverter.Convert(NamingConvention.CamelCase, property.Split(':')[0]);

            var propertyType = property.Split(':')[1] switch
            {
                "guid" => "string",
                "int" => "number",
                _ => property.Split(':')[1]
            };

            typeScriptTypeModel.Properties.Add(PropertyModel.TypeScriptProperty(propertyName, propertyType));
        }

        model.Files.Add(new ObjectFileModel<TypeScriptTypeModel>(typeScriptTypeModel, typeScriptTypeModel.Name, model.Directory, "ts"));

        model.Files.Add(_fileModelFactory.CreateTemplate("http-service", $"{modelNameSnakeCase}.service", model.Directory, "ts", tokens: new TokensBuilder()
            .With("entityName", modelNameSnakeCase)
            .Build()));

        model.Files.Add(_fileModelFactory.CreateTemplate("component-store", $"{modelNameSnakeCase}.store", model.Directory, "ts", tokens: new TokensBuilder()
            .With("entityName", modelNameSnakeCase)
            .Build()));

        model.Files.Add(new ContentFileModel(new StringBuilder()
            .AppendLine($"export * from './{modelNameSnakeCase}.ts")
            .AppendLine($"export * from './{modelNameSnakeCase}.service.ts")
            .AppendLine($"export * from './{modelNameSnakeCase}.store.ts")
            .ToString(), "index", model.Directory, "ts"));


        return model;
    }
}


