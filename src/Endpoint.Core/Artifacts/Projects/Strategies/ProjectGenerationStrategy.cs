// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Xml.Linq;
using Endpoint.Core.Artifacts.Projects.Enums;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Projects.Strategies;

public class ProjectGenerationStrategy : GenericArtifactGenerationStrategy<ProjectModel>
{
    private readonly ILogger<ProjectGenerationStrategy> logger;
    private readonly IFileSystem fileSystem;
    private readonly ICommandService commandService;

    public ProjectGenerationStrategy(
        ILogger<ProjectGenerationStrategy> logger,
        IFileSystem fileSystem,
        ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public override async Task GenerateAsync(IArtifactGenerator generator, ProjectModel model)
    {
        logger.LogInformation("Generating artifact for {0}.", model);

        string templateType = model.DotNetProjectType switch
        {
            DotNetProjectType.Web => "web",
            DotNetProjectType.WebApi => "webapi",
            DotNetProjectType.ClassLib => "classlib",
            DotNetProjectType.Worker => "worker",
            DotNetProjectType.XUnit => "xunit",
            DotNetProjectType.NUnit => "nunit",
            DotNetProjectType.Angular => "angular",
            _ => "console"
        };

        fileSystem.Directory.CreateDirectory(model.Directory);

        commandService.Start($"dotnet new {templateType} --framework net7.0", model.Directory);

        foreach (var path in fileSystem.Directory.GetFiles(model.Directory, "*1.cs", SearchOption.AllDirectories))
        {
            fileSystem.File.Delete(path);
        }

        if (templateType == "webapi")
        {
            fileSystem.File.Delete($"{model.Directory}{Path.DirectorySeparatorChar}Controllers{Path.DirectorySeparatorChar}WeatherForecastController.cs");

            fileSystem.File.Delete($"{model.Directory}{Path.DirectorySeparatorChar}WeatherForecast.cs");

            fileSystem.Directory.Delete($"{model.Directory}{Path.DirectorySeparatorChar}Controllers");
        }

        foreach (var folder in model.Folders)
        {
            fileSystem.Directory.CreateDirectory(folder.Directory);
        }

        foreach (var package in model.Packages)
        {
            var version = package.IsPreRelease ? "--prerelease" : $"--version {package.Version}";

            if (!package.IsPreRelease && string.IsNullOrEmpty(package.Version))
            {
                version = null;
            }

            commandService.Start($"dotnet add package {package.Name} {version}", model.Directory);
        }

        if (model.References != null)
        {
            foreach (var path in model.References)
            {
                commandService.Start($"dotnet add {model.Directory} reference {path}", model.Directory);
            }
        }

        foreach (var file in model.Files)
        {
            await generator.GenerateAsync(file);
        }

        var doc = XDocument.Load(model.Path);
        var projectNode = doc.FirstNode as XElement;

        var element = projectNode.Nodes()
            .Where(x => x.NodeType == System.Xml.XmlNodeType.Element)
            .First(x => (x as XElement).Name == "PropertyGroup") as XElement;

        element.Add(new XElement("NoWarn", string.Join(",", model.NoWarn)));

        if (model.GenerateDocumentationFile || templateType == "web" || templateType == "webapi" || templateType == "angular")
        {
            element.Add(new XElement("GenerateDocumentationFile", true));
        }

        doc.Save(model.Path);
    }
}
