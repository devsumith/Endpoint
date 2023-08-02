// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Models.Artifacts.Projects;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Classes.Factories;
using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Models.WebArtifacts.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
    private readonly ISolutionModelFactory _solutionModelFactory;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ICommandService _commandService;
    private readonly IClassModelFactory _classModelFactory;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IPlaywrightService _playwrightService;

    public SignalRAppCreateRequestHandler(
        ILogger<SignalRAppCreateRequestHandler> logger,
        ISolutionService solutionService,
        IAngularService angularService,
        ISolutionModelFactory solutionModelFactory,
        IArtifactGenerator artifactGenerator,
        INamingConventionConverter namingConventionConverter,
        ICommandService commandService,
        IClassModelFactory classModelFactory,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IFileModelFactory fileModelFactory,
        IPlaywrightService playwrightService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        _solutionModelFactory = solutionModelFactory;
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter)); ;
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
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

        var solutionModel = _solutionModelFactory.Create(request.Name, $"{request.Name}.Api", "webapi", string.Empty, request.Directory);

        var messageModel = _classModelFactory.CreateMessageModel();

        var hubClassModel = _classModelFactory.CreateHubModel(request.Name);

        var interfaceModel = _classModelFactory.CreateHubInterfaceModel(request.Name);

        var projectModel = solutionModel.DefaultProject;

        var workerModel = _classModelFactory.CreateMessageProducerWorkerModel(request.Name, projectModel.Directory);

        projectModel.Files.Add(new ObjectFileModel<ClassModel>(workerModel, workerModel.UsingDirectives, workerModel.Name, projectModel.Directory, "cs"));

        projectModel.Files.Add(new ObjectFileModel<ClassModel>(messageModel, messageModel.UsingDirectives, messageModel.Name, projectModel.Directory, "cs"));

        projectModel.Files.Add(new ObjectFileModel<ClassModel>(hubClassModel, hubClassModel.UsingDirectives, hubClassModel.Name, projectModel.Directory, "cs"));

        projectModel.Files.Add(new ObjectFileModel<InterfaceModel>(interfaceModel, interfaceModel.Name, projectModel.Directory, "cs"));

        _artifactGenerator.CreateFor(solutionModel);

        HubAdd(projectModel, request.Name);

        ServiceAdd(projectModel, $"services.AddHostedService<{request.Name}.Api.MessageProducer>();".Indent(2));

        var temporaryAppName = $"{_namingConventionConverter.Convert(NamingConvention.SnakeCase, request.Name)}-app";

        _angularService.CreateWorkspace(temporaryAppName, request.Version, "app", "application", "app", solutionModel.SrcDirectory, false);

        var nameSnakeCase = _namingConventionConverter.Convert(NamingConvention.SnakeCase, request.Name);

        var serviceName = $"{nameSnakeCase}-hub-client";

        var appDirectory = Path.Combine(solutionModel.SrcDirectory, temporaryAppName, "projects", "app", "src", "app");

        _commandService.Start($"ng g s {serviceName}", appDirectory);

        _commandService.Start($"ng g c {_namingConventionConverter.Convert(NamingConvention.SnakeCase, request.Name)}", appDirectory);

        _commandService.Start("npm install @microsoft/signalr", Path.Combine(solutionModel.SrcDirectory, temporaryAppName));

        var mainFileModel = _fileModelFactory
            .CreateTemplate(
            "SignalRAppMain",
            "main",
            Path.Combine(solutionModel.SrcDirectory, temporaryAppName, "projects", "app", "src"),
            "ts",
            tokens: new TokensBuilder()
            .With("baseUrl", projectModel.GetApplicationUrl(_fileSystem))
            .With("name", request.Name)
            .Build());

        var componentFileModel = _fileModelFactory
            .CreateTemplate(
            "Components.HubClientServiceConsumer.Component",
            $"{nameSnakeCase}{Path.DirectorySeparatorChar}{nameSnakeCase}.component",
            appDirectory,
            "ts",
            tokens: new TokensBuilder()
            .With("name", request.Name)
            .Build());

        var componentTemplateFileModel = _fileModelFactory
            .CreateTemplate(
            "Components.HubClientServiceConsumer.Html",
            $"{nameSnakeCase}{Path.DirectorySeparatorChar}{nameSnakeCase}.component",
            appDirectory,
            "html",
            tokens: new TokensBuilder()
            .With("name", request.Name)
            .With("code", "{{ vm.messages | json }}")
            .Build());

        var hubServiceFileModel = _fileModelFactory
            .CreateTemplate(
            "Services.HubClientService.Service",
            $"{serviceName}.service",
            appDirectory,
            "ts",
            tokens: new TokensBuilder()
            .With("name", request.Name)
            .Build());

        var hubServiceSpecFileModel = _fileModelFactory
            .CreateTemplate(
            "Services.HubClientService.Spec",
            $"{serviceName}.service.spec",
            appDirectory,
            "ts",
            tokens: new TokensBuilder()
            .With("name", request.Name)
            .Build());

        var appHtmlFileModel = new ContentFileModel("<router-outlet />", "app.component", Path.Combine(solutionModel.SrcDirectory, temporaryAppName, "projects", "app", "src", "app"), "html");

        _artifactGenerator.CreateFor(mainFileModel);

        _artifactGenerator.CreateFor(componentTemplateFileModel);

        _artifactGenerator.CreateFor(componentFileModel);

        _artifactGenerator.CreateFor(hubServiceFileModel);

        _artifactGenerator.CreateFor(hubServiceSpecFileModel);

        _artifactGenerator.CreateFor(appHtmlFileModel);

        Directory.Move(Path.Combine(solutionModel.SrcDirectory, temporaryAppName), Path.Combine(solutionModel.SrcDirectory, $"{request.Name}.App"));

        PlaywrightCreate(request.Name, solutionModel.SrcDirectory);

        _commandService.Start("code .", solutionModel.SolutionDirectory);

    }
}