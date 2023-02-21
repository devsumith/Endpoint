// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Entities.Aggregate;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Models.Artifacts.Folders.Strategies;

public class FolderFactory : IFolderFactory
{
    private readonly ILogger<FolderFactory> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly INamingConventionConverter _namingConventionConverter;

    public FolderFactory(
        ILogger<FolderFactory> logger,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        INamingConventionConverter namingConventionConverter)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _namingConventionConverter = namingConventionConverter;
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
}


