// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Attributes;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Fields;
using Endpoint.Core.Models.Syntax.Methods.ControllerMethods;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Services;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.Controllers;

public class ControllerModel : ClassModel
{
    public ControllerModel(INamingConventionConverter namingConventionConverter, ClassModel entity)
        : base($"{entity.Name}Controller")
    {

        Attributes.Add(new AttributeModel(AttributeType.ApiController, "ApiController", null));

        Implements.Add(new TypeModel("ControllerBase"));

        Fields.Add(new FieldModel()
        {
            Type = new TypeModel("ILogger")
            {
                GenericTypeParameters = new List<TypeModel>
                {
                    new TypeModel(Name)
                }
            },
            Name = "_logger"
        });

        Fields.Add(new FieldModel()
        {
            Type = new TypeModel($"IMediator"),
            Name = "_mediator"
        });

        var ctor = new ConstructorModel(this, Name);

        ctor.Params.Add(new ParamModel()
        {
            Type = new TypeModel("ILogger")
            {
                GenericTypeParameters = new List<TypeModel>
                {
                    new TypeModel(Name)
                }
            },
            Name = "logger"
        });

        ctor.Params.Add(new ParamModel()
        {
            Type = new TypeModel($"IMediator"),
            Name = "mediator"
        });

        Constructors.Add(ctor);

        foreach (var routeType in Enum.GetValues<RouteType>())
        {
            switch (routeType)
            {
                case RouteType.Page:
                    break;

                case RouteType.Get:
                    Methods.Add(new GetControllerMethodModel(namingConventionConverter, entity));
                    break;

                case RouteType.GetById:
                    Methods.Add(new GetByIdControllerMethodModel(namingConventionConverter, entity));
                    break;

                case RouteType.Create:
                    Methods.Add(new CreateControllerMethodModel(namingConventionConverter, entity));
                    break;

                case RouteType.Update:
                    Methods.Add(new UpdateControllerMethodModel(namingConventionConverter, entity));
                    break;

                case RouteType.Delete:
                    Methods.Add(new DeleteControllerMethodModel(namingConventionConverter, entity));
                    break;

            }
        }
    }
}

