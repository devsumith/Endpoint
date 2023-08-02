// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Folders.Factories;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Artifacts.Folders.Services;

public class FolderService : IFolderService
{
    private readonly ILogger<FolderService> _logger;
    private readonly IFolderFactory _folderFactory;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly ISyntaxService _syntaxService;
    private readonly IFileProvider _fileProvider;
    public FolderService(
        ILogger<FolderService> logger,
        IFolderFactory folderFactory,
        IArtifactGenerator artifactGenerator,
        ISyntaxService syntaxService,
        IFileProvider fileProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _folderFactory = folderFactory ?? throw new ArgumentNullException(nameof(folderFactory));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _syntaxService = syntaxService ?? throw new ArgumentNullException(nameof(syntaxService));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    public FolderModel AggregateQueries(ClassModel aggregate, string directory)
    {
        var model = _folderFactory.AggregagteQueries(aggregate, directory);

        var serviceName = Path.GetFileNameWithoutExtension(_fileProvider.Get("*.csproj", directory).Split('.').First());

        var entity = _syntaxService.SolutionModel?.GetClass(aggregate.Name, serviceName);

        _artifactGenerator.CreateFor(model, new { Entity = entity });

        return model;
    }

    public FolderModel AggregateCommands(ClassModel aggregate, string directory)
    {
        var model = _folderFactory.AggregagteCommands(aggregate, directory);

        var serviceName = Path.GetFileNameWithoutExtension(_fileProvider.Get("*.csproj", directory).Split('.').First());

        var entity = _syntaxService.SolutionModel?.GetClass(aggregate.Name, serviceName);

        _artifactGenerator.CreateFor(model, new { Entity = entity });

        return model;
    }
}