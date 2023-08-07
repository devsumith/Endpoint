// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Projects;
using Endpoint.Core.Artifacts.Solutions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Interfaces;
using Endpoint.Core.Artifacts.Services;

namespace Endpoint.Cli.Commands;


[Verb("signalr-app-create")]
public class SignalRAppCreateRequest : IRequest
{
    [Option('n', "name", HelpText = "Name of Asp.Net Core SignalR Solution and Angular App")]
    public string Name { get; set; }

    [Option('v', "version", HelpText = "Version of Angular Cli")]
    public string Version { get; set; } = "latest";

    [Option('d', Required = false, HelpText = "Output directory")]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SignalRAppCreateRequestHandler : IRequestHandler<SignalRAppCreateRequest>
{
    private readonly ILogger<SignalRAppCreateRequestHandler> _logger;
    private readonly ISolutionService _solutionService;
    private readonly IAngularService _angularService;
    private readonly ISolutionFactory _solutionFactory;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ICommandService _commandService;
    private readonly IClassFactory _classFactory;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly IFileFactory _fileFactory;
    private readonly IPlaywrightService _playwrightService;

    public SignalRAppCreateRequestHandler(
        ILogger<SignalRAppCreateRequestHandler> logger,
        ISolutionService solutionService,
        IAngularService angularService,
        ISolutionFactory solutionFactory,
        IArtifactGenerator artifactGenerator,
        INamingConventionConverter namingConventionConverter,
        ICommandService commandService,
        IClassFactory classFactory,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IFileFactory fileFactory,
        IPlaywrightService playwrightService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        _solutionFactory = solutionFactory;
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter)); ;
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        _playwrightService = playwrightService ?? throw new ArgumentNullException(nameof(playwrightService));
    }

    public void PlaywrightCreate(string name, string directory)
    {
        var e2eDirectory = Path.Combine(directory, $"{name}.E2e");

        Directory.CreateDirectory(e2eDirectory);

        _playwrightService.Create(e2eDirectory);
    }

    public void HubAdd(ProjectModel projectModel, string name)
    {
        var appBuilderRegistration = $"app.MapHub<{name}.Api.{name}Hub>(\"/hub\");";

        var signalrServiceAddition = "services.AddSignalR();".Indent(2);

        var programPath = Path.Combine(projectModel.Directory, "Program.cs");

        var programContent = new List<string>();

        foreach (var line in _fileSystem.ReadAllLines(programPath))
        {
            if (line.Contains("app.Run();"))
            {
                programContent.Add(appBuilderRegistration);

                programContent.Add("");
            }

            programContent.Add(line);
        }

        _fileSystem.WriteAllLines(programPath, programContent.ToArray());

        ServiceAdd(projectModel, signalrServiceAddition);
    }

    public void ServiceAdd(ProjectModel projectModel, string serviceRegistration)
    {
        var configureServicePath = Path.Combine(projectModel.Directory, "ConfigureServices.cs");

        var configureServicesContent = new List<string>();

        foreach (var line in _fileSystem.ReadAllLines(configureServicePath))
        {
            configureServicesContent.Add(line);

            if (line.Contains("services.AddSwaggerGen();"))
            {
                configureServicesContent.Add(serviceRegistration);
            }
        }

        _fileSystem.WriteAllLines(configureServicePath, configureServicesContent.ToArray());
    }

    public async Task Handle(SignalRAppCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SignalRAppCreateRequestHandler));

        var solutionModel = await _solutionFactory.Create(request.Name, $"{request.Name}.Api", "webapi", string.Empty, request.Directory);

        var messageModel = _classFactory.CreateMessageModel();

        var hubClassModel = _classFactory.CreateHubModel(request.Name);

        var interfaceModel = _classFactory.CreateHubInterfaceModel(request.Name);

        var projectModel = solutionModel.DefaultProject;

        var workerModel = _classFactory.CreateMessageProducerWorkerModel(request.Name, projectModel.Directory);

        projectModel.Files.Add(new ObjectFileModel<ClassModel>(workerModel, workerModel.UsingDirectives, workerModel.Name, projectModel.Directory, ".cs"));

        projectModel.Files.Add(new ObjectFileModel<ClassModel>(messageModel, messageModel.UsingDirectives, messageModel.Name, projectModel.Directory, ".cs"));

        projectModel.Files.Add(new ObjectFileModel<ClassModel>(hubClassModel, hubClassModel.UsingDirectives, hubClassModel.Name, projectModel.Directory, ".cs"));

        projectModel.Files.Add(new ObjectFileModel<InterfaceModel>(interfaceModel, interfaceModel.Name, projectModel.Directory, ".cs"));

        await _artifactGenerator.GenerateAsync(solutionModel);

        HubAdd(projectModel, request.Name);

        ServiceAdd(projectModel, $"services.AddHostedService<{request.Name}.Api.MessageProducer>();".Indent(2));

        var temporaryAppName = $"{_namingConventionConverter.Convert(NamingConvention.SnakeCase, request.Name)}-app";

        await _angularService.CreateWorkspace(temporaryAppName, request.Version, "app", "application", "app", solutionModel.SrcDirectory, false);

        var nameSnakeCase = _namingConventionConverter.Convert(NamingConvention.SnakeCase, request.Name);

        var serviceName = $"{nameSnakeCase}-hub-client";

        var appDirectory = Path.Combine(solutionModel.SrcDirectory, temporaryAppName, "projects", "app", "src", "app");

        _commandService.Start($"ng g s {serviceName}", appDirectory);

        _commandService.Start($"ng g c {_namingConventionConverter.Convert(NamingConvention.SnakeCase, request.Name)}", appDirectory);

        _commandService.Start("npm install @microsoft/signalr", Path.Combine(solutionModel.SrcDirectory, temporaryAppName));

        var mainFileModel = _fileFactory
            .CreateTemplate(
            "SignalRAppMain",
            "main",
            Path.Combine(solutionModel.SrcDirectory, temporaryAppName, "projects", "app", "src"),
            ".ts",
            tokens: new TokensBuilder()
            .With("baseUrl", projectModel.GetApplicationUrl(_fileSystem))
            .With("name", request.Name)
            .Build());

        var componentFileModel = _fileFactory
            .CreateTemplate(
            "Components.HubClientServiceConsumer.Component",
            $"{nameSnakeCase}{Path.DirectorySeparatorChar}{nameSnakeCase}.component",
            appDirectory,
            ".ts",
            tokens: new TokensBuilder()
            .With("name", request.Name)
            .Build());

        var componentTemplateFileModel = _fileFactory
            .CreateTemplate(
            "Components.HubClientServiceConsumer.Html",
            $"{nameSnakeCase}{Path.DirectorySeparatorChar}{nameSnakeCase}.component",
            appDirectory,
            ".html",
            tokens: new TokensBuilder()
            .With("name", request.Name)
            .With("code", "{{ vm.messages | json }}")
            .Build());

        var hubServiceFileModel = _fileFactory
            .CreateTemplate(
            "Services.HubClientService.Service",
            $"{serviceName}.service",
            appDirectory,
            ".ts",
            tokens: new TokensBuilder()
            .With("name", request.Name)
            .Build());

        var hubServiceSpecFileModel = _fileFactory
            .CreateTemplate(
            "Services.HubClientService.Spec",
            $"{serviceName}.service.spec",
            appDirectory,
            ".ts",
            tokens: new TokensBuilder()
            .With("name", request.Name)
            .Build());

        var appHtmlFileModel = new ContentFileModel("<router-outlet />", "app.component", Path.Combine(solutionModel.SrcDirectory, temporaryAppName, "projects", "app", "src", "app"), ".html");

        await _artifactGenerator.GenerateAsync(mainFileModel);

        await _artifactGenerator.GenerateAsync(componentTemplateFileModel);

        await _artifactGenerator.GenerateAsync(componentFileModel);

        await _artifactGenerator.GenerateAsync(hubServiceFileModel);

        await _artifactGenerator.GenerateAsync(hubServiceSpecFileModel);

        await _artifactGenerator.GenerateAsync(appHtmlFileModel);

        Directory.Move(Path.Combine(solutionModel.SrcDirectory, temporaryAppName), Path.Combine(solutionModel.SrcDirectory, $"{request.Name}.App"));

        PlaywrightCreate(request.Name, solutionModel.SrcDirectory);

        _commandService.Start("code .", solutionModel.SolutionDirectory);

    }
}