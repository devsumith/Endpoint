// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Syntax.Attributes;
using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.SpecFlow;

public class SpecFlowService : ISpecFlowService
{
    private readonly ILogger<SpecFlowService> logger;
    private readonly IFileFactory fileFactory;
    private readonly IArtifactGenerator artifactGenerator;

    public SpecFlowService(
        ILogger<SpecFlowService> logger,
        IArtifactGenerator artifactGenerator,
        IFileFactory fileFactory)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
    }

    public async Task CreatePageObject(string name, string directory)
    {
        logger.LogInformation("Create Page Object: {name}", name);

        var pageObjectName = $"{name}PageObject";

        var classModel = new ClassModel(pageObjectName);

        classModel.Attributes.Add(new AttributeModel()
        {
            Name = "Binding",
        });

        classModel.Usings.Add(new UsingModel()
        {
            Name = "TechTalk.SpecFlow",
        });

        var fileModel = new CodeFileModel<ClassModel>(classModel, classModel.Usings, classModel.Name, directory, ".cs");

        await artifactGenerator.GenerateAsync(fileModel);
    }

    public async Task CreateHook(string name, string directory)
    {
        logger.LogInformation("Create Hook: {name}", name);

        var hookName = $"{name}Hooks";

        var classModel = new ClassModel(hookName);

        classModel.Attributes.Add(new AttributeModel()
        {
            Name = "Binding",
        });

        classModel.Usings.Add(new ("TechTalk.SpecFlow"));

        var fileModel = new CodeFileModel<ClassModel>(classModel, classModel.Usings, classModel.Name, directory, ".cs");

        await artifactGenerator.GenerateAsync(fileModel);
    }

    public async Task CreateDockerControllerHooks(string directory)
    {
        var model = fileFactory.CreateTemplate("DockerControllerHooks", "DockerControllerHooks", directory);

        await artifactGenerator.GenerateAsync(model);
    }
}