// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Entities;
using Endpoint.Core.Syntax.Methods.Factories;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Projects.Services;

public class ApiProjectService : IApiProjectService
{
    private readonly ILogger<ApiProjectService> logger;
    private readonly IFileProvider fileProvider;
    private readonly IFileSystem fileSystem;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly IFileFactory fileFactory;
    private readonly IClassFactory classFactory;
    private readonly ISyntaxGenerator syntaxGenerator;
    private readonly IMethodFactory methodFactory;
    private readonly IClipboardService clipboardService;
    private readonly ICodeAnalysisService codeAnalysisService;

    public ApiProjectService(
        ILogger<ApiProjectService> logger,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IArtifactGenerator artifactGenerator,
        IFileFactory fileFactory,
        IClassFactory classFactory,
        ISyntaxGenerator syntaxGenerator,
        IMethodFactory methodFactory,
        IClipboardService clipboardService,
        ICodeAnalysisService codeAnalysisService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        this.syntaxGenerator = syntaxGenerator ?? throw new ArgumentNullException(nameof(syntaxGenerator));
        this.methodFactory = methodFactory ?? throw new ArgumentNullException(nameof(methodFactory));
        this.clipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));
        this.codeAnalysisService = codeAnalysisService ?? throw new ArgumentNullException(nameof(codeAnalysisService));
    }

    public async Task ControllerCreateAsync(string entityName, bool empty, string directory)
    {
        logger.LogInformation("Controller Add");

        var entity = new EntityModel(entityName);

        await artifactGenerator.GenerateAsync(new ProjectReferenceModel()
        {
            ReferenceDirectory = directory,
        });

        var csProjPath = fileProvider.Get("*.csproj", directory);

        var csProjDirectory = Path.GetDirectoryName(csProjPath);

        var controllersDirectory = fileSystem.Path.Combine(csProjDirectory, "Controllers");

        fileSystem.Directory.CreateDirectory(controllersDirectory);

        var controllerClassModel = empty ? classFactory.CreateEmptyController(entityName, csProjDirectory) : classFactory.CreateController(entity, csProjDirectory);

        await artifactGenerator.GenerateAsync(fileFactory.CreateCSharp(controllerClassModel, controllersDirectory));
    }

    public async Task ControllerMethodAdd(string name, string controller, string route, string directory)
    {
        var model = methodFactory.CreateControllerMethod(name, controller, route switch
        {
            "create" => RouteType.Create,
            "update" => RouteType.Update,
            "delete" => RouteType.Delete,
            "get" => RouteType.Get,
            "getbyid" => RouteType.GetById,
            "page" => RouteType.Page,
            "httpget" => RouteType.HttpGet,
            "httpput" => RouteType.HttpPut,
            "httpdelete" => RouteType.HttpDelete,
            "httppost" => RouteType.HttpPost,
            _ => throw new NotImplementedException()
        }, directory);

        var syntax = await syntaxGenerator.GenerateAsync(model);

        clipboardService.SetText(syntax);

        logger.LogInformation(syntax);
    }

    private async Task AddApiFiles(string serviceName, string directory)
    {
        var tokens = new TokensBuilder()
            .With("serviceName", serviceName)
            .With("DbContextName", $"{serviceName}DbContext")
            .With("Port", "5001")
            .With("SslPort", "5000")
            .Build();

        var configureServiceFile = fileFactory.CreateTemplate("Api.ConfigureServices", "ConfigureServices", directory, ".cs", tokens: tokens);

        var appSettingsFile = fileFactory.CreateTemplate("Api.AppSettings", "appsettings", directory, ".json", tokens: tokens);

        var launchSettingsFile = fileFactory.CreateTemplate("Api.LaunchSettings", "launchsettings", directory, ".json", tokens: tokens);

        foreach (var file in new FileModel[]
        {
            configureServiceFile,
            appSettingsFile,
            launchSettingsFile,
        })
        {
            await artifactGenerator.GenerateAsync(file);
        }
    }
}
