// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Endpoint.Core.Artifacts.Projects.Services;

public class ProjectService : IProjectService
{
    private readonly ICommandService _commandService;
    private readonly IFileProvider _fileProvider;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly IFileSystem _fileSystem;
    private readonly IFileModelFactory _fileModelFactory;
    public ProjectService(
        IArtifactGenerator artifactGenerator,
        ICommandService commandService,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IFileModelFactory fileModelFactory)
    {
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
    }

    public void AddProject(ProjectModel model)
    {
        _artifactGenerator.CreateFor(model);

        AddToSolution(model);
    }

    public void AddToSolution(ProjectModel model)
    {
        var solution = _fileProvider.Get("*.sln", model.Directory);
        var solutionName = Path.GetFileName(solution);
        var solutionDirectory = _fileSystem.GetDirectoryName(solution);
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
        var projectPath = _fileProvider.Get("*.csproj", directory);

        var projectDirectory = _fileSystem.GetDirectoryName(projectPath);

        var projectFileContents = _fileSystem.ReadAllText(projectPath);

        if (!projectFileContents.Contains($"PackageReference Include=\"{name}\""))
        {
            _commandService.Start($"dotnet add package {name}", projectDirectory);
        }

    }

    public void CoreFilesAdd(string directory)
    {
        var projectPath = _fileProvider.Get("*.csproj", directory);

        var projectDirectory = _fileSystem.GetDirectoryName(projectPath);

        foreach (var file in new List<FileModel>()
        {
            _fileModelFactory.CreateResponseBase(projectDirectory),
            _fileModelFactory.CreateCoreUsings(projectDirectory),
            _fileModelFactory.CreateLinqExtensions(projectDirectory)
        })
        {
            if (!_fileSystem.Exists(file.Path))
                _artifactGenerator.CreateFor(file);
        }
    }

    public void CorePackagesAdd(string directory)
    {
        PackageAdd("MediatR", directory);
        PackageAdd("FluentValidation", directory);
        PackageAdd("Microsoft.EntityFrameworkCore", directory);
        PackageAdd("Microsoft.Extensions.Logging.Abstractions", directory);
    }

    public void CorePackagesAndFiles(string directory)
    {
        CorePackagesAdd(directory);
        CoreFilesAdd(directory);
    }
}

