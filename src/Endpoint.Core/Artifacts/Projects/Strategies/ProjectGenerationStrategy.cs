// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Projects.Enums;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Endpoint.Core.Artifacts.Projects.Strategies;

public class ProjectGenerationStrategy : GenericArtifactGenerationStrategy<ProjectModel>
{
    private readonly ILogger<ProjectGenerationStrategy> _logger;
    private readonly IFileSystem _fileSystem;
    private readonly ICommandService _commandService;

    public ProjectGenerationStrategy(
        ILogger<ProjectGenerationStrategy> logger,
        IFileSystem fileSystem,
        ICommandService commandService)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public override async Task GenerateAsync(IArtifactGenerator generator, ProjectModel model)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

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

        _fileSystem.Directory.CreateDirectory(model.Directory);

        _commandService.Start($"dotnet new {templateType} --framework net7.0", model.Directory);

        foreach (var path in _fileSystem.Directory.GetFiles(model.Directory, "*1.cs", SearchOption.AllDirectories))
            _fileSystem.File.Delete(path);

        if (templateType == "webapi")
        {
            _fileSystem.File.Delete($"{model.Directory}{Path.DirectorySeparatorChar}Controllers{Path.DirectorySeparatorChar}WeatherForecastController.cs");

            _fileSystem.File.Delete($"{model.Directory}{Path.DirectorySeparatorChar}WeatherForecast.cs");

            _fileSystem.Directory.Delete($"{model.Directory}{Path.DirectorySeparatorChar}Controllers");
        }

        foreach (var folder in model.Folders)
        {
            _fileSystem.Directory.CreateDirectory(folder.Directory);
        }

        foreach (var package in model.Packages)
        {
            var version = package.IsPreRelease ? "--prerelease" : $"--version {package.Version}";

            if (!package.IsPreRelease && string.IsNullOrEmpty(package.Version))
                version = null;

            _commandService.Start($"dotnet add package {package.Name} {version}", model.Directory);
        }

        if (model.References != null)
            foreach (var path in model.References)
            {
                _commandService.Start($"dotnet add {model.Directory} reference {path}", model.Directory);
            }

        foreach (var file in model.Files)
        {
            await generator.GenerateAsync(file);
        }

        if (model.GenerateDocumentationFile || templateType == "web" || templateType == "webapi" || templateType == "angular")
        {
            var doc = XDocument.Load(model.Path);
            var projectNode = doc.FirstNode as XElement;

            var element = projectNode.Nodes()
                .Where(x => x.NodeType == System.Xml.XmlNodeType.Element)
                .First(x => (x as XElement).Name == "PropertyGroup") as XElement;

            element.Add(new XElement("GenerateDocumentationFile", true));
            element.Add(new XElement("NoWarn", "$(NoWarn);1591"));
            doc.Save(model.Path);
        }

    }
}
