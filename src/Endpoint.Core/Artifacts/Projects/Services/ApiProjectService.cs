// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Projects.Commands;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Entities;
using Endpoint.Core.Syntax.Methods.Factories;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Endpoint.Core.Artifacts.Projects.Services;

public class ApiProjectService : IApiProjectService
{
    private readonly ILogger<ApiProjectService> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly IFileFactory _fileModelFactory;
    private readonly IClassModelFactory _classModelFactory;
    private readonly ISyntaxGenerator _syntaxGenerator;
    private readonly IMethodModelFactory _methodModelFactory;
    private readonly IClipboardService _clipboardService;
    public ApiProjectService(
        ILogger<ApiProjectService> logger,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IArtifactGenerator artifactGenerator,
        IFileFactory fileModelFactory,
        IClassModelFactory classModelFactory,
        ISyntaxGenerator syntaxGenerator,
        IMethodModelFactory methodModelFactory,
        IClipboardService clipboardService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));
        _syntaxGenerator = syntaxGenerator ?? throw new ArgumentNullException(nameof(syntaxGenerator));
        _methodModelFactory = methodModelFactory ?? throw new ArgumentNullException(nameof(methodModelFactory));
        _clipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));
    }

    public async Task ControllerAdd(string entityName, bool empty, string directory)
    {
        _logger.LogInformation("Controller Add");

        var entity = new EntityModel(entityName);

        await _artifactGenerator.CreateAsync(new ProjectReferenceModel()
        {
            ReferenceDirectory = directory
        }, new { Command = new ApiProjectEnsure() });

        var csProjPath = _fileProvider.Get("*.csproj", directory);

        var csProjDirectory = Path.GetDirectoryName(csProjPath);

        var controllersDirectory = $"{csProjDirectory}{Path.DirectorySeparatorChar}Controllers";

        _fileSystem.CreateDirectory(controllersDirectory);

        var controllerClassModel = empty ? _classModelFactory.CreateEmptyController(entityName, csProjDirectory) : _classModelFactory.CreateController(entity, csProjDirectory);

        await _artifactGenerator.CreateAsync(_fileModelFactory.CreateCSharp(controllerClassModel, controllersDirectory));
    }

    private async Task AddApiFiles(string serviceName, string directory)
    {
        var tokens = new TokensBuilder()
            .With("serviceName", serviceName)
            .With("DbContextName", $"{serviceName}DbContext")
            .With("Port", "5001")
            .With("SslPort", "5000")
            .Build();

        var configureServiceFile = _fileModelFactory.CreateTemplate("Api.ConfigureServices", "ConfigureServices", directory, ".cs", tokens: tokens);

        var appSettingsFile = _fileModelFactory.CreateTemplate("Api.AppSettings", "appsettings", directory, "json", tokens: tokens);

        var launchSettingsFile = _fileModelFactory.CreateTemplate("Api.LaunchSettings", "launchsettings", directory, "json", tokens: tokens);


        foreach (var file in new FileModel[] {
            configureServiceFile,
            appSettingsFile,
            launchSettingsFile
        })
        {
            await _artifactGenerator.CreateAsync(file);
        }
    }

    public async Task ControllerMethodAdd(string name, string controller, string route, string directory)
    {
        var model = _methodModelFactory.CreateControllerMethod(name, controller, route switch
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

        var syntax = await _syntaxGenerator.CreateAsync(model);

        _clipboardService.SetText(syntax);

        _logger.LogInformation(syntax);
    }
}


