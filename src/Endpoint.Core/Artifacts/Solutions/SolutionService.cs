// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Folders;
using Endpoint.Core.Artifacts.Projects.Enums;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Internals;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Services;
using MediatR;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Artifacts.Solutions;

public class SolutionService : ISolutionService
{
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly IPlantUmlParserStrategyFactory _plantUmlParserStrategyFactory;
    private readonly IProjectFactory _projectFactory;
    private readonly IDomainDrivenDesignFileService _domainDrivenDesignFileService;
    private readonly IDomainDrivenDesignService _domainDrivenDesignService;
    private readonly Observable<INotification> _notificationListener;
    private readonly IFileProvider _fileProvider;
    private readonly ICommandService _commandService;

    public SolutionService(
        IArtifactGenerator artifactGenerator,
        IPlantUmlParserStrategyFactory plantUmlParserStrategyFactory,
        IProjectFactory projectFactory,
        IDomainDrivenDesignFileService domainDrivenDesignFileService,
        IDomainDrivenDesignService domainDrivenDesignService,
        Observable<INotification> notificationListener,
        IFileProvider fileProvider,
        ICommandService commandService)
    {
        _artifactGenerator = artifactGenerator;
        _plantUmlParserStrategyFactory = plantUmlParserStrategyFactory;
        _projectFactory = projectFactory;
        _domainDrivenDesignFileService = domainDrivenDesignFileService;
        _domainDrivenDesignService = domainDrivenDesignService;
        _notificationListener = notificationListener;
        _fileProvider = fileProvider;
        _commandService = commandService;
    }

    public async Task AddSolutionItem(string path)
    {
        throw new NotImplementedException();
    }

    public async Task Create(SolutionModel model)
    {
        await _artifactGenerator.GenerateAsync(model);
    }

    public async Task EventDrivenMicroservicesCreate(string name, string services, string directory)
    {
        var notifications = new List<INotification>();

        var solutionModel = new SolutionModel(name, directory);

        solutionModel.Folders.Add(await BuildingBlocksCreate(solutionModel.SolutionDirectory));

        solutionModel.Folders.Add(await ServicesCreate(services, solutionModel.SolutionDirectory, notifications));

        await _artifactGenerator.GenerateAsync(solutionModel);

        foreach (var notification in notifications)
        {
            _notificationListener.Broadcast(notification);
        }
    }

    private async Task<FolderModel> BuildingBlocksCreate(string directory)
    {
        var model = new FolderModel("BuildingBlocks", directory);

        var messagingFolder = new FolderModel("Messaging", model.Directory);

        model.Projects.Add(await _projectFactory.CreateKernelProject(model.Directory));

        messagingFolder.Projects.Add(await _projectFactory.CreateMessagingProject(messagingFolder.Directory));

        messagingFolder.Projects.Add(await _projectFactory.CreateMessagingUdpProject(messagingFolder.Directory));

        model.SubFolders.Add(messagingFolder);

        return model;
    }

    private async Task<FolderModel> ServicesCreate(string services, string directory, List<INotification> notifications)
    {
        var model = new FolderModel("Services", directory);

        if (string.IsNullOrEmpty(services))
            return model;


        foreach (var service in services.Split(','))
        {
            var serviceFolder = new FolderModel(service, model.Directory);

            var coreModel = await _projectFactory.CreateLibrary($"{service}.Core", serviceFolder.Directory, new List<string>()
            {
                Constants.ProjectType.Core
            });

            var serviceBusMessageConsumer = _domainDrivenDesignService.ServiceBusMessageConsumerCreate($"{coreModel.Name}.Messages", coreModel.Directory);

            var fileModel = new CodeFileModel<ClassModel>(serviceBusMessageConsumer, serviceBusMessageConsumer.Usings, serviceBusMessageConsumer.Name, coreModel.Directory, ".cs");

            coreModel.Files.Add(fileModel);

            coreModel.References.Add(@"..\..\..\BuildingBlocks\Messaging\Messaging.Udp\Messaging.Udp.csproj");

            serviceFolder.Projects.Add(coreModel);

            var infrastructureModel = await _projectFactory.CreateLibrary($"{service}.Infrastructure", serviceFolder.Directory, new List<string>()
            {
                Constants.ProjectType.Infrastructure
            });

            infrastructureModel.References.Add(@$"..\{service}.Core\{service}.Core.csproj");

            serviceFolder.Projects.Add(infrastructureModel);

            var apiProjectModel = await _projectFactory.CreateLibrary($"{service}.Api", serviceFolder.Directory, new List<string>()
            {
                Constants.ProjectType.Api
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


    public async Task Create(string name, string plantUmlSourcePath, string directory)
    {

    }

    public async Task<SolutionModel> CreateFromPlantUml(string plantUml, string name, string directory)
    {
        var model = _plantUmlParserStrategyFactory.CreateFor(plantUml, new
        {
            SolutionName = name,
            SolutionRootDirectory = directory
        });

        await _artifactGenerator.GenerateAsync(model);

        return model;
    }

    public async Task MessagingBuildingBlockAdd(string directory)
    {
        var solutionPath = _fileProvider.Get("*.sln", directory);

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


        var messagingProjectModel = await _projectFactory.CreateMessagingProject(messagingDirectory);

        await _artifactGenerator.GenerateAsync(messagingProjectModel);

        _commandService.Start($"dotnet sln {solutionName} add {messagingProjectModel.Path}", solutionDirectory);

        var messagingUdpProjectModel = await _projectFactory.CreateMessagingUdpProject(messagingDirectory);

        await _artifactGenerator.GenerateAsync(messagingUdpProjectModel);

        _commandService.Start($"dotnet sln {solutionName} add {messagingUdpProjectModel.Path}", solutionDirectory);


    }

    public async Task IOCompressionBuildingBlockAdd(string directory)
    {
        var solutionPath = _fileProvider.Get("*.sln", directory);

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


        var messagingProjectModel = await _projectFactory.CreateMessagingProject(messagingDirectory);

        await _artifactGenerator.GenerateAsync(messagingProjectModel);

        _commandService.Start($"dotnet sln {solutionName} add {messagingProjectModel.Path}", solutionDirectory);

        var messagingUdpProjectModel = await _projectFactory.CreateMessagingUdpProject(messagingDirectory);

        await _artifactGenerator.GenerateAsync(messagingUdpProjectModel);

        _commandService.Start($"dotnet sln {solutionName} add {messagingUdpProjectModel.Path}", solutionDirectory);
    }
}
