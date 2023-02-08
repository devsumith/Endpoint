// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Projects.Enums;
using Endpoint.Core.Models.Artifacts.Projects.Factories;
using Endpoint.Core.Services;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Reflection;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class SolutionService : ISolutionService
{
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly IPlantUmlParserStrategyFactory _plantUmlParserStrategyFactory;
    private readonly IProjectModelFactory _projectModelFactory;
    private readonly IDomainDrivenDesignFileService _domainDrivenDesignFileService;
    public SolutionService(
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        IPlantUmlParserStrategyFactory plantUmlParserStrategyFactory,
        IProjectModelFactory projectModelFactory,
        IDomainDrivenDesignFileService domainDrivenDesignFileService)
    {
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory;
        _plantUmlParserStrategyFactory = plantUmlParserStrategyFactory;
        _projectModelFactory = projectModelFactory;
        _domainDrivenDesignFileService = domainDrivenDesignFileService;
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
        var solutionModel = new SolutionModel(name, directory);

        var buildingBlocksFolder = new FolderModel("BuildingBlocks", solutionModel.SolutionDirectory);

        var servicesFolder = new FolderModel("Services", solutionModel.SolutionDirectory);

        var messagingFolder = new FolderModel("Messaging", buildingBlocksFolder.Directory);

        messagingFolder.Projects.Add(_projectModelFactory.CreateMessagingProject(messagingFolder.Directory));

        messagingFolder.Projects.Add(_projectModelFactory.CreateMessagingUdpProject(messagingFolder.Directory));

        buildingBlocksFolder.SubFolders.Add(messagingFolder);

        solutionModel.Folders.Add(buildingBlocksFolder);

        if(!string.IsNullOrEmpty(services))
        {
            foreach(var service in services.Split(','))
            {
                var serviceFolder = new FolderModel(service, servicesFolder.Directory);

                var coreModel = _projectModelFactory.CreateLibrary($"{service}.Core", serviceFolder.Directory, new List<string>()
                {
                    Constants.ProjectType.Core
                });

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

                servicesFolder.SubFolders.Add(serviceFolder);
            }
        }

        solutionModel.Folders.Add(servicesFolder);

        _artifactGenerationStrategyFactory.CreateFor(solutionModel);

        foreach(var serviceFolder in servicesFolder.SubFolders)
        {
            foreach(var projectModel in serviceFolder.Projects)
            {
                if(projectModel.Name.EndsWith("Core"))
                {
                    _domainDrivenDesignFileService.ServiceBusMessageConsumerCreate(directory: projectModel.Directory);
                }
            }
        }

        
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
}
