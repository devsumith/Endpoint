using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Services;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Artifacts.Projects.MicroserviceTemplates;

public class ConsoleMicroserviceProjectModel: ProjectModel {

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

        var classFileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, "Worker", Directory, "cs");

        Files = new List<FileModel>
        {
            classFileModel
        };
    }
}

public class ConsoleMicroserviceArtifactGenerationStrategy : ArtifactGenerationStrategyBase<ConsoleMicroserviceProjectModel>
{
    private readonly IFileSystem _fileSytem;

    public ConsoleMicroserviceArtifactGenerationStrategy(IServiceProvider serviceProvider, IFileSystem fileSystem) 
        :base(serviceProvider)
    { 
        _fileSytem = fileSystem;
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, ConsoleMicroserviceProjectModel model, dynamic configuration = null)
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

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, ConsoleMicroserviceProjectModel model, dynamic configuration = null)
    {
        throw new NotImplementedException();
    }
}