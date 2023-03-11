// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

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

public class CommandModel : CqrsBase
{
    public CommandModel(string microserviceName, ClassModel entity, INamingConventionConverter namingConventionConverter, RouteType routeType = default, string name = null)
    {
        Name = name ?? routeType switch
        {
            RouteType.Create => $"Create{entity.Name}",
            RouteType.Update => $"Update{entity.Name}",
            RouteType.Delete => $"Delete{entity.Name}",
            _ => throw new NotSupportedException()
        };

        Request = new RequestModel(entity, routeType, namingConventionConverter);

        RequestValidator = new RequestValidatorModel(Request);

        Response = new ClassModel($"{Name}Response")
        {
            Implements = new List<TypeModel>
            {
                new TypeModel("ResponseBase")
            }
        };

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

