// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Projects.Commands;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Artifacts.Projects.Strategies;

public class ApiProjectEnsureArtifactGenerationStrategy : ArtifactGenerationStrategyBase<ProjectReferenceModel>
{
    private readonly ILogger<ApiProjectEnsureArtifactGenerationStrategy> _logger;
    private readonly IFileFactory _fileFactory;
    private readonly IFileSystem _fileSystem;
    private readonly IFileProvider _fileProvider;
    private readonly ICommandService _commandService;

    public ApiProjectEnsureArtifactGenerationStrategy(
        IFileFactory fileFactory,
        IFileSystem fileSystem,
        IFileProvider fileProvider,
        IServiceProvider serviceProvider,
        ICommandService commandService,
        ILogger<ApiProjectEnsureArtifactGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
    }

    public override bool CanHandle(object model, dynamic context = null)
        => model is ProjectReferenceModel && context != null && context.Command is ApiProjectEnsure;

    public override int Priority => 10;

    public override async Task CreateAsync(IArtifactGenerator artifactGenerator, ProjectReferenceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        var projectDirectory = Path.GetDirectoryName(_fileProvider.Get("*.csproj", model.ReferenceDirectory));

        EnsureDefaultFilesRemoved(projectDirectory);

        EnsurePackagesInstalled(projectDirectory);

        EnsureProjectsReferenced(projectDirectory);

        await EnsureApiDefaultFilesAdd(artifactGenerator, projectDirectory);
    }

    private void EnsureDefaultFilesRemoved(string projectDirectory)
    {
        _fileSystem.Delete($"{projectDirectory}{Path.DirectorySeparatorChar}Controllers{Path.DirectorySeparatorChar}WeatherForecastController.cs");
        _fileSystem.Delete($"{projectDirectory}{Path.DirectorySeparatorChar}WeatherForecast.cs");
    }

    private async Task EnsureApiDefaultFilesAdd(IArtifactGenerator artifactGenerator, string projectDirectory)
    {
        var projectName = Path.GetFileNameWithoutExtension(projectDirectory).Split('.').First();

        var dbContext = $"{projectName}DbContext";

        if (!_fileSystem.Exists($"{projectDirectory}{Path.DirectorySeparatorChar}Properties{Path.DirectorySeparatorChar}launchSettings.json"))
        {
            await artifactGenerator.CreateAsync(_fileFactory.LaunchSettingsJson(projectDirectory, projectName, 5000));
        }

        if (!_fileSystem.Exists($"{projectDirectory}{Path.DirectorySeparatorChar}ConfigureServices.cs"))
        {
            await artifactGenerator.CreateAsync(_fileFactory.CreateTemplate("Api.ConfigureServices", "ConfigureServices", projectDirectory, tokens: new TokensBuilder()
                .With("DbContext", dbContext)
                .With("serviceName", projectName)
                .Build()));

            await artifactGenerator.CreateAsync(_fileFactory.CreateTemplate("Api.Program", "Program", projectDirectory, tokens: new TokensBuilder()
                .With("DbContext", dbContext)
                .With("serviceName", projectName)
                .Build()));
        }
    }

    private void EnsurePackagesInstalled(string projectDirectory)
    {
        var projectPath = _fileProvider.Get("*.csproj", projectDirectory);

        foreach (var package in new string[] {
            "Serilog",
            "SerilogTimings",
            "Serilog.AspNetCore",
            "Swashbuckle.AspNetCore",
            "Swashbuckle.AspNetCore.Annotations",
            "Swashbuckle.AspNetCore.Swagger",
            "Swashbuckle.AspNetCore.Newtonsoft"
        })
        {
            var projectFileContents = _fileSystem.ReadAllText(projectPath);

            if (!projectFileContents.Contains($"PackageReference Include=\"{package}\""))
            {
                _commandService.Start($"dotnet add package {package}", projectDirectory);
            }
        }
    }

    private void EnsureProjectsReferenced(string projectDirectory)
    {
        var projectName = Path.GetFileNameWithoutExtension(projectDirectory).Split('.').First();

        var solutionDirectory = projectDirectory;

        while (!Directory.EnumerateFiles(solutionDirectory, "*.sln").Any(x => x.EndsWith($"sln")))
        {
            solutionDirectory = solutionDirectory.Substring(0, solutionDirectory.LastIndexOf(Path.DirectorySeparatorChar));
        }

        foreach (var infrastructureProjectPath in Directory.GetFiles(solutionDirectory, $"{projectName}.Infrastructure.csproj", SearchOption.AllDirectories))
        {
            _commandService.Start($"dotnet add {projectDirectory} reference {infrastructureProjectPath}", projectDirectory);
        }
    }
}
