// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Models.Syntax.Classes.Factories;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Models.Artifacts.Projects.Services;

public class InfrastructureProjectService : IInfrastructureProjectService
{
    private readonly ILogger<InfrastructureProjectService> _logger;
    private readonly IClassModelFactory _classModelFactory;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;

    public InfrastructureProjectService(
        ILogger<InfrastructureProjectService> logger,
        IClassModelFactory classModelFactory,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        IFileModelFactory fileModelFactory,
        IFileProvider fileProvider,
        IFileSystem fileSystem)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public void DbContextAdd(string directory)
    {
        var csProjPath = _fileProvider.Get("*.csproj", directory);

        var csProjDirectory = Path.GetDirectoryName(csProjPath);

        var dataDirectory = $"{csProjDirectory}{Path.DirectorySeparatorChar}Data";

        _fileSystem.CreateDirectory(dataDirectory);

        var serviceName = Path.GetFileNameWithoutExtension(csProjPath).Split('.').First();

        var dbContext = _classModelFactory.CreateDbContext($"{serviceName}DbContext", new List<EntityModel>(), serviceName);

        var fileModel = _fileModelFactory.CreateCSharp(dbContext, dataDirectory);

        _artifactGenerationStrategyFactory.CreateFor(fileModel);
    }
}


