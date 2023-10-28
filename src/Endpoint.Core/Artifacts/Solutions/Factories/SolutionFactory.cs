// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Xml.Linq;
using Endpoint.Core.Artifacts.Folders;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using Endpoint.Core.SystemModels;

namespace Endpoint.Core.Artifacts.Solutions.Factories;

public class SolutionFactory : ISolutionFactory
{
    private readonly IProjectFactory projectFactory;
    private readonly IContext context;

    public SolutionFactory(IProjectFactory projectFactory, IContext context)
    {
        this.projectFactory = projectFactory ?? throw new ArgumentNullException(nameof(projectFactory));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<SolutionModel> Create(string name)
    {
        var model = new SolutionModel() { Name = name };

        return model;
    }

    public async Task<SolutionModel> Create(string name, string projectName, string dotNetProjectTypeName, string folderName, string directory)
    {
        var model = new SolutionModel(name, directory);

        var srcFolder = new FolderModel("src", model.SolutionDirectory);

        model.Folders.Add(srcFolder);

        FolderModel userDefinedFolder = null;

        if (!string.IsNullOrEmpty(folderName))
        {
            userDefinedFolder = new FolderModel(folderName, srcFolder.Directory);

            srcFolder.SubFolders.Add(userDefinedFolder);
        }

        var project = await projectFactory.Create(dotNetProjectTypeName, projectName, userDefinedFolder == null ? $"{srcFolder.Directory}" : userDefinedFolder.Directory);

        (userDefinedFolder == null ? srcFolder : userDefinedFolder).Projects.Add(project);

        return model;
    }

    public async Task<SolutionModel> CreateHttpSolution(CreateEndpointSolutionOptions options)
    {
        var solutionModel = new SolutionModel(options.Name, options.Directory);

        solutionModel.Projects.Add(await projectFactory.CreateHttpProject(options.Name, solutionModel.SrcDirectory));

        return solutionModel;
    }

    public async Task<SolutionModel> Minimal(CreateEndpointSolutionOptions options)
    {
        var model = string.IsNullOrEmpty(options.SolutionDirectory) ? new SolutionModel(options.Name, options.Directory) : new SolutionModel(options.Name, options.Directory, options.SolutionDirectory);

        var minimalApiProject = await projectFactory.CreateMinimalApiProject(new CreateMinimalApiProjectOptions
        {
            Name = $"{options.Name}.Api",
            ShortIdPropertyName = false,
            NumericIdPropertyDataType = false,
            Resource = options.Resource,
            Properties = options.Properties,
            Port = 5000,
            Directory = model.SrcDirectory,
            DbContextName = options.DbContextName,
        });

        var unitTestProject = await projectFactory.CreateMinimalApiUnitTestsProject(options.Name, model.TestDirectory, options.Resource);

        model.Projects.Add(minimalApiProject);

        model.Projects.Add(unitTestProject);

        model.DependOns.Add(new DependsOnModel(unitTestProject, minimalApiProject));

        return model;
    }

    public async Task<SolutionModel> CleanArchitectureMicroservice(CreateCleanArchitectureMicroserviceOptions options)
    {
        var model = string.IsNullOrEmpty(options.SolutionDirectory) ? new SolutionModel(options.Name, options.Directory) : new SolutionModel(options.Name, options.Directory, options.SolutionDirectory);

        var domain = await projectFactory.CreateLibrary($"{options.Name}.Domain", model.SrcDirectory);

        domain.Metadata.Add(Constants.ProjectType.Domain);

        var infrastructure = await projectFactory.CreateLibrary($"{options.Name}.Infrastructure", model.SrcDirectory);

        infrastructure.Metadata.Add(Constants.ProjectType.Infrastructure);

        var application = await projectFactory.CreateLibrary($"{options.Name}.Application", model.SrcDirectory);

        application.Metadata.Add(Constants.ProjectType.Application);

        var api = await projectFactory.CreateWebApi($"{options.Name}.Api", model.SrcDirectory);

        api.Metadata.Add(Constants.ProjectType.Api);

        model.Projects.Add(api);

        model.Projects.Add(domain);

        model.Projects.Add(infrastructure);

        model.Projects.Add(application);

        model.DependOns.Add(new DependsOnModel(infrastructure, domain));

        model.DependOns.Add(new DependsOnModel(application, domain));

        model.DependOns.Add(new DependsOnModel(api, application));

        model.DependOns.Add(new DependsOnModel(api, infrastructure));

        return model;
    }

    public async Task<SolutionModel> Workspace(ResolveOrCreateWorkspaceOptions options)
    {
        var model = new SolutionModel(nameof(Workspace), options.Directory, $"{options.Directory}{Path.DirectorySeparatorChar}{options.Name}");

        return model;
    }

    public async Task<SolutionModel> Resolve(ResolveOrCreateWorkspaceOptions options)
    {
        var model = new SolutionModel(nameof(Workspace), options.Directory, $"{options.Directory}{Path.DirectorySeparatorChar}{options.Name}");

        return model;
    }

    public async Task<SolutionModel> ResolveCleanArchitectureMicroservice(UpdateCleanArchitectureMicroserviceOptions options)
    {
        return new SolutionModel
        {
        };
    }

    public async Task<SolutionModel> DddCreateAync(string name, string directory)
    {
        var systemContext = context.Get<ISystemContext>();

        var model = new SolutionModel(name, directory);

        var schema = name.Remove("Service");

        var sourceFolder = new FolderModel("src", model.SolutionDirectory);

        var servicesFolder = new FolderModel("Services", sourceFolder);

        var serviceFolder = new FolderModel(schema, servicesFolder);

        var buildingBlocksFolder = new FolderModel("BuildingBlocks", sourceFolder) { Priority = 1 };

        var kernel = await projectFactory.CreateKernelProject(buildingBlocksFolder.Directory);

        var messaging = await projectFactory.CreateMessagingProject(buildingBlocksFolder.Directory);

        var messagingUdp = await projectFactory.CreateMessagingUdpProject(buildingBlocksFolder.Directory);

        var security = await projectFactory.CreateSecurityProject(buildingBlocksFolder.Directory);

        var validation = await projectFactory.CreateValidationProject(buildingBlocksFolder.Directory);

        var compression = await projectFactory.CreateIOCompression(buildingBlocksFolder.Directory);

        buildingBlocksFolder.Projects.Add(messaging);

        buildingBlocksFolder.Projects.Add(messagingUdp);

        buildingBlocksFolder.Projects.Add(security);

        buildingBlocksFolder.Projects.Add(kernel);

        buildingBlocksFolder.Projects.Add(validation);

        buildingBlocksFolder.Projects.Add(compression);

        servicesFolder.Projects.Add(await projectFactory.CreateCommon(servicesFolder.Directory));

        var core = await projectFactory.CreateCore(name, serviceFolder.Directory);

        var infrastructure = await projectFactory.CreateInfrastructure(name, serviceFolder.Directory);

        var api = await projectFactory.CreateApi(name, serviceFolder.Directory);

        sourceFolder.Projects.AddRange(new[] { core, infrastructure, api });

        model.Folders.Add(sourceFolder);

        model.Folders.Add(buildingBlocksFolder);

        model.Folders.Add(servicesFolder);

        model.Folders = model.Folders.OrderByDescending(x => x.Priority).ToList();

        return model;
    }
}
