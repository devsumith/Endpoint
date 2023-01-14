using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Octokit;
using Octokit.Internal;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Endpoint.Core.Models.Artifacts.Projects;

public class ProjectService : IProjectService
{
    private readonly ICommandService _commandService;
    private readonly IFileProvider _fileProvider;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    public ProjectService(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, ICommandService commandService, IFileProvider fileProvider)
    {
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory;
    }

    public void AddProject(ProjectModel model)
    {
        _artifactGenerationStrategyFactory.CreateFor(model);

        AddToSolution(model);
    }

    public void AddToSolution(ProjectModel model)
    {
        var solution = _fileProvider.Get("*.sln", model.Directory);

        var solutionName = Path.GetFileName(solution);

        var solutionDirectory = Path.GetDirectoryName(solution);

        _commandService.Start($"dotnet sln {solutionName} add {model.Path}", solutionDirectory);
    }

    public void AddGenerateDocumentationFile(string csprojFilePath)
    {
        var doc = XDocument.Load(csprojFilePath);
        var projectNode = doc.FirstNode as XElement;

        var element = projectNode.Nodes()
            .Where(x => x.NodeType == System.Xml.XmlNodeType.Element)
            .First(x => (x as XElement).Name == "PropertyGroup") as XElement;

        element.Add(new XElement("GenerateDocumentationFile", true));
        element.Add(new XElement("NoWarn", "$(NoWarn);1591"));
        doc.Save(csprojFilePath);
    }

    public void AddEndpointPostBuildTargetElement(string csprojFilePath)
    {
        var doc = XDocument.Load(csprojFilePath);
        var projectNode = doc.FirstNode as XElement;
        projectNode.Add(_createEndpointPostBuildTargetElement());
        doc.Save(csprojFilePath);
    }

    private XElement _createEndpointPostBuildTargetElement()
    {
        var dotnetToolRestoreCommand = new XElement("Exec");

        dotnetToolRestoreCommand.SetAttributeValue("Command", "dotnet tool restore");

        var toFileCommand = new XElement("Exec");

        toFileCommand.SetAttributeValue("Command", "dotnet tool run swagger tofile --serializeasv2  --output \"$(ProjectDir)swagger.json\" \"$(TargetDir)$(TargetFileName)\" v1");

        var endpointCommand = new XElement("Exec");

        endpointCommand.SetAttributeValue("Command", "endpoint post-api-build");

        var element = new XElement("Target", dotnetToolRestoreCommand, toFileCommand, endpointCommand);

        element.SetAttributeValue("Name", "EndpointPostBuildTarget");

        element.SetAttributeValue("AfterTargets", "Build");

        return element;
    }

    public void PackageAdd(string name, string directory)
    {
        var projectDirectory = Path.GetDirectoryName(_fileProvider.Get("*.csproj",directory));

        _commandService.Start($"dotnet add package {name}", projectDirectory);
    }
}
