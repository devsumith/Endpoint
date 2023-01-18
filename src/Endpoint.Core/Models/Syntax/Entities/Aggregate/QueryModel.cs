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

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

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
