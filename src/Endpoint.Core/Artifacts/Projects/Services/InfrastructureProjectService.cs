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
using System.Threading.Tasks;

namespace Endpoint.Core.Artifacts.Projects.Services;

public class InfrastructureProjectService : IInfrastructureProjectService
{
    private readonly ILogger<InfrastructureProjectService> _logger;
    private readonly IClassFactory _classFactory;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly IFileFactory _fileFactory;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;

    public InfrastructureProjectService(
        ILogger<InfrastructureProjectService> logger,
        IClassFactory classFactory,
        IArtifactGenerator artifactGenerator,
        IFileFactory fileFactory,
        IFileProvider fileProvider,
        IFileSystem fileSystem)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        _fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public async Task DbContextAdd(string directory)
    {
        var csProjPath = _fileProvider.Get("*.csproj", directory);

        var csProjDirectory = Path.GetDirectoryName(csProjPath);

        var dataDirectory = $"{csProjDirectory}{Path.DirectorySeparatorChar}Data";

        _fileSystem.Directory.CreateDirectory(dataDirectory);

        var serviceName = Path.GetFileNameWithoutExtension(csProjPath).Split('.').First();

        var dbContext = _classFactory.CreateDbContext($"{serviceName}DbContext", new List<EntityModel>(), serviceName);

        var fileModel = _fileFactory.CreateCSharp(dbContext, dataDirectory);

        await _artifactGenerator.GenerateAsync(fileModel);
    }
}


