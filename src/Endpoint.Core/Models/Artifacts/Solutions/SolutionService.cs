// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Internals;
using Endpoint.Core.Messages;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Folders;
using Endpoint.Core.Models.Artifacts.Projects;
using Endpoint.Core.Models.Artifacts.Projects.Enums;
using Endpoint.Core.Models.Artifacts.Projects.Factories;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Classes.Services;
using Endpoint.Core.Services;
using MediatR;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class SolutionService : ISolutionService
{
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly IPlantUmlParserStrategyFactory _plantUmlParserStrategyFactory;
    private readonly IProjectModelFactory _projectModelFactory;
    private readonly IDomainDrivenDesignFileService _domainDrivenDesignFileService;
    private readonly IDomainDrivenDesignService _domainDrivenDesignService;
    private readonly Observable<INotification> _notificationListener;
    private readonly IFileProvider _fileProvider;
    private readonly ICommandService _commandService;
    public SolutionService(
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        IPlantUmlParserStrategyFactory plantUmlParserStrategyFactory,
        IProjectModelFactory projectModelFactory,
        IDomainDrivenDesignFileService domainDrivenDesignFileService,
        IDomainDrivenDesignService domainDrivenDesignService,
        Observable<INotification> notificationListener,
        IFileProvider fileProvider,
        ICommandService commandService)
    {
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory;
        _plantUmlParserStrategyFactory = plantUmlParserStrategyFactory;
        _projectModelFactory = projectModelFactory;
        _domainDrivenDesignFileService = domainDrivenDesignFileService;
        _domainDrivenDesignService = domainDrivenDesignService;
        _notificationListener = notificationListener;
        _fileProvider = fileProvider;
        _commandService = commandService;
    }

    public void AddSolutionItem(string path)
    {
        throw new NotImplementedException();
    }

    public void Create(SolutionModel model)
    {
        _artifactGenerationStrategyFactory.CreateFor(model);
    }

    public void EventDrivenMicroservicesCreate(string name, string services, string directory)
    {
        var notifications = new List<INotification>();

        var solutionModel = new SolutionModel(name, directory);

        solutionModel.Folders.Add(BuildingBlocksCreate(solutionModel.SolutionDirectory));

        solutionModel.Folders.Add(ServicesCreate(services, solutionModel.SolutionDirectory, notifications));

        _artifactGenerationStrategyFactory.CreateFor(solutionModel);

        foreach (var notification in notifications)
        {
            _notificationListener.Broadcast(notification);
        }
    }

    private FolderModel BuildingBlocksCreate(string directory)
    {
        var model = new FolderModel("BuildingBlocks", directory);

        var messagingFolder = new FolderModel("Messaging", model.Directory);

        model.Projects.Add(_projectModelFactory.CreateKernelProject(model.Directory));

        messagingFolder.Projects.Add(_projectModelFactory.CreateMessagingProject(messagingFolder.Directory));

        messagingFolder.Projects.Add(_projectModelFactory.CreateMessagingUdpProject(messagingFolder.Directory));

        model.SubFolders.Add(messagingFolder);

        return model;
    }

    private FolderModel ServicesCreate(string services, string directory, List<INotification> notifications)
    {
        var model = new FolderModel("Services", directory);

        if (string.IsNullOrEmpty(services))
            return model;


        foreach (var service in services.Split(','))
        {
            var serviceFolder = new FolderModel(service, model.Directory);

            var coreModel = _projectModelFactory.CreateLibrary($"{service}.Core", serviceFolder.Directory, new List<string>()
            {
                Constants.ProjectType.Core
            });

            var serviceBusMessageConsumer = _domainDrivenDesignService.ServiceBusMessageConsumerCreate($"{coreModel.Name}.Messages", coreModel.Directory);

            var fileModel = new ObjectFileModel<ClassModel>(serviceBusMessageConsumer, serviceBusMessageConsumer.UsingDirectives, serviceBusMessageConsumer.Name, coreModel.Directory, "cs");

            coreModel.Files.Add(fileModel);

            notifications.Add(new WorkerFileCreated(fileModel.Name, coreModel.Directory));

            coreModel.References.Add(@"..\..\..\BuildingBlocks\Messaging\Messaging.Udp\Messaging.Udp.csproj");

            serviceFolder.Projects.Add(coreModel);

            var infrastructureModel = _projectModelFactory.CreateLibrary($"{service}.Infrastructure", serviceFolder.Directory, new List<string>()
            {
                Constants.ProjectType.Infrastructure
            });

            infrastructureModel.References.Add(@$"..\{service}.Core\{service}.Core.csproj");

            serviceFolder.Projects.Add(infrastructureModel);

            var apiProjectModel = _projectModelFactory.CreateLibrary($"{service}.Api", serviceFolder.Directory, new List<string>()
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


    public void Create(string name, string plantUmlSourcePath, string directory)
    {

    }

    public SolutionModel CreateFromPlantUml(string plantUml, string name, string directory)
    {
        var model = _plantUmlParserStrategyFactory.CreateFor(plantUml, new
        {
            SolutionName = name,
            SolutionRootDirectory = directory
        });

        _artifactGenerationStrategyFactory.CreateFor(model);

        return model;
    }

    public void MessagingBuildingBlockAdd(string directory)
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


        var messagingProjectModel = _projectModelFactory.CreateMessagingProject(messagingDirectory);

        _artifactGenerationStrategyFactory.CreateFor(messagingProjectModel);

        _commandService.Start($"dotnet sln {solutionName} add {messagingProjectModel.Path}", solutionDirectory);

        var messagingUdpProjectModel = _projectModelFactory.CreateMessagingUdpProject(messagingDirectory);

        _artifactGenerationStrategyFactory.CreateFor(messagingUdpProjectModel);

        _commandService.Start($"dotnet sln {solutionName} add {messagingUdpProjectModel.Path}", solutionDirectory);


    }

    public void IOCompressionBuildingBlockAdd(string directory)
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


        var messagingProjectModel = _projectModelFactory.CreateMessagingProject(messagingDirectory);

        _artifactGenerationStrategyFactory.CreateFor(messagingProjectModel);

        _commandService.Start($"dotnet sln {solutionName} add {messagingProjectModel.Path}", solutionDirectory);

        var messagingUdpProjectModel = _projectModelFactory.CreateMessagingUdpProject(messagingDirectory);

        _artifactGenerationStrategyFactory.CreateFor(messagingUdpProjectModel);

        _commandService.Start($"dotnet sln {solutionName} add {messagingUdpProjectModel.Path}", solutionDirectory);
    }
}
