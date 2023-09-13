// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Attributes;
using Endpoint.Core.Syntax.Constructors;
using Endpoint.Core.Syntax.Entities;
using Endpoint.Core.Syntax.Fields;
using Endpoint.Core.Syntax.Interfaces;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Syntax.Methods.Factories;
using Endpoint.Core.Syntax.Params;
using Endpoint.Core.Syntax.Properties;
using Endpoint.Core.Syntax.Types;
using Octokit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Classes.Factories;

public class ClassFactory : IClassFactory
{
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly INamespaceProvider _namespaceProvider;
    private readonly IFileProvider _fileProvider;
    private readonly IMethodFactory _methodFactory;

    public ClassFactory(INamingConventionConverter namingConventionConverter, INamespaceProvider namespaceProvider, IFileProvider fileProvider, IMethodFactory methodFactory)
    {
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
        _methodFactory = methodFactory ?? throw new ArgumentNullException(nameof(methodFactory));
    }

    public async Task<ClassModel> DtoExtensionsCreateAsync(ClassModel aggregate)
    {
        var model = new ClassModel($"{aggregate.Name}Extensions")
        {
            Static = true,
            Methods = new List<MethodModel>()
            {
                await _methodFactory.ToDtoCreateAsync(aggregate),
                await _methodFactory.ToDtosAsyncCreateAsync(aggregate),
            }
        };

        return model;
    }

    public ClassModel CreateController(EntityModel model, string directory)
    {
        var csProjPath = _fileProvider.Get("*.csproj", directory);

        var csProjDirectory = Path.GetDirectoryName(csProjPath);

        var rootNamesapce = _namespaceProvider.Get(csProjDirectory).Split('.')[0];

        var classModel = new ClassModel($"{model.Name}Controller");

        classModel.UsingDirectives.Add(new($"{rootNamesapce}.Core.AggregatesModel.{model.Name}Aggregate.Commands"));

        classModel.UsingDirectives.Add(new($"{rootNamesapce}.Core.AggregatesModel.{model.Name}Aggregate.Queries"));

        classModel.UsingDirectives.Add(new("System.Net"));

        classModel.UsingDirectives.Add(new("System.Threading.Tasks"));

        classModel.UsingDirectives.Add(new("MediatR"));

        classModel.UsingDirectives.Add(new("Microsoft.AspNetCore.Mvc"));

        classModel.UsingDirectives.Add(new("System.Net.Mime"));

        classModel.UsingDirectives.Add(new("Swashbuckle.AspNetCore.Annotations"));

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.ApiController, Name = nameof(AttributeType.ApiController) });

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.ApiVersion, Name = nameof(AttributeType.ApiVersion), Template = "\"1.0\"" });

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.Route, Name = nameof(AttributeType.Route), Template = "\"api/{version:apiVersion}/[controller]\"" });

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.Produces, Name = nameof(AttributeType.Produces), Template = "MediaTypeNames.Application.Json" });

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.Consumes, Name = nameof(AttributeType.Consumes), Template = "MediaTypeNames.Application.Json" });

        classModel.Fields.Add(FieldModel.Mediator);

        classModel.Fields.Add(FieldModel.LoggerOf(classModel.Name));

        classModel.Constructors.Add(new(classModel, classModel.Name)
        {
            Params = new()
            {
                ParamModel.Mediator,
                ParamModel.LoggerOf(classModel.Name)
            }
        });

        foreach (var route in Enum.GetValues<RouteType>())
        {
            if (route == RouteType.Page)
                break;

            classModel.Methods.Add(CreateControllerMethod(classModel, model, route));
        }

        return classModel;
    }

    public ClassModel CreateEmptyController(string name, string directory)
    {
        var csProjPath = _fileProvider.Get("*.csproj", directory);

        var csProjDirectory = Path.GetDirectoryName(csProjPath);

        var rootNamesapce = _namespaceProvider.Get(csProjDirectory).Split('.')[0];

        var classModel = new ClassModel($"{name}Controller");

        classModel.UsingDirectives.Add(new("System.Net"));

        classModel.UsingDirectives.Add(new("System.Threading.Tasks"));

        classModel.UsingDirectives.Add(new("MediatR"));

        classModel.UsingDirectives.Add(new("Microsoft.AspNetCore.Mvc"));

        classModel.UsingDirectives.Add(new("System.Net.Mime"));

        classModel.UsingDirectives.Add(new("Swashbuckle.AspNetCore.Annotations"));

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.ApiController, Name = nameof(AttributeType.ApiController) });

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.ApiVersion, Name = nameof(AttributeType.ApiVersion), Template = "\"1.0\"" });

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.Route, Name = nameof(AttributeType.Route), Template = "\"api/{version:apiVersion}/[controller]\"" });

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.Produces, Name = nameof(AttributeType.Produces), Template = "MediaTypeNames.Application.Json" });

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.Consumes, Name = nameof(AttributeType.Consumes), Template = "MediaTypeNames.Application.Json" });

        classModel.Fields.Add(FieldModel.Mediator);

        classModel.Fields.Add(FieldModel.LoggerOf(classModel.Name));

        classModel.Constructors.Add(new(classModel, classModel.Name)
        {
            Params = new()
            {
                ParamModel.Mediator,
                ParamModel.LoggerOf(classModel.Name)
            }
        });

        return classModel;
    }

    private MethodModel CreateControllerMethod(ClassModel controller, EntityModel model, RouteType routeType)
    {
        MethodModel methodModel = new MethodModel
        {
            ParentType = controller,
            Async = true,
            AccessModifier = AccessModifier.Public
        };

        var cancellationTokenParam = ParamModel.CancellationToken;

        var entityNameCamelCase = _namingConventionConverter.Convert(NamingConvention.CamelCase, model.Name);

        var entityNamePascalCase = _namingConventionConverter.Convert(NamingConvention.PascalCase, model.Name);

        var entityNamePascalCasePlural = _namingConventionConverter.Convert(NamingConvention.PascalCase, model.Name, pluralize: true);

        var entityIdNameCamelCase = $"{entityNameCamelCase}Id";

        var entityIdNamePascalCase = $"{entityNamePascalCase}Id";

        methodModel.Name = routeType switch
        {
            RouteType.Get => "Get",
            RouteType.GetById => "GetById",
            RouteType.Create => "Create",
            RouteType.Update => "Update",
            RouteType.Delete => "Delete",
            _ => ""
        };

        methodModel.ReturnType = routeType switch
        {
            RouteType.Get => TypeModel.CreateTaskOfActionResultOf($"Get{entityNamePascalCasePlural}Response"),
            RouteType.GetById => TypeModel.CreateTaskOfActionResultOf($"Get{entityNamePascalCase}ByIdResponse"),
            RouteType.Page => TypeModel.CreateTaskOfActionResultOf($"Get{entityNamePascalCasePlural}PageResponse"),
            RouteType.Create => TypeModel.CreateTaskOfActionResultOf($"Create{entityNamePascalCase}Response"),
            RouteType.Update => TypeModel.CreateTaskOfActionResultOf($"Update{entityNamePascalCase}Response"),
            RouteType.Delete => TypeModel.CreateTaskOfActionResultOf($"Delete{entityNamePascalCase}Response"),
            _ => null
        };

        switch (routeType)
        {
            case RouteType.GetById:

                methodModel.Attributes.Add(new SwaggerOperationAttributeModel($"Get {entityNamePascalCase} by id", $"Get {entityNamePascalCase} by id"));

                methodModel.Attributes.Add(new AttributeModel()
                {
                    Name = "HttpGet",
                    Template = "\"{" + entityIdNameCamelCase + ":guid}\"",
                    Properties = new Dictionary<string, string>() {
                    { "Name", $"get{entityNamePascalCase}ById" }
                }
                });

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("NotFound", "string"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("InternalServerError"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("BadRequest", "ProblemDetails"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("OK", $"Get{entityNamePascalCase}ByIdResponse"));

                methodModel.Params = new List<ParamModel>
                {
                    new ParamModel { Type = new TypeModel("Guid"), Name = entityIdNameCamelCase, Attribute = new AttributeModel() { Name ="FromRoute" } },
                    cancellationTokenParam
                };

                var methodBodyBuilder = new StringBuilder();

                methodBodyBuilder.AppendLine($"var request = new Get{entityNamePascalCase}ByIdRequest()" + "{" + $"{entityIdNamePascalCase} = {entityIdNameCamelCase}" + "};");

                methodBodyBuilder.AppendLine("");

                methodBodyBuilder.AppendLine("var response = await _mediator.Send(request, cancellationToken);");

                methodBodyBuilder.AppendLine("");

                methodBodyBuilder.AppendLine($"if (response.{entityNamePascalCase} == null)");

                methodBodyBuilder.AppendLine("{");

                methodBodyBuilder.AppendLine($"return new NotFoundObjectResult(request.{entityIdNamePascalCase});".Indent(1));

                methodBodyBuilder.AppendLine("}");

                methodBodyBuilder.AppendLine("");

                methodBodyBuilder.Append("return response;");

                methodModel.Body = new Syntax.Expressions.ExpressionModel(methodBodyBuilder.ToString());

                break;

            case RouteType.Get:

                methodModel.Attributes.Add(new SwaggerOperationAttributeModel($"Get {entityNamePascalCasePlural}", $"Get {entityNamePascalCasePlural}"));

                methodModel.Attributes.Add(new AttributeModel()
                {
                    Name = "HttpGet",
                    Properties = new Dictionary<string, string>() {
                    { "Name", $"get{entityNamePascalCasePlural}" }
                }
                });

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("InternalServerError"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("BadRequest", "ProblemDetails"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("OK", $"Get{entityNamePascalCasePlural}Response"));

                methodModel.Params = new List<ParamModel>
                {
                    cancellationTokenParam
                };

                methodModel.Body = new Syntax.Expressions.ExpressionModel($"return await _mediator.Send(new Get{entityNamePascalCasePlural}Request(), cancellationToken);");

                break;

            case RouteType.Create:

                methodModel.Attributes.Add(new SwaggerOperationAttributeModel($"Create {entityNamePascalCase}", $"Create {entityNamePascalCase}"));

                methodModel.Attributes.Add(new AttributeModel()
                {
                    Name = "HttpPost",
                    Properties = new Dictionary<string, string>() {
                    { "Name", $"create{entityNamePascalCase}" }
                }
                });

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("InternalServerError"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("BadRequest", "ProblemDetails"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("OK", $"Create{entityNamePascalCase}Response"));

                methodModel.Params = new List<ParamModel>
                {
                    new ParamModel { Type = new TypeModel($"Create{entityNamePascalCase}Request "), Name = "request", Attribute = new AttributeModel() { Name ="FromBody" } },
                    cancellationTokenParam
                };

                methodModel.Body = new Syntax.Expressions.ExpressionModel("return await _mediator.Send(request, cancellationToken);");

                break;

            case RouteType.Update:

                methodModel.Attributes.Add(new SwaggerOperationAttributeModel($"Update {entityNamePascalCase}", $"Update {entityNamePascalCase}"));

                methodModel.Attributes.Add(new AttributeModel()
                {
                    Name = "HttpPut",
                    Properties = new Dictionary<string, string>() {
                    { "Name", $"update{entityNamePascalCase}" }
                }
                });

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("InternalServerError"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("BadRequest", "ProblemDetails"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("OK", $"Update{entityNamePascalCase}Response"));

                methodModel.Params = new List<ParamModel>
                {
                    new ParamModel { Type = new TypeModel($"Update{entityNamePascalCase}Request "), Name = "request", Attribute = new AttributeModel() { Name ="FromBody" } },
                    cancellationTokenParam
                };

                methodModel.Body = new Syntax.Expressions.ExpressionModel("return await _mediator.Send(request, cancellationToken);");
                break;

            case RouteType.Delete:
                methodModel.Attributes.Add(new SwaggerOperationAttributeModel($"Delete {entityNamePascalCase}", $"Delete {entityNamePascalCase}"));

                methodModel.Attributes.Add(new AttributeModel()
                {
                    Name = "HttpDelete",
                    Template = "\"{" + entityIdNameCamelCase + ":guid}\"",
                    Properties = new Dictionary<string, string>() {
                    { "Name", $"delete{entityNamePascalCase}" }
                }
                });

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("InternalServerError"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("BadRequest", "ProblemDetails"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("OK", $"Delete{entityNamePascalCase}Response"));

                methodModel.Params = new List<ParamModel>
                {
                    new ParamModel { Type = new TypeModel("Guid"), Name = entityIdNameCamelCase, Attribute = new AttributeModel() { Name ="FromRoute" } },
                    cancellationTokenParam
                };

                methodModel.Body = new Syntax.Expressions.ExpressionModel(new StringBuilder().AppendJoin(Environment.NewLine, new string[]
                {
                    $"var request = new Delete{entityNamePascalCase}Request()" + " {" + $"{entityIdNamePascalCase} = {entityIdNameCamelCase}" + " };",
                    "",
                    "return await _mediator.Send(request, cancellationToken);"
                }).ToString());

                break;

        }

        return methodModel;
    }

    public ClassModel CreateEntity(string name, string properties = null)
    {
        var classModel = new ClassModel(name);

        var hasId = false;

        if (!string.IsNullOrEmpty(properties))
        {
            foreach (var property in properties.Split(','))
            {
                var parts = property.Split(':');
                var propName = parts[0];
                var propType = parts[1];

                if (propName == $"{name}Id")
                    hasId = true;

                classModel.Properties.Add(new PropertyModel(classModel, AccessModifier.Public, new TypeModel(propType), propName, PropertyAccessorModel.GetSet));

            }
        }

        if (!hasId)
        {
            classModel.Properties.Add(new PropertyModel(classModel, AccessModifier.Public, new("Guid"), $"{name}Id", PropertyAccessorModel.GetSet));
        }

        return classModel;
    }

    public ClassModel CreateDbContext(string name, List<EntityModel> entities, string serviceName)
    {
        var dbContext = new DbContextModel(_namingConventionConverter, name, entities, serviceName);

        return dbContext;
    }

    public ClassModel CreateWorker(string name, string directory)
    {
        var usings = new List<UsingDirectiveModel>()
        {
            new UsingDirectiveModel() { Name = "Microsoft.Extensions.Hosting" },
            new UsingDirectiveModel() { Name = "Microsoft.Extensions.Logging" },
            new UsingDirectiveModel() { Name = "System" },
            new UsingDirectiveModel() { Name = "System.Threading" },
            new UsingDirectiveModel() { Name = "System.Threading.Tasks" }
        };

        var model = new ClassModel(name);

        var fields = new List<FieldModel>()
        {
            new FieldModel()
            {
                Name = "_logger",
                Type = TypeModel.LoggerOf(name)
            }
        };

        var constructors = new List<ConstructorModel>()
        {

            new ConstructorModel(model,model.Name)
            {
                Params = new List<ParamModel>
                {
                    new ParamModel()
                    {
                        Type = TypeModel.LoggerOf(name),
                        Name = "logger"
                    }
                }
            }
        };

        model.Fields = fields;

        model.Constructors = constructors;

        model.Implements.Add(new TypeModel() { Name = "BackgroundService" });

        var methodBodyBuilder = new StringBuilder();

        methodBodyBuilder.AppendLine("while (!stoppingToken.IsCancellationRequested)");

        methodBodyBuilder.AppendLine("{");

        methodBodyBuilder.AppendLine("_logger.LogInformation(\"Worker running at: {time}\", DateTimeOffset.Now);".Indent(1));

        methodBodyBuilder.AppendLine("await Task.Delay(1000, stoppingToken);".Indent(1));

        methodBodyBuilder.AppendLine("}");

        var method = new MethodModel()
        {
            Name = "ExecuteAsync",
            Override = true,
            AccessModifier = AccessModifier.Protected,
            Body = new Syntax.Expressions.ExpressionModel(methodBodyBuilder.ToString()),
            Async = true,
            ReturnType = new TypeModel() { Name = "Task" },
            Params = new List<ParamModel>
            {
                new ParamModel()
                {
                    Name = "stoppingToken",
                    Type = new TypeModel() { Name = "CancellationToken" }
                }
            }
        };

        model.Methods.Add(method);

        return model;
    }

    public ClassModel CreateMessageModel()
    {
        var model = new ClassModel($"Message");

        model.Properties.Add(new PropertyModel(
            model,
            AccessModifier.Public,
            new TypeModel("string"),
            "MessageType",
            PropertyAccessorModel.GetSet
            )
        {
            DefaultValue = "nameof(Message)"
        });

        model.Properties.Add(new PropertyModel(
            model,
            AccessModifier.Public,
            new TypeModel("DateTimeOffset"),
            "Created",
            PropertyAccessorModel.GetSet
            )
        {
            DefaultValue = "DateTimeOffset.Now"
        });

        return model;
    }

    public ClassModel CreateHubModel(string name)
    {
        var hubClassModel = new ClassModel($"{name}Hub");

        hubClassModel.Implements.Add(new TypeModel("Hub")
        {
            GenericTypeParameters = new List<TypeModel>() {
                new TypeModel($"I{name}Hub")
            }
        });

        hubClassModel.UsingDirectives.Add(new UsingDirectiveModel() { Name = "Microsoft.AspNetCore.SignalR" });

        return hubClassModel;
    }

    public InterfaceModel CreateHubInterfaceModel(string name)
    {
        var interfaceModel = new InterfaceModel($"I{name}Hub");

        interfaceModel.Methods.Add(new MethodModel()
        {
            ParentType = interfaceModel,
            Interface = true,
            ReturnType = new TypeModel("Task"),
            AccessModifier = AccessModifier.Public,
            Name = "Message",
            Params = new() { new() { Name = "message", Type = new("string") } }
        });

        return interfaceModel;
    }

    public ClassModel CreateMessageProducerWorkerModel(string name, string directory)
    {
        var model = CreateWorker("MessageProducer", directory);

        model.UsingDirectives.Add(new UsingDirectiveModel() { Name = "System.Text.Json" });

        var hubContextType = new TypeModel("IHubContext")
        {
            GenericTypeParameters = new List<TypeModel>()
            {
                new TypeModel($"{name}Hub"),
                new TypeModel($"I{name}Hub")
            }
        };

        model.UsingDirectives.Add(new UsingDirectiveModel() { Name = "Microsoft.AspNetCore.SignalR" });

        model.Fields.Add(new FieldModel()
        {

            Type = hubContextType,
            Name = "_hubContext"
        });

        var methodBodyBuilder = new StringBuilder();

        methodBodyBuilder.AppendLine("while (!stoppingToken.IsCancellationRequested)");

        methodBodyBuilder.AppendLine("{");

        methodBodyBuilder.AppendLine("_logger.LogInformation(\"Worker running at: {time}\", DateTimeOffset.Now);".Indent(1));

        methodBodyBuilder.AppendLine("");

        methodBodyBuilder.Append(new StringBuilder()
            .AppendLine("var message = new Message();")
            .AppendLine("")
            .AppendLine("var json = JsonSerializer.Serialize(message);")
            .AppendLine("")
            .AppendLine("await _hubContext.Clients.All.Message(json);")
            .ToString()
            .Indent(1));

        methodBodyBuilder.AppendLine("");

        methodBodyBuilder.AppendLine("await Task.Delay(1000, stoppingToken);".Indent(1));

        methodBodyBuilder.AppendLine("}");

        model.Methods.First().Body = new Syntax.Expressions.ExpressionModel(methodBodyBuilder.ToString());

        model.Constructors.First().Params.Add(new ParamModel()
        {
            Type = hubContextType,
            Name = "hubContext"
        });

        return model;
    }

    public Tuple<ClassModel, InterfaceModel> CreateClassAndInterface(string name)
    {
        var interfaceModel = new InterfaceModel($"I{name}");

        var classModel = new ClassModel(name);

        classModel.Implements.Add(new TypeModel(interfaceModel.Name));

        return new Tuple<ClassModel, InterfaceModel>(classModel, interfaceModel);
    }

    public ClassModel CreateServiceBusMessageConsumer(string name, string messagesNamespace)
    {
        var classModel = new ClassModel(name);

        classModel.Implements.Add(new("BackgroundService"));

        classModel.UsingDirectives.Add(new("Messaging"));

        classModel.UsingDirectives.Add(new("Messaging.Udp"));

        classModel.UsingDirectives.Add(new("Microsoft.Extensions.DependencyInjection"));

        classModel.UsingDirectives.Add(new("Microsoft.Extensions.Hosting"));

        classModel.UsingDirectives.Add(new("System.Text"));

        classModel.UsingDirectives.Add(new("Microsoft.Extensions.Logging"));

        classModel.UsingDirectives.Add(new("System.Threading.Tasks"));

        classModel.UsingDirectives.Add(new("System.Threading"));

        classModel.UsingDirectives.Add(new("MediatR"));

        classModel.UsingDirectives.Add(new("System.Linq"));


        var constructorModel = new ConstructorModel(classModel, classModel.Name);

        foreach (var type in new TypeModel[] { TypeModel.LoggerOf("ServiceBusMessageConsumer"), new TypeModel("IServiceScopeFactory"), new TypeModel("IUdpClientFactory") })
        {
            var propName = type.Name switch
            {
                "ILogger" => "logger",
                "IUdpClientFactory" => "udpClientFactory",
                "IServiceScopeFactory" => "serviceScopeFactory"
            };

            classModel.Fields.Add(new()
            {
                Name = $"_{propName}",
                Type = type
            });

            constructorModel.Params.Add(new()
            {
                Name = propName,
                Type = type
            });
        }

        classModel.Fields.Add(new()
        {
            Name = $"_supportedMessageTypes",
            Type = new("string[]"),
            DefaultValue = "new string[] { }"
        });

        var methodBody = new string[]
        {
            "var client = _udpClientFactory.Create();",

            "",

            "while(!cancellationToken.IsCancellationRequested) {",

            "",

            "var result = await client.ReceiveAsync(cancellationToken);".Indent(1),

            "",

            "var json = Encoding.UTF8.GetString(result.Buffer);".Indent(1),

            "",

            "var message = System.Text.Json.JsonSerializer.Deserialize<ServiceBusMessage>(json)!;".Indent(1),

            "",

            "var messageType = message.MessageAttributes[\"MessageType\"];".Indent(1),

            "",

            "if(_supportedMessageTypes.Contains(messageType))".Indent(1),

            "{".Indent(1),

            new StringBuilder()
            .Append("var type = Type.GetType($\"")
            .Append(messagesNamespace)
            .Append(".{messageType}\");")
            .ToString()
            .Indent(2),

            "",

            "var request = System.Text.Json.JsonSerializer.Deserialize(message.Body, type!)!;".Indent(2),

            "",

            "using (var scope = _serviceScopeFactory.CreateScope())".Indent(2),

            "{".Indent(2),

            "",

            "}".Indent(2),

            "}".Indent(1),

            "",

            "await Task.Delay(0);".Indent(1),

            "}",
        };

        var method = new MethodModel
        {
            Name = "ExecuteAsync",
            Override = true,
            AccessModifier = AccessModifier.Protected,
            Async = true,
            ReturnType = new("Task"),
            Body = new Syntax.Expressions.ExpressionModel(string.Join(Environment.NewLine, methodBody))
        };

        method.Params.Add(ParamModel.CancellationToken);

        classModel.Constructors.Add(constructorModel);

        classModel.Methods.Add(method);

        return classModel;
    }

    public ClassModel CreateConfigureServices(string serviceSuffix)
    {
        var classModel = new ClassModel("ConfigureServices");

        var methodParam = new ParamModel()
        {
            Type = new TypeModel("IServiceCollection"),
            Name = "services",
            ExtensionMethodParam = true
        };

        var method = new MethodModel()
        {
            Name = $"Add{serviceSuffix}Services",
            ReturnType = new TypeModel("void"),
            Static = true,
            Params = new List<ParamModel>() { methodParam }
        };

        classModel.Static = true;

        classModel.Methods.Add(method);

        return classModel;
    }

    public async Task<ClassModel> CreateRequestAsync(string name, string responseName)
    {
        var model = new ClassModel(name);

        model.Implements.Add(new("IRequest")
        {
            GenericTypeParameters = new() { new(responseName) }
        });

        /*        Name = routeType switch
                {
                    RouteType.Create => $"Create{entity.Name}Request",
                    RouteType.Update => $"Update{entity.Name}Request",
                    RouteType.Delete => $"Delete{entity.Name}Request",
                    RouteType.Get => $"Get{entityNamePascalCasePlural}Request",
                    RouteType.GetById => $"Get{entity.Name}ByIdRequest",
                    RouteType.Page => $"Get{entityNamePascalCasePlural}PageRequest",
                    _ => throw new NotSupportedException()
                };

                Implements.Add(new TypeModel("IRequest")
                {
                    GenericTypeParameters = new List<TypeModel> { new TypeModel(response.Name) }
                });

                if (routeType == RouteType.Delete)
                {

                    Properties.Add(entity.Properties.FirstOrDefault(x => x.Name == $"{entity.Name}Id"));
                }

                if (routeType == RouteType.Create)
                {
                    foreach (var prop in entity.Properties.Where(x => x.Name != $"{entity.Name}Id"))
                    {
                        Properties.Add(new PropertyModel(this, AccessModifier.Public, prop.Type, prop.Name, PropertyAccessorModel.GetSet));
                    }
                }

                if (routeType == RouteType.Update)
                {
                    foreach (var prop in entity.Properties)
                    {
                        Properties.Add(new PropertyModel(this, AccessModifier.Public, prop.Type, prop.Name, PropertyAccessorModel.GetSet));
                    }
                }

                if (routeType == RouteType.GetById)
                {
                    Properties.Add(new PropertyModel(this, AccessModifier.Public, new TypeModel("Guid"), $"{entity.Name}Id", PropertyAccessorModel.GetSet));

                }

                if (routeType == RouteType.Page)
                {
                    Properties.Add(new PropertyModel(this, AccessModifier.Public, new TypeModel("int"), "PageSize", PropertyAccessorModel.GetSet));

                    Properties.Add(new PropertyModel(this, AccessModifier.Public, new TypeModel("int"), "Index", PropertyAccessorModel.GetSet));

                    Properties.Add(new PropertyModel(this, AccessModifier.Public, new TypeModel("int"), "Length", PropertyAccessorModel.GetSet));
                */

        return model;
    }

    public async Task<ClassModel> CreateResponseAsync(string name)
    {
        var model = new ClassModel(name);


        return model;
    }

    public async Task<ClassModel> CreateRequestHandlerAsync(string name)
    {
        var model = new ClassModel(name);


        return model;
    }

}

