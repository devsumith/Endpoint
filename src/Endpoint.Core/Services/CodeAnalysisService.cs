// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using Endpoint.Core.Artifacts.Solutions;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Logging;
using static System.Environment.SpecialFolder;

namespace Endpoint.Core.Services;

public class CodeAnalysisService : ICodeAnalysisService
{
    private readonly ILogger<CodeAnalysisService> logger;
    private readonly IFileProvider fileProvider;
    private readonly IFileSystem fileSystem;
    private readonly MSBuildWorkspace workspace;

    public CodeAnalysisService(
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        ILogger<CodeAnalysisService> logger)
    {
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        MSBuildLocator.RegisterDefaults();

        workspace = MSBuildWorkspace.Create();
    }

    public SyntaxModel SyntaxModel { get; set; }

    public SolutionModel SolutionModel { get; set; }

    public async Task<bool> IsNpmPackageInstalledAsync(string name)
    {
        logger.LogInformation("Querying if npm package is installed. {name}", name);

        return fileSystem.Directory.Exists(fileSystem.Path.Combine($"{ApplicationData}", "Roaming", "npm", "node_modules", name));
    }

    public async Task<bool> IsPackageInstalledAsync(string name, string directory)
    {
        logger.LogInformation("Checking if package is installed. {name}", name);

        var projectPath = fileProvider.Get("*.csproj", directory);

        var project = await workspace.OpenProjectAsync(projectPath);

        var result = project.MetadataReferences.Any(x => x.Display.Contains(name));

        workspace.CloseSolution();

        return result;
    }
}