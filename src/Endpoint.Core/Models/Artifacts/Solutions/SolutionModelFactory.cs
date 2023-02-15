// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Artifacts.Projects.Factories;
using Endpoint.Core.Options;
using System.IO;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class SolutionModelFactory: ISolutionModelFactory
{
    private readonly IProjectModelFactory _projectModelFactory;

    public SolutionModelFactory(IProjectModelFactory projectModelFactory)
    {
        _projectModelFactory = projectModelFactory;
    }
    public SolutionModel Create(string name)
    {
        var model = new SolutionModel() { Name = name };

        return model;
    }

    public SolutionModel Create(string name, string projectName, string dotNetProjectTypeName, string folderName, string directory)
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
        
        var project = _projectModelFactory.Create(dotNetProjectTypeName, projectName, userDefinedFolder == null ? $"{srcFolder.Directory}" : userDefinedFolder.Directory);

        (userDefinedFolder == null ? srcFolder : userDefinedFolder).Projects.Add(project);

        return model;
    }

    public SolutionModel CreateHttpSolution(CreateEndpointSolutionOptions options)
    {
        var solutionModel = new SolutionModel(options.Name, options.Directory);

        solutionModel.Projects.Add(_projectModelFactory.CreateHttpProject(options.Name, solutionModel.SrcDirectory));

        return solutionModel;
    }

    public SolutionModel Minimal(CreateEndpointSolutionOptions options)
    {
        var model = string.IsNullOrEmpty(options.SolutionDirectory) ? new SolutionModel(options.Name, options.Directory) : new SolutionModel(options.Name, options.Directory, options.SolutionDirectory);

        var minimalApiProject = _projectModelFactory.CreateMinimalApiProject(new CreateMinimalApiProjectOptions
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

        var unitTestProject = _projectModelFactory.CreateMinimalApiUnitTestsProject(options.Name, model.TestDirectory, options.Resource);

        model.Projects.Add(minimalApiProject);

        model.Projects.Add(unitTestProject);

        model.DependOns.Add(new DependsOnModel(unitTestProject, minimalApiProject));

        return model;
    }

    public SolutionModel CleanArchitectureMicroservice(CreateCleanArchitectureMicroserviceOptions options)
    {
        var model = string.IsNullOrEmpty(options.SolutionDirectory) ? new SolutionModel(options.Name, options.Directory) : new SolutionModel(options.Name, options.Directory, options.SolutionDirectory);

        var domain = _projectModelFactory.CreateLibrary($"{options.Name}.Domain", model.SrcDirectory);

        domain.Metadata.Add(Constants.ProjectType.Domain);

        var infrastructure = _projectModelFactory.CreateLibrary($"{options.Name}.Infrastructure", model.SrcDirectory);

        infrastructure.Metadata.Add(Constants.ProjectType.Infrastructure);

        var application = _projectModelFactory.CreateLibrary($"{options.Name}.Application", model.SrcDirectory);

        application.Metadata.Add(Constants.ProjectType.Application);

        var api = _projectModelFactory.CreateWebApi($"{options.Name}.Api", model.SrcDirectory);

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

    public SolutionModel Workspace(ResolveOrCreateWorkspaceOptions options)
    {
        var model = new SolutionModel(nameof(Workspace), options.Directory, $"{options.Directory}{Path.DirectorySeparatorChar}{options.Name}");

        return model;
    }

    public SolutionModel Resolve(ResolveOrCreateWorkspaceOptions options)
    {
        var model = new SolutionModel(nameof(Workspace), options.Directory, $"{options.Directory}{Path.DirectorySeparatorChar}{options.Name}");

        return model;
    }

    public SolutionModel ResolveCleanArchitectureMicroservice(UpdateCleanArchitectureMicroserviceOptions options)
    {
        return new SolutionModel
        {

        };
    }
}

