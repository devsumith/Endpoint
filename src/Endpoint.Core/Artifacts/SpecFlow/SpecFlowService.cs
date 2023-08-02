// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Attributes;
using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.SpecFlow;

public class SpecFlowService : ISpecFlowService
{
    private readonly ILogger<SpecFlowService> _logger;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IArtifactGenerator _artifactGenerator;
    public SpecFlowService(
        ILogger<SpecFlowService> logger,
        IArtifactGenerator artifactGenerator,
        IFileModelFactory fileModelFactory
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
    }

    public void CreatePageObject(string name, string directory)
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

        var fileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, directory, "cs");

        _artifactGenerator.CreateFor(fileModel);
    }

    public void CreateHook(string name, string directory)
    {
        _logger.LogInformation("Create Hook: {name}", name);

        var hookName = $"{name}Hooks";

        var classModel = new ClassModel(hookName);

        classModel.Attributes.Add(new AttributeModel()
        {
            Name = "Binding"
        });

        classModel.UsingDirectives.Add(new("TechTalk.SpecFlow"));

        var fileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, directory, "cs");

        _artifactGenerator.CreateFor(fileModel);
    }

    public void CreateDockerControllerHooks(string directory)
    {
        var model = _fileModelFactory.CreateTemplate("DockerControllerHooks", "DockerControllerHooks", directory);

        _artifactGenerator.CreateFor(model);
    }
}