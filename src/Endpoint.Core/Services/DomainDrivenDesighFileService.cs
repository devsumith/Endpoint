using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Fields;
using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Types;
using System.Collections.Generic;

namespace Endpoint.Core.Services;

public class DomainDrivenDesighFileService: IDomainDrivenDesignFileService {

    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly IFileProvider _fileProvider;

    public DomainDrivenDesighFileService(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, IFileProvider fileProvider)
	{
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
	}

	public void ServiceCreate(string name, string directory)
	{
        var usingDirectives = new List<UsingDirectiveModel>()
        {
            new UsingDirectiveModel() { Name = "Microsoft.Extensions.Logging" },
            new UsingDirectiveModel() { Name = "System" },
            new UsingDirectiveModel() { Name = "System.Threading.Tasks" }
        };

        var @class = new ClassModel(name);

        var fields = new List<FieldModel>()
        {
            new FieldModel()
            {
                Name = "_logger",
                Type = new TypeModel()
                {
                    Name = "ILogger",
                    GenericTypeParameters = new List<TypeModel>()
                    {
                        new TypeModel() { Name = name}
                    }
                }
            }
        };

        var constructors = new List<ConstructorModel>()
        {

            new ConstructorModel(@class,@class.Name)
            {
                Params = new List<ParamModel>
                {
                    new ParamModel()
                    {
                        Type = new TypeModel()
                        {
                            Name = "ILogger",
                            GenericTypeParameters = new List<TypeModel>()
                            {
                                new TypeModel() { Name = name}
                            }
                        },
                        Name = "logger"
                    }
                }
            }
        };

        var classMethods = new List<MethodModel>()
        {
            new MethodModel()
            {
                Name = "DoWorkAsync",
                ReturnType = new TypeModel() { Name = "Task" },
                Async = true
            }
        };

        var interfaceMethods = new List<MethodModel>()
        {
            new MethodModel()
            {
                Name = "DoWorkAsync",
                ReturnType = new TypeModel() { Name = "Task" },
                Async = true,
                Interface = true
            }
        };

        var @interface = new InterfaceModel($"I{name}");

        @interface.Methods = interfaceMethods;

        @interface.UsingDirectives.AddRange(usingDirectives);

        var interfaceFile = new ObjectFileModel<InterfaceModel>(
            @interface,
            @interface.UsingDirectives,
            @interface.Name,
            directory,
            "cs"
            );

        _artifactGenerationStrategyFactory.CreateFor(interfaceFile);

        @class.Constructors = constructors;

        @class.Methods = classMethods;

        @class.Fields = fields;

        @class.UsingDirectives.AddRange(usingDirectives);

        @class.Implements.Add(new TypeModel() { Name = @interface.Name });

        var classFile = new ObjectFileModel<ClassModel>(
            @class,
            @class.UsingDirectives,
            @class.Name,
            directory,
            "cs"
            );

        _artifactGenerationStrategyFactory.CreateFor(classFile);

    }
}
