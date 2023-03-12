// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Models.Syntax.Classes;

public class RequestModel: ClassModel {
    public RequestModel(ResponseModel response, ClassModel entity, RouteType routeType, INamingConventionConverter namingConventionConverter)
        :base("")
    {
        var entityNamePascalCasePlural = namingConventionConverter.Convert(NamingConvention.PascalCase, entity.Name, true);

        Name = routeType switch
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

        if (routeType == RouteType.Delete) {

            Properties.Add(entity.Properties.FirstOrDefault(x => x.Name == $"{entity.Name}Id"));
        }

        if (routeType == RouteType.Create)
        {
            foreach (var prop in entity.Properties.Where(x => x.Name != $"{entity.Name}Id"))
            {
                Properties.Add(new PropertyModel(this, Enums.AccessModifier.Public, prop.Type, prop.Name, PropertyAccessorModel.GetSet));
            }
        }

        if (routeType == RouteType.Update)
        {
            foreach (var prop in entity.Properties)
            {
                Properties.Add(new PropertyModel(this, Enums.AccessModifier.Public, prop.Type, prop.Name, PropertyAccessorModel.GetSet));
            }
        }

        if (routeType == RouteType.GetById)
        {
            Properties.Add(new PropertyModel(this, Enums.AccessModifier.Public, new TypeModel("Guid"), $"{entity.Name}Id", PropertyAccessorModel.GetSet));

        }

        if (routeType == RouteType.Page)
        {
            Properties.Add(new PropertyModel(this, Enums.AccessModifier.Public, new TypeModel("int"), "PageSize", PropertyAccessorModel.GetSet));

            Properties.Add(new PropertyModel(this, Enums.AccessModifier.Public, new TypeModel("int"), "Index", PropertyAccessorModel.GetSet));

            Properties.Add(new PropertyModel(this, Enums.AccessModifier.Public, new TypeModel("int"), "Length", PropertyAccessorModel.GetSet));
        }
    }

}

