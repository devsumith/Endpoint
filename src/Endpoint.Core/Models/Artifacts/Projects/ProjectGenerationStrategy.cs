using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using Octokit.Internal;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Endpoint.Core.Models.Artifacts.Projects;

public class ProjectGenerationStrategy : ArtifactGenerationStrategyBase<ProjectModel>
{
    private readonly ILogger<ProjectGenerationStrategy> _logger;
    private readonly IFileSystem _fileSystem;
    private readonly ICommandService _commandService;

    public ProjectGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<ProjectGenerationStrategy> logger,
        IFileSystem fileSystem,
        ICommandService commandService) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, ProjectModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        string templateType = model.DotNetProjectType switch
        {
            DotNetProjectType.Web => "web",
            DotNetProjectType.WebApi => "webapi",
            DotNetProjectType.ClassLib => "classlib",
            DotNetProjectType.Worker => "worker",
            DotNetProjectType.XUnit => "xunit",
            _ => "console"
        };

        _fileSystem.CreateDirectory(model.Directory);

        _commandService.Start($"dotnet new {templateType} --framework net7.0", model.Directory);

        foreach (var path in _fileSystem.GetFiles(model.Directory, "*.cs", SearchOption.AllDirectories))
        {
            _fileSystem.Delete(path);
        }

        foreach (var package in model.Packages)
        {
            var version = package.IsPreRelease ? "--prerelease" : $"--version {package.Version}";

            _commandService.Start($"dotnet add package {package.Name} {version}", model.Directory);
        }

        if(model.References != null)
            foreach (var path in model.References)
            {
                _commandService.Start($"dotnet add {model.Directory} reference {path}", model.Directory);
            }

        foreach (var file in model.Files)
        {
            artifactGenerationStrategyFactory.CreateFor(file);
        }

        if (model.GenerateDocumentationFile)
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