using Endpoint.Core.Abstractions;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Fields;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Services;
using System.Collections.Generic;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

public class AggregateModel
{
    public AggregateModel(INamingConventionConverter namingConventionConverter, string microserviceName, ClassModel aggregate, string directory)
    {
        Queries = new List<QueryModel>();
        Commands = new List<CommandModel>();
        Directory = directory;
        MicroserviceName = microserviceName;
        Aggregate = aggregate;
        AggregateDto = aggregate.CreateDto();
        AggregateExtensions = new DtoExtensionsModel(namingConventionConverter, $"{aggregate.Name}Extensions", aggregate);

        foreach (var routeType in Enum.GetValues<RouteType>()) { 

            switch(routeType)
            {
                case RouteType.Get:
                case RouteType.GetById:
                case RouteType.Page:
                    Queries.Add(new QueryModel(microserviceName, namingConventionConverter,aggregate,routeType));
                    break;

                case RouteType.Delete:
                case RouteType.Create:
                case RouteType.Update:
                    Commands.Add(new CommandModel(microserviceName, namingConventionConverter,aggregate,routeType));
                    break;

                default:
                    throw new NotSupportedException();
            }
        }
    }

    public string MicroserviceName { get; set; }
    public ClassModel Aggregate { get; set; }
    public ClassModel AggregateDto { get; set; }
    public DtoExtensionsModel AggregateExtensions { get; set; }
    public List<CommandModel> Commands { get; set; }
    public List<QueryModel> Queries { get; set; }
    public string Directory { get; set; }

}


public class QueryModel : CqrsBase
{
    public QueryModel(string microserviceName, INamingConventionConverter namingConventionConverter, ClassModel entity, RouteType routeType)
    {
        var entityNamePascalCasePlural = namingConventionConverter.Convert(NamingConvention.PascalCase, entity.Name, pluralize: true);

        Name = routeType switch
        {
            RouteType.Get => $"Get{entityNamePascalCasePlural}",
            RouteType.GetById => $"Get{entity.Name}ById",
            RouteType.Page => $"Get{entityNamePascalCasePlural}Page",
            _ => throw new NotSupportedException()
        };

        Request = new ClassModel($"{Name}Request");

        Response = new ClassModel($"{Name}Response");

        Request.Implements.Add(new TypeModel("IRequest")
        {
            GenericTypeParameters = new List<TypeModel> { new TypeModel(Response.Name) }
        });

        RequestHandler = new ClassModel($"{Request.Name}Handler");

        RequestHandler.Implements.Add(new TypeModel("IRequestHandler")
        {
            GenericTypeParameters = new List<TypeModel> { new TypeModel(Request.Name), new TypeModel(Response.Name) }
        });

        RequestHandler.Fields.Add(new FieldModel()
        {
            Type = new TypeModel("ILogger")
            {
                GenericTypeParameters = new List<TypeModel>
                {
                    new TypeModel(RequestHandler.Name)
                }
            },
            Name = "_logger"
        });

        RequestHandler.Fields.Add(new FieldModel()
        {
            Type = new TypeModel($"I{microserviceName}DbContext"),
            Name = "_context"
        });

        var requestHandlerCtor = new ConstructorModel(RequestHandler, RequestHandler.Name);

        requestHandlerCtor.Params.Add(new ParamModel() { 
            Type = new TypeModel("ILogger")
            {
                GenericTypeParameters = new List<TypeModel>
                {
                    new TypeModel(RequestHandler.Name)
                }
            },
            Name = "logger"
        });

        requestHandlerCtor.Params.Add(new ParamModel() {
            Type = new TypeModel($"I{microserviceName}DbContext"),
            Name = "context"
        });
        
        RequestHandler.Constructors.Add(requestHandlerCtor);

        if (routeType == RouteType.Get)
        {
            Response.Properties.Add(new PropertyModel(Response, AccessModifier.Public, new TypeModel("List")
            {
                GenericTypeParameters = new List<TypeModel>()
                {
                    new TypeModel($"{entity.Name}Dto")
                }
            }, namingConventionConverter.Convert(NamingConvention.PascalCase, entity.Name, pluralize: true) , PropertyAccessorModel.GetSet));
        }

        var methodModel = new MethodModel()
        {
            AccessModifier = AccessModifier.Public,
            ParentType = RequestHandler,
            Name = "Handle",
            Async = true,
            ReturnType = new TypeModel("Task")
            {
                GenericTypeParameters = new List<TypeModel>()
                {
                    new TypeModel(Response.Name)
                }
            },
            Params = new List<ParamModel>()
            {
                new ParamModel() {
                    Type = new TypeModel(Request.Name),
                    Name = "request"
                },
                new ParamModel() {
                    Type = new TypeModel("CancellationToken"),
                    Name = "cancellationToken"
                }
            }
        };

        RequestHandler.Methods.Add(methodModel);

        if (routeType == RouteType.GetById)
        {
            Request.Properties.Add(new PropertyModel(Request, AccessModifier.Public, new TypeModel("Guid"), $"{entity.Name}Id", PropertyAccessorModel.GetSet));

            Response.Properties.Add(new PropertyModel(Response, AccessModifier.Public, new TypeModel($"{entity.Name}Dto"), $"{entity.Name}", PropertyAccessorModel.GetSet));
        }


        if (routeType == RouteType.Page)
        {
            Request.Properties.Add(new PropertyModel(Request, AccessModifier.Public, new TypeModel("int"), "PageSize", PropertyAccessorModel.GetSet));

            Request.Properties.Add(new PropertyModel(Request, AccessModifier.Public, new TypeModel("int"), "Index", PropertyAccessorModel.GetSet));

            Response.Properties.Add(new PropertyModel(Response, AccessModifier.Public, new TypeModel("int"), "Length", PropertyAccessorModel.GetSet));

            Response.Properties.Add(new PropertyModel(Response, AccessModifier.Public, new TypeModel("List")
            {
                GenericTypeParameters = new List<TypeModel>()
                {
                    new TypeModel($"{entity.Name}Dto")
                }
            }, "Entities ", PropertyAccessorModel.GetSet));
        }
    }

}


