using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Models.Syntax.Types;
using System.Collections.Generic;

namespace Endpoint.Core.Services;

public class DomainDrivenDesighFileService: IDomainDrivenDesignFileService {

    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;

    public DomainDrivenDesighFileService(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory)
	{
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory;
	}

	public void ServiceCreate(string name, string directory)
	{
        var @class = new ClassModel(name);

        var @interface = new InterfaceModel($"I{name}");

        var usingDirectives = new List<UsingDirectiveModel>()
        {
            new UsingDirectiveModel() { Name = "System" }
        };

        @class.UsingDirectives.AddRange(usingDirectives);

        @class.Implements.Add(new TypeModel() { Name = @interface.Name });

        var classFile = new ObjectFileModel<ClassModel>(
            @class,
            @class.UsingDirectives,
            @class.Name,
            directory,
        "cs"
            );

        @interface.UsingDirectives.AddRange(usingDirectives);

        var interfaceFile = new ObjectFileModel<InterfaceModel>(
            @interface,
            @interface.UsingDirectives,
            @interface.Name,
            directory,
            "cs"
            );

        _artifactGenerationStrategyFactory.CreateFor(classFile);

        _artifactGenerationStrategyFactory.CreateFor(interfaceFile);
    }
}
