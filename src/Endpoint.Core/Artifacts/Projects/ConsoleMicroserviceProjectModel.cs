// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Projects.Enums;
using Endpoint.Core.Internals;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Services;
using MediatR;
using System.Collections.Generic;
using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Artifacts.Projects;

public class ConsoleMicroserviceProjectModel : ProjectModel
{

    public ConsoleMicroserviceProjectModel(string name)
    {
        Name = name;
        DotNetProjectType = DotNetProjectType.Worker;
        Packages.Add(new PackageModel("Microsoft.Extensions.Hosting", "7.0.0"));
        Order = 0;
    }
}

public class ConsoleMicroserviceTestProjectModel : ProjectModel
{
    public ConsoleMicroserviceTestProjectModel(string name, string directory)
    {
        Name = name;
        Directory = $"{directory}{System.IO.Path.DirectorySeparatorChar}{name}";
        DotNetProjectType = DotNetProjectType.XUnit;
        Packages.Add(new PackageModel("Moq", "4.18.4"));
        Order = 1;
        References = new List<string>
        {
            @$"..\\{name.Replace(".Tests","")}\\{name.Replace(".Tests","")}.csproj"
        };

        var classModel = new ClassModel("Worker");

        var classFileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, "Worker", Directory, ".cs");

        Files = new List<FileModel>
        {
            classFileModel
        };
    }
}

public class ConsoleMicroserviceArtifactGenerationStrategy : ArtifactGenerationStrategyBase<ConsoleMicroserviceProjectModel>
{
    private readonly IFileSystem _fileSytem;
    private readonly Observable<INotification> _notificationListener;
    public ConsoleMicroserviceArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        IFileSystem fileSystem,
        Observable<INotification> notificationListener)
        : base(serviceProvider)
    {
        _fileSytem = fileSystem;
        _notificationListener = notificationListener;
    }

    public override async Task CreateAsync(IArtifactGenerator artifactGenerator, ConsoleMicroserviceProjectModel model, dynamic context = null)
    {
        throw new NotImplementedException();
    }
}

public class ConsoleMicroserviceTestArtifactGenerationStrategy : ArtifactGenerationStrategyBase<ConsoleMicroserviceProjectModel>
{
    private readonly IFileSystem _fileSytem;
    public ConsoleMicroserviceTestArtifactGenerationStrategy(IServiceProvider serviceProvider, IFileSystem fileSystem)
        : base(serviceProvider)
    {
        _fileSytem = fileSystem;
    }

    public override async Task CreateAsync(IArtifactGenerator artifactGenerator, ConsoleMicroserviceProjectModel model, dynamic context = null)
    {
        throw new NotImplementedException();
    }
}
