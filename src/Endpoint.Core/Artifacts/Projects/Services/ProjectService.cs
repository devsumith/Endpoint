// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;


namespace Endpoint.Core.Artifacts.Projects.Services;

//https://learn.microsoft.com/en-us/visualstudio/extensibility/internals/solution-dot-sln-file?view=vs-2022

public class ProjectService : IProjectService
{
    private readonly ICommandService _commandService;
    private readonly IFileProvider _fileProvider;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly IFileSystem _fileSystem;
    private readonly IFileFactory _fileFactory;
    public ProjectService(
        IArtifactGenerator artifactGenerator,
        ICommandService commandService,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IFileFactory fileFactory)
    {
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
    }

    public async Task AddProjectAsync(ProjectModel model)
    {
        await _artifactGenerator.GenerateAsync(model);

        await AddToSolution(model);
    }

    public async Task AddToSolution(ProjectModel model)
    {
        var solution = _fileProvider.Get("*.sln", model.Directory);
        var solutionName = Path.GetFileName(solution);
        var solutionDirectory = _fileSystem.Path.GetDirectoryName(solution);

        if(model.Extension == ".csproj")
        {
            _commandService.Start($"dotnet sln {solutionName} add {model.Path}", solutionDirectory);
        }
        else
        {
            var lines = new List<string>();

            var projectEntry = new string[2]
            {
                "Project(\"{" + $"{Guid.NewGuid()}".ToUpper() + "}\") = \"" + model.Name + "\", \"" + model.Path.Replace($"{solutionDirectory}{Path.DirectorySeparatorChar}", string.Empty) + "\", \"{" + $"{Guid.NewGuid()}".ToUpper() + "}\"",
                "EndProject"
            };

            foreach(var line in _fileSystem.File.ReadAllLines(solution))
            {
                lines.Add(line);

                if(line.StartsWith("MinimumVisualStudioVersion"))
                {
                    foreach(var entry in projectEntry)
                    {
                        lines.Add(entry);
                    }
                }
            }

            _fileSystem.File.WriteAllLines(solution, lines.ToArray());
        }
        
    }

    public async Task AddGenerateDocumentationFile(string csprojFilePath)
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

    public async Task AddEndpointPostBuildTargetElement(string csprojFilePath)
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

    public async Task PackageAdd(string name, string directory)
    {
        var projectPath = _fileProvider.Get("*.csproj", directory);

        var projectDirectory = _fileSystem.Path.GetDirectoryName(projectPath);

        var projectFileContents = _fileSystem.File.ReadAllText(projectPath);

        if (!projectFileContents.Contains($"PackageReference Include=\"{name}\""))
        {
            _commandService.Start($"dotnet add package {name}", projectDirectory);
        }

    }

    public async Task CoreFilesAdd(string directory)
    {
        var projectPath = _fileProvider.Get("*.csproj", directory);

        var projectDirectory = _fileSystem.Path.GetDirectoryName(projectPath);

        foreach (var file in new List<FileModel>()
        {
            _fileFactory.CreateResponseBase(projectDirectory),
            _fileFactory.CreateCoreUsings(projectDirectory),
            _fileFactory.CreateLinqExtensions(projectDirectory)
        })
        {
            if (!_fileSystem.File.Exists(file.Path))
                await _artifactGenerator.GenerateAsync(file);
        }
    }

    public async Task CorePackagesAdd(string directory)
    {
        PackageAdd("MediatR", directory);
        PackageAdd("FluentValidation", directory);
        PackageAdd("Microsoft.EntityFrameworkCore", directory);
        PackageAdd("Microsoft.Extensions.Logging.Abstractions", directory);
    }

    public async Task CorePackagesAndFiles(string directory)
    {
        CorePackagesAdd(directory);
        CoreFilesAdd(directory);
    }
}