public class CommandModel : CqrsBase
{
    public CommandModel(string microserviceName, INamingConventionConverter namingConventionConverter, ClassModel entity, RouteType routeType)
    {
        Name = routeType switch
        {
            RouteType.Create => $"Create{entity.Name}",
            RouteType.Update => $"Update{entity.Name}",
            RouteType.Delete => $"Delete{entity.Name}",
            _ => throw new NotSupportedException()
        };

        Request = new ClassModel($"{Name}Request");

        RequestValidator = new ClassModel($"{Name}RequestValidator");

        RequestValidator.Implements.Add(new TypeModel("AbstractValidator")
        {
            GenericTypeParameters = new List<TypeModel>()
            {
                new TypeModel(Request.Name) {}
            }
        });

        Response = new ClassModel($"{Name}Response");

        Request.Implements.Add(new TypeModel("IRequest")
        {
            GenericTypeParameters = new List<TypeModel> { new TypeModel(Response.Name) }
        });

        RequestHandler = new ClassModel($"{Request.Name}Handler");

        RequestHandler.Implements.Add(new TypeModel("IRequestHandler")
        {
            GenericTypeParameters = new List<TypeModel> { new TypeModel(Request.Name), new TypeModel(Response.Name) }
        });

        RequestHandler.Fields.Add(new FieldModel()
        {
            Type = new TypeModel("ILogger")
            {
                GenericTypeParameters = new List<TypeModel>
                {
                    new TypeModel(RequestHandler.Name)
                }
            },
            Name = "_logger"
        });

        RequestHandler.Fields.Add(new FieldModel()
        {
            Type = new TypeModel($"I{microserviceName}DbContext"),
            Name = "_context"
        });

        var requestHandlerCtor = new ConstructorModel(RequestHandler, RequestHandler.Name);

        requestHandlerCtor.Params.Add(new ParamModel()
        {
            Type = new TypeModel("ILogger")
            {
                GenericTypeParameters = new List<TypeModel>
                {
                    new TypeModel(RequestHandler.Name)
                }
            },
            Name = "logger"
        });

        requestHandlerCtor.Params.Add(new ParamModel()
        {
            Type = new TypeModel($"I{microserviceName}DbContext"),
            Name = "context"
        });

        RequestHandler.Constructors.Add(requestHandlerCtor);

        var methodModel = new MethodModel()
        {
            AccessModifier = AccessModifier.Public,
            ParentType = RequestHandler,
            Name = "Handle",
            Async = true,
            ReturnType = new TypeModel("Task")
            {
                GenericTypeParameters = new List<TypeModel>()
                {
                    new TypeModel(Response.Name)
                }
            },
            Params = new List<ParamModel>()
            {
                new ParamModel() { 
                    Type = new TypeModel(Request.Name),
                    Name = "request"
                },
                new ParamModel() {
                    Type = new TypeModel("CancellationToken"),
                    Name = "cancellationToken"
                }
            }
        };

        RequestHandler.Methods.Add(methodModel);

        if (routeType == RouteType.Create)
        {
            foreach (var prop in entity.Properties)
            {
                Request.Properties.Add(new PropertyModel(Request, AccessModifier.Public, prop.Type, prop.Name, PropertyAccessorModel.GetSet));
            }

            Response.Properties.Add(new PropertyModel(Response, AccessModifier.Public, new TypeModel($"{entity.Name}Dto"), $"{entity.Name}", PropertyAccessorModel.GetSet));
        }

        if (routeType == RouteType.Update)
        {
            foreach (var prop in entity.Properties)
            {
                Request.Properties.Add(new PropertyModel(Request, AccessModifier.Public, prop.Type, prop.Name, PropertyAccessorModel.GetSet));
            }

            Response.Properties.Add(new PropertyModel(Response, AccessModifier.Public, new TypeModel($"{entity.Name}Dto"), $"{entity.Name}", PropertyAccessorModel.GetSet));
        }

        if (routeType == RouteType.Delete)
        {
            Request.Properties.Add(new PropertyModel(Request, AccessModifier.Public, new TypeModel("Guid"), $"{entity.Name}Id", PropertyAccessorModel.GetSet));

            Response.Properties.Add(new PropertyModel(Response, AccessModifier.Public, new TypeModel($"{entity.Name}Dto"), $"{entity.Name}", PropertyAccessorModel.GetSet));
        }
    }
    public ClassModel RequestValidator { get; set; }
}

public class CqrsBase
{
    public CqrsBase()
    {
        UsingDirectives = new List<UsingDirectiveModel>();
    }
    public string Name { get; set; }
    public ClassModel Request { get; set; }
    public ClassModel Response { get; set; }
    public ClassModel RequestHandler { get; set; }
    public List<UsingDirectiveModel> UsingDirectives { get; set; }
}


public class QueryModelSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<QueryModel>
{
    public QueryModelSyntaxGenerationStrategy(IServiceProvider serviceProvider) 
        :base(serviceProvider) { }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, QueryModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.Request));

        builder.AppendLine("");

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.Response));

        builder.AppendLine("");

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.RequestHandler, context));

        return builder.ToString();
    }
}

public class CommandModelSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<CommandModel>
{
    public CommandModelSyntaxGenerationStrategy(IServiceProvider serviceProvider) 
        :base(serviceProvider) { }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, CommandModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.RequestValidator));

        builder.AppendLine("");

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.Request));

        builder.AppendLine("");

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.Response));

        builder.AppendLine("");

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.RequestHandler, context));

        return builder.ToString();
    }
}