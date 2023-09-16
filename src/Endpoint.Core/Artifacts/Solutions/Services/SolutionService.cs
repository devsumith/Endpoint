// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Folders;
using Endpoint.Core.Artifacts.Projects.Enums;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Internals;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Services;
using MediatR;

namespace Endpoint.Core.Artifacts.Solutions.Services;

public class SolutionService : ISolutionService
{
    private readonly IArtifactGenerator artifactGenerator;
    private readonly IProjectFactory projectFactory;
    private readonly IDomainDrivenDesignFileService domainDrivenDesignFileService;
    private readonly IDomainDrivenDesignService domainDrivenDesignService;
    private readonly Observable<INotification> notificationListener;
    private readonly IFileProvider fileProvider;
    private readonly ICommandService commandService;

    public SolutionService(
        IArtifactGenerator artifactGenerator,
        IProjectFactory projectFactory,
        IDomainDrivenDesignFileService domainDrivenDesignFileService,
        IDomainDrivenDesignService domainDrivenDesignService,
        Observable<INotification> notificationListener,
        IFileProvider fileProvider,
        ICommandService commandService)
    {
        this.artifactGenerator = artifactGenerator;
        this.projectFactory = projectFactory;
        this.domainDrivenDesignFileService = domainDrivenDesignFileService;
        this.domainDrivenDesignService = domainDrivenDesignService;
        this.notificationListener = notificationListener;
        this.fileProvider = fileProvider;
        this.commandService = commandService;
    }

    public async Task AddSolutionItem(string path)
    {
        throw new NotImplementedException();
    }

    public async Task Create(SolutionModel model)
    {
        await artifactGenerator.GenerateAsync(model);
    }

    public async Task EventDrivenMicroservicesCreate(string name, string services, string directory)
    {
        var notifications = new List<INotification>();

        var solutionModel = new SolutionModel(name, directory);

        solutionModel.Folders.Add(await BuildingBlocksCreate(solutionModel.SolutionDirectory));

        solutionModel.Folders.Add(await ServicesCreate(services, solutionModel.SolutionDirectory, notifications));

        await artifactGenerator.GenerateAsync(solutionModel);

        foreach (var notification in notifications)
        {
            notificationListener.Broadcast(notification);
        }
    }

    public async Task Create(string name, string plantUmlSourcePath, string directory)
    {
    }

    private async Task<FolderModel> BuildingBlocksCreate(string directory)
    {
        var model = new FolderModel("BuildingBlocks", directory);

        var messagingFolder = new FolderModel("Messaging", model.Directory);

        model.Projects.Add(await projectFactory.CreateKernelProject(model.Directory));

        messagingFolder.Projects.Add(await projectFactory.CreateMessagingProject(messagingFolder.Directory));

        messagingFolder.Projects.Add(await projectFactory.CreateMessagingUdpProject(messagingFolder.Directory));

        model.SubFolders.Add(messagingFolder);

        return model;
    }

    private async Task<FolderModel> ServicesCreate(string services, string directory, List<INotification> notifications)
    {
        var model = new FolderModel("Services", directory);

        if (string.IsNullOrEmpty(services))
        {
            return model;
        }

        foreach (var service in services.Split(','))
        {
            var serviceFolder = new FolderModel(service, model.Directory);

            var coreModel = await projectFactory.CreateLibrary($"{service}.Core", serviceFolder.Directory, new List<string>()
            {
                Constants.ProjectType.Core,
            });

            var serviceBusMessageConsumer = domainDrivenDesignService.ServiceBusMessageConsumerCreate($"{coreModel.Name}.Messages", coreModel.Directory);

            var fileModel = new CodeFileModel<ClassModel>(serviceBusMessageConsumer, serviceBusMessageConsumer.Usings, serviceBusMessageConsumer.Name, coreModel.Directory, ".cs");

            coreModel.Files.Add(fileModel);

            coreModel.References.Add(@"..\..\..\BuildingBlocks\Messaging\Messaging.Udp\Messaging.Udp.csproj");

            serviceFolder.Projects.Add(coreModel);

            var infrastructureModel = await projectFactory.CreateLibrary($"{service}.Infrastructure", serviceFolder.Directory, new List<string>()
            {
                Constants.ProjectType.Infrastructure,
            });

            infrastructureModel.References.Add(@$"..\{service}.Core\{service}.Core.csproj");

            serviceFolder.Projects.Add(infrastructureModel);

            var apiProjectModel = await projectFactory.CreateLibrary($"{service}.Api", serviceFolder.Directory, new List<string>()
            {
                Constants.ProjectType.Api,
            });

            apiProjectModel.References.Add(@$"..\{service}.Infrastructure\{service}.Infrastructure.csproj");

            apiProjectModel.DotNetProjectType = DotNetProjectType.Web;

            serviceFolder.Projects.Add(apiProjectModel);

            model.SubFolders.Add(serviceFolder);
        }

        return model;
    }

    private FolderModel AppsCreate(string directory)
    {
        var model = new FolderModel("Apps", directory);

        return model;
    }

    public async Task MessagingBuildingBlockAdd(string directory)
    {
        var solutionPath = fileProvider.Get("*.sln", directory);

        var solutionName = Path.GetFileName(solutionPath);

        var solutionDirectory = Path.GetDirectoryName(solutionPath);

        var buildingBlocksDirectory = Path.Combine(solutionDirectory, "src", "BuildingBlocks");

        var messagingDirectory = Path.Combine(buildingBlocksDirectory, "Messaging");

        if (!Directory.Exists(buildingBlocksDirectory))
        {
            Directory.CreateDirectory(buildingBlocksDirectory);
        }

        if (!Directory.Exists(messagingDirectory))
        {
            Directory.CreateDirectory(messagingDirectory);
        }

        var messagingProjectModel = await projectFactory.CreateMessagingProject(messagingDirectory);

        await artifactGenerator.GenerateAsync(messagingProjectModel);

        commandService.Start($"dotnet sln {solutionName} add {messagingProjectModel.Path}", solutionDirectory);

        var messagingUdpProjectModel = await projectFactory.CreateMessagingUdpProject(messagingDirectory);

        await artifactGenerator.GenerateAsync(messagingUdpProjectModel);

        commandService.Start($"dotnet sln {solutionName} add {messagingUdpProjectModel.Path}", solutionDirectory);
    }

    public async Task IOCompressionBuildingBlockAdd(string directory)
    {
        var solutionPath = fileProvider.Get("*.sln", directory);

        var solutionName = Path.GetFileName(solutionPath);

        var solutionDirectory = Path.GetDirectoryName(solutionPath);

        var buildingBlocksDirectory = Path.Combine(solutionDirectory, "src", "BuildingBlocks");

        var messagingDirectory = Path.Combine(buildingBlocksDirectory, "IO.Compression");

        if (!Directory.Exists(buildingBlocksDirectory))
        {
            Directory.CreateDirectory(buildingBlocksDirectory);
        }

        if (!Directory.Exists(messagingDirectory))
        {
            Directory.CreateDirectory(messagingDirectory);
        }

        var messagingProjectModel = await projectFactory.CreateMessagingProject(messagingDirectory);

        await artifactGenerator.GenerateAsync(messagingProjectModel);

        commandService.Start($"dotnet sln {solutionName} add {messagingProjectModel.Path}", solutionDirectory);

        var messagingUdpProjectModel = await projectFactory.CreateMessagingUdpProject(messagingDirectory);

        await artifactGenerator.GenerateAsync(messagingUdpProjectModel);

        commandService.Start($"dotnet sln {solutionName} add {messagingUdpProjectModel.Path}", solutionDirectory);
    }
}
