using Endpoint.Core.Abstractions;
using Endpoint.Core.Internals;
using Endpoint.Core.Messages;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Fields;
using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Services;

public class DomainDrivenDesignFileService: IDomainDrivenDesignFileService {

    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly IFileProvider _fileProvider;
    private readonly Observable<INotification> _notificationListener;
    public DomainDrivenDesignFileService(
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, 
        IFileProvider fileProvider,
        Observable<INotification> notificationListener)
	{
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _notificationListener = notificationListener ?? throw new ArgumentNullException(nameof(notificationListener));
	}

	public void ServiceCreate(string name, string directory)
	{
        var usingDirectives = new List<UsingDirectiveModel>()
        {
            new UsingDirectiveModel() { Name = "Microsoft.Extensions.Logging" },
            new UsingDirectiveModel() { Name = "System" },
            new UsingDirectiveModel() { Name = "System.Threading.Tasks" }
        };

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

        var methods = new List<MethodModel>()
        {
            new MethodModel()
            {
                Name = "DoWorkAsync",
                ReturnType = new TypeModel("Task"),
                Async = true,
                Body = "_logger.LogInformation(\"DoWorkAsync\");"
            }
        };

        var @interface = createInterface(name, methods, usingDirectives, directory);

        _ = createClass(@interface,name,methods,usingDirectives, directory);

        InterfaceModel createInterface(string name, List<MethodModel> methods, List<UsingDirectiveModel> usings, string directory)
        {
            var @interface = new InterfaceModel($"I{name}");

            @interface.Methods = methods.Select(x => new MethodModel() { Name = x.Name, ReturnType = x.ReturnType, Async = x.Async, Interface = true }).ToList();

            @interface.UsingDirectives.AddRange(usings);

            var interfaceFile = new ObjectFileModel<InterfaceModel>(
                @interface,
                @interface.UsingDirectives,
                @interface.Name,
                directory,
                "cs"
                );

            _artifactGenerationStrategyFactory.CreateFor(interfaceFile);

            return @interface;
        }

        ClassModel createClass(InterfaceModel @interface, string name, List<MethodModel> methods, List<UsingDirectiveModel> usings, string directory)
        {
            var @class = new ClassModel(name);

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
            @class.Constructors = constructors;

            @class.Methods = methods;

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

            _notificationListener.Broadcast(new ServiceFileCreated(@interface.Name, @class.Name,directory));
            
            return @class;
        }
    }
}
