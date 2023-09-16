// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Projects.Strategies;

public class CoreProjectEnsureArtifactGenerationStrategy : GenericArtifactGenerationStrategy<ProjectReferenceModel>
{
    private readonly ILogger<ApiProjectEnsureArtifactGenerationStrategy> logger;
    private readonly IFileFactory fileFactory;
    private readonly IFileSystem fileSystem;
    private readonly IFileProvider fileProvider;
    private readonly ICommandService commandService;

    public CoreProjectEnsureArtifactGenerationStrategy(
        IFileFactory fileFactory,
        IFileSystem fileSystem,
        IFileProvider fileProvider,

        ICommandService commandService,
        ILogger<ApiProjectEnsureArtifactGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
    }

    public bool CanHandle(object model)
        => true; // => model is ProjectReferenceModel && context.Command is CoreProjectEnsure;

    public override async Task GenerateAsync(IArtifactGenerator artifactGenerator, ProjectReferenceModel model)
    {
        logger.LogInformation("Generating artifact for {0}.", model);

        var projectDirectory = Path.GetDirectoryName(fileProvider.Get("*.csproj", model.ReferenceDirectory));

        EnsureDefaultFilesRemoved(projectDirectory);

        EnsurePackagesInstalled(projectDirectory);

        EnsureProjectsReferenced(projectDirectory);

        EnsureDefaultFilesAdd(artifactGenerator, projectDirectory);
    }

    private void EnsureDefaultFilesRemoved(string projectDirectory)
    {
        fileSystem.File.Delete($"{projectDirectory}{Path.DirectorySeparatorChar}Class1.cs");
    }

    private void EnsureDefaultFilesAdd(IArtifactGenerator artifactGenerator, string projectDirectory)
    {
        var projectName = Path.GetFileNameWithoutExtension(projectDirectory).Split('.').First();

        var dbContext = $"{projectName}DbContext";
    }

    private void EnsurePackagesInstalled(string projectDirectory)
    {
        var projectPath = fileProvider.Get("*.csproj", projectDirectory);

        foreach (var package in new string[]
        {
            "Microsoft.EntityFrameworkCore",
        })
        {
            var projectFileContents = fileSystem.File.ReadAllText(projectPath);

            if (!projectFileContents.Contains($"PackageReference Include=\"{package}\""))
            {
                commandService.Start($"dotnet add package {package}", projectDirectory);
            }
        }
    }

    private void EnsureProjectsReferenced(string projectDirectory)
    {
        commandService.Start($"dotnet add {projectDirectory} reference \"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}BuildingBlocks{Path.DirectorySeparatorChar}Messaging{Path.DirectorySeparatorChar}Messaging.Udp{Path.DirectorySeparatorChar}Messaging.Udp.csproj{Path.DirectorySeparatorChar}{Path.DirectorySeparatorChar}", projectDirectory);
    }
}
