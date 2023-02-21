// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Artifacts.Folders.Factories;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Endpoint.Core.Models.Artifacts.Folders.Services;

public class FolderService : IFolderService
{
    private readonly ILogger<FolderService> _logger;
    private readonly IFolderFactory _folderFactory;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly ISyntaxService _syntaxService;
    private readonly IFileProvider _fileProvider;
    public FolderService(
        ILogger<FolderService> logger,
        IFolderFactory folderFactory,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        ISyntaxService syntaxService,
        IFileProvider fileProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _folderFactory = folderFactory ?? throw new ArgumentNullException(nameof(folderFactory));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _syntaxService = syntaxService ?? throw new ArgumentNullException(nameof(syntaxService));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    public FolderModel AggregateQueries(string aggregateName, string directory)
    {
        var model = _folderFactory.AggregagteQueries(aggregateName, directory);

        var serviceName = Path.GetFileNameWithoutExtension(_fileProvider.Get("*.csproj", directory).Split('.').First());

        var entity = _syntaxService.SolutionModel?.GetClass(aggregateName, serviceName);

        _artifactGenerationStrategyFactory.CreateFor(model, new { Entity = entity });

        return model;
    }

    public FolderModel AggregateCommands(string aggregateName, string directory)
    {
        var model = _folderFactory.AggregagteCommands(aggregateName, directory);

        var serviceName = Path.GetFileNameWithoutExtension(_fileProvider.Get("*.csproj", directory).Split('.').First());

        var entity = _syntaxService.SolutionModel?.GetClass(aggregateName, serviceName);

        _artifactGenerationStrategyFactory.CreateFor(model, new { Entity = entity });

        return model;
    }
}