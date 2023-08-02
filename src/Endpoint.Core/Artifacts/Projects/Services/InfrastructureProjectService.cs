// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Artifacts.Projects.Services;

public class InfrastructureProjectService : IInfrastructureProjectService
{
    private readonly ILogger<InfrastructureProjectService> _logger;
    private readonly IClassModelFactory _classModelFactory;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;

    public InfrastructureProjectService(
        ILogger<InfrastructureProjectService> logger,
        IClassModelFactory classModelFactory,
        IArtifactGenerator artifactGenerator,
        IFileModelFactory fileModelFactory,
        IFileProvider fileProvider,
        IFileSystem fileSystem)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
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

        _artifactGenerator.CreateFor(fileModel);
    }
}


