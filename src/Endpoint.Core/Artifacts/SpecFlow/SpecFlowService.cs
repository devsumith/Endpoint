// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Attributes;
using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Endpoint.Core.Artifacts.SpecFlow;

public class SpecFlowService : ISpecFlowService
{
    private readonly ILogger<SpecFlowService> _logger;
    private readonly IFileFactory _fileFactory;
    private readonly IArtifactGenerator _artifactGenerator;
    public SpecFlowService(
        ILogger<SpecFlowService> logger,
        IArtifactGenerator artifactGenerator,
        IFileFactory fileFactory
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
    }

    public async Task CreatePageObject(string name, string directory)
    {
        _logger.LogInformation("Create Page Object: {name}", name);

        var pageObjectName = $"{name}PageObject";

        var classModel = new ClassModel(pageObjectName);

        classModel.Attributes.Add(new AttributeModel()
        {
            Name = "Binding"
        });

        classModel.UsingDirectives.Add(new UsingDirectiveModel()
        {
            Name = "TechTalk.SpecFlow"
        });

        var fileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, directory, ".cs");

        await _artifactGenerator.GenerateAsync(fileModel);
    }

    public async Task CreateHook(string name, string directory)
    {
        _logger.LogInformation("Create Hook: {name}", name);

        var hookName = $"{name}Hooks";

        var classModel = new ClassModel(hookName);

        classModel.Attributes.Add(new AttributeModel()
        {
            Name = "Binding"
        });

        classModel.UsingDirectives.Add(new("TechTalk.SpecFlow"));

        var fileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, directory, ".cs");

        await _artifactGenerator.GenerateAsync(fileModel);
    }

    public async Task CreateDockerControllerHooks(string directory)
    {
        var model = _fileFactory.CreateTemplate("DockerControllerHooks", "DockerControllerHooks", directory);

        await _artifactGenerator.GenerateAsync(model);
    }
}