// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Attributes;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Constructors;
using Endpoint.Core.Syntax.Fields;
using Endpoint.Core.Syntax.Methods.ControllerMethods;
using Endpoint.Core.Syntax.Params;
using Endpoint.Core.Syntax.Types;

namespace Endpoint.Core.Syntax.Controllers;

public class ControllerModel : ClassModel
{
    public ControllerModel(INamingConventionConverter namingConventionConverter, ClassModel entity)
        : base($"{entity.Name}Controller")
    {
        Attributes.Add(new AttributeModel(AttributeType.ApiController, "ApiController", null));

        Implements.Add(new TypeModel("ControllerBase"));

        Fields.Add(FieldModel.LoggerOf(Name));

        Fields.Add(FieldModel.Mediator);

        var ctor = new ConstructorModel(this, Name);

        ctor.Params.Add(ParamModel.LoggerOf(Name));

        ctor.Params.Add(ParamModel.Mediator);

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
