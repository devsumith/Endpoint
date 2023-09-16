// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Entities;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Projects.Services;

public class InfrastructureProjectService : IInfrastructureProjectService
{
    private readonly ILogger<InfrastructureProjectService> logger;
    private readonly IClassFactory classFactory;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly IFileFactory fileFactory;
    private readonly IFileProvider fileProvider;
    private readonly IFileSystem fileSystem;

    public InfrastructureProjectService(
        ILogger<InfrastructureProjectService> logger,
        IClassFactory classFactory,
        IArtifactGenerator artifactGenerator,
        IFileFactory fileFactory,
        IFileProvider fileProvider,
        IFileSystem fileSystem)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public async Task DbContextAdd(string directory)
    {
        var csProjPath = fileProvider.Get("*.csproj", directory);

        var csProjDirectory = Path.GetDirectoryName(csProjPath);

        var dataDirectory = $"{csProjDirectory}{Path.DirectorySeparatorChar}Data";

        fileSystem.Directory.CreateDirectory(dataDirectory);

        var serviceName = Path.GetFileNameWithoutExtension(csProjPath).Split('.').First();

        var dbContext = classFactory.CreateDbContext($"{serviceName}DbContext", new List<EntityModel>(), serviceName);

        var fileModel = fileFactory.CreateCSharp(dbContext, dataDirectory);

        await artifactGenerator.GenerateAsync(fileModel);
    }
}
