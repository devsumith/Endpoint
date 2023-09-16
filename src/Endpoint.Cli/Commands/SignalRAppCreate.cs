// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Projects;
using Endpoint.Core.Artifacts.Services;
using Endpoint.Core.Artifacts.Solutions.Factories;
using Endpoint.Core.Artifacts.Solutions.Services;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using static Endpoint.Core.Constants.FileExtensions;

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
    private readonly ILogger<SignalRAppCreateRequestHandler> logger;
    private readonly ISolutionService solutionService;
    private readonly IAngularService angularService;
    private readonly ISolutionFactory solutionFactory;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly INamingConventionConverter namingConventionConverter;
    private readonly ICommandService commandService;
    private readonly IClassFactory classFactory;
    private readonly IFileProvider fileProvider;
    private readonly IFileSystem fileSystem;
    private readonly IFileFactory fileFactory;

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
        IFileFactory fileFactory)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
        this.solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        this.solutionFactory = solutionFactory;
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
    }

    public void HubAdd(ProjectModel projectModel, string name)
    {
        var appBuilderRegistration = $"app.MapHub<{name}.Api.{name}Hub>(\"/hub\");";

        var signalrServiceAddition = "services.AddSignalR();".Indent(2);

        var programPath = Path.Combine(projectModel.Directory, "Program.cs");

        var programContent = new List<string>();

        foreach (var line in fileSystem.File.ReadAllLines(programPath))
        {
            if (line.Contains("app.Run();"))
            {
                programContent.Add(appBuilderRegistration);

                programContent.Add(string.Empty);
            }

            programContent.Add(line);
        }

        fileSystem.File.WriteAllLines(programPath, programContent.ToArray());

        ServiceAdd(projectModel, signalrServiceAddition);
    }

    public void ServiceAdd(ProjectModel projectModel, string serviceRegistration)
    {
        var configureServicePath = Path.Combine(projectModel.Directory, "ConfigureServices.cs");

        var configureServicesContent = new List<string>();

        foreach (var line in fileSystem.File.ReadAllLines(configureServicePath))
        {
            configureServicesContent.Add(line);

            if (line.Contains("services.AddSwaggerGen();"))
            {
                configureServicesContent.Add(serviceRegistration);
            }
        }

        fileSystem.File.WriteAllLines(configureServicePath, configureServicesContent.ToArray());
    }

    public async Task Handle(SignalRAppCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(SignalRAppCreateRequestHandler));

        if (fileSystem.Directory.Exists(Path.Combine(request.Directory, request.Name)))
        {
            fileSystem.Directory.Delete(Path.Combine(request.Directory, request.Name), true);
        }

        var solutionModel = await solutionFactory.Create(request.Name, $"{request.Name}.Api", "webapi", string.Empty, request.Directory);

        var messageModel = classFactory.CreateMessageModel();

        var hubClassModel = classFactory.CreateHubModel(request.Name);

        var interfaceModel = classFactory.CreateHubInterfaceModel(request.Name);

        var projectModel = solutionModel.DefaultProject;

        var workerModel = await classFactory.CreateMessageProducerWorkerAsync(request.Name, projectModel.Directory);

        projectModel.Files.Add(new CodeFileModel<ClassModel>(workerModel, workerModel.Name, projectModel.Directory, CSharpFile));

        projectModel.Files.Add(new CodeFileModel<ClassModel>(messageModel, messageModel.Name, projectModel.Directory, CSharpFile));

        projectModel.Files.Add(new CodeFileModel<ClassModel>(hubClassModel, hubClassModel.Name, projectModel.Directory, CSharpFile));

        projectModel.Files.Add(new CodeFileModel<InterfaceModel>(interfaceModel, interfaceModel.Name, projectModel.Directory, CSharpFile));

        await artifactGenerator.GenerateAsync(solutionModel);

        HubAdd(projectModel, request.Name);

        ServiceAdd(projectModel, $"services.AddHostedService<{request.Name}.Api.MessageProducer>();".Indent(2));

        var temporaryAppName = $"{namingConventionConverter.Convert(NamingConvention.SnakeCase, request.Name)}-app";

        await angularService.CreateWorkspace(temporaryAppName, request.Version, "app", "application", "app", solutionModel.SrcDirectory, false);

        var nameSnakeCase = namingConventionConverter.Convert(NamingConvention.SnakeCase, request.Name);

        var serviceName = $"{nameSnakeCase}-hub-client";

        var appDirectory = Path.Combine(solutionModel.SrcDirectory, temporaryAppName, "projects", "app", "src", "app");

        commandService.Start($"ng g s {serviceName}", appDirectory);

        commandService.Start($"ng g c {namingConventionConverter.Convert(NamingConvention.SnakeCase, request.Name)}", appDirectory);

        commandService.Start("npm install @microsoft/signalr --force", Path.Combine(solutionModel.SrcDirectory, temporaryAppName));

        var mainFileModel = fileFactory
            .CreateTemplate(
            "SignalRAppMain",
            "main",
            Path.Combine(solutionModel.SrcDirectory, temporaryAppName, "projects", "app", "src"),
            TypeScriptFile,
            tokens: new TokensBuilder()
            .With("baseUrl", projectModel.GetApplicationUrl(fileSystem))
            .With("name", request.Name)
            .Build());

        var componentFileModel = fileFactory
            .CreateTemplate(
            "Components.HubClientServiceConsumer.Component",
            Path.Combine(nameSnakeCase, $"{nameSnakeCase}.component"),
            appDirectory,
            TypeScriptFile,
            tokens: new TokensBuilder()
            .With("name", request.Name)
            .Build());

        var componentTemplateFileModel = fileFactory
            .CreateTemplate(
            "Components.HubClientServiceConsumer.Html",
            Path.Combine(nameSnakeCase, $"{nameSnakeCase}.component"),
            appDirectory,
            HtmlFile,
            tokens: new TokensBuilder()
            .With("name", request.Name)
            .With("code", "{{ vm.messages | json }}")
            .Build());

        var hubServiceFileModel = fileFactory
            .CreateTemplate(
            "Services.HubClientService.Service",
            $"{serviceName}.service",
            appDirectory,
            TypeScriptFile,
            tokens: new TokensBuilder()
            .With("name", request.Name)
            .Build());

        var hubServiceSpecFileModel = fileFactory
            .CreateTemplate(
            "Services.HubClientService.Spec",
            $"{serviceName}.service.spec",
            appDirectory,
            TypeScriptFile,
            tokens: new TokensBuilder()
            .With("name", request.Name)
            .Build());

        var appHtmlFileModel = new ContentFileModel("<router-outlet />", "app.component", Path.Combine(solutionModel.SrcDirectory, temporaryAppName, "projects", "app", "src", "app"), HtmlFile);

        await artifactGenerator.GenerateAsync(mainFileModel);

        await artifactGenerator.GenerateAsync(componentTemplateFileModel);

        await artifactGenerator.GenerateAsync(componentFileModel);

        await artifactGenerator.GenerateAsync(hubServiceFileModel);

        await artifactGenerator.GenerateAsync(hubServiceSpecFileModel);

        await artifactGenerator.GenerateAsync(appHtmlFileModel);

        Directory.Move(Path.Combine(solutionModel.SrcDirectory, temporaryAppName), Path.Combine(solutionModel.SrcDirectory, $"{request.Name}.App"));

        commandService.Start("code .", solutionModel.SolutionDirectory);
    }
}