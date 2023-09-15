// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Folders;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Options;
using System.IO;

namespace Endpoint.Core.Artifacts.Solutions.Factories;

public class SolutionFactory : ISolutionFactory
{
    private readonly IProjectFactory _projectFactory;

    public SolutionFactory(IProjectFactory projectFactory)
    {
        _projectFactory = projectFactory;
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

        var project = await _projectFactory.Create(dotNetProjectTypeName, projectName, userDefinedFolder == null ? $"{srcFolder.Directory}" : userDefinedFolder.Directory);

        (userDefinedFolder == null ? srcFolder : userDefinedFolder).Projects.Add(project);

        return model;
    }

    public async Task<SolutionModel> CreateHttpSolution(CreateEndpointSolutionOptions options)
    {
        var solutionModel = new SolutionModel(options.Name, options.Directory);

        solutionModel.Projects.Add(await _projectFactory.CreateHttpProject(options.Name, solutionModel.SrcDirectory));

        return solutionModel;
    }

    public async Task<SolutionModel> Minimal(CreateEndpointSolutionOptions options)
    {
        var model = string.IsNullOrEmpty(options.SolutionDirectory) ? new SolutionModel(options.Name, options.Directory) : new SolutionModel(options.Name, options.Directory, options.SolutionDirectory);

        var minimalApiProject = await _projectFactory.CreateMinimalApiProject(new CreateMinimalApiProjectOptions
        {
            Name = $"{options.Name}.Api",
            ShortIdPropertyName = false,
            NumericIdPropertyDataType = false,
            Resource = options.Resource,
            Properties = options.Properties,
            Port = 5000,
            Directory = model.SrcDirectory,
            DbContextName = options.DbContextName
        });

        var unitTestProject = await _projectFactory.CreateMinimalApiUnitTestsProject(options.Name, model.TestDirectory, options.Resource);

        model.Projects.Add(minimalApiProject);

        model.Projects.Add(unitTestProject);

        model.DependOns.Add(new DependsOnModel(unitTestProject, minimalApiProject));

        return model;
    }

    public async Task<SolutionModel> CleanArchitectureMicroservice(CreateCleanArchitectureMicroserviceOptions options)
    {
        var model = string.IsNullOrEmpty(options.SolutionDirectory) ? new SolutionModel(options.Name, options.Directory) : new SolutionModel(options.Name, options.Directory, options.SolutionDirectory);

        var domain = await _projectFactory.CreateLibrary($"{options.Name}.Domain", model.SrcDirectory);

        domain.Metadata.Add(Constants.ProjectType.Domain);

        var infrastructure = await _projectFactory.CreateLibrary($"{options.Name}.Infrastructure", model.SrcDirectory);

        infrastructure.Metadata.Add(Constants.ProjectType.Infrastructure);

        var application = await _projectFactory.CreateLibrary($"{options.Name}.Application", model.SrcDirectory);

        application.Metadata.Add(Constants.ProjectType.Application);

        var api = await _projectFactory.CreateWebApi($"{options.Name}.Api", model.SrcDirectory);

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
}

