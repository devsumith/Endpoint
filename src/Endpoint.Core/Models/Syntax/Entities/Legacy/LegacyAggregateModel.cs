// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Properties;
using System.Collections.Generic;
using System.Linq;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Types;

namespace Endpoint.Core.Models.Syntax.Entities.Legacy;

[Obsolete]
public class LegacyAggregatesModel : EntityModel
{
    public List<EntityModel> Entities { get; private set; }

    public string IdPropertyName { get; set; }
    public string IdPropertyType { get; set; }

    public LegacyAggregatesModel(string name, List<PropertyModel> classProperties)
        : base(name)
    {
        Name = name;
        Properties = classProperties;
        Entities = new List<EntityModel>();
    }

    public LegacyAggregatesModel(string name, bool useIntIdPropertyType, bool useShortIdProperty, string properties)
        : base(name)
    {
        Name = name;

        IdPropertyType = useIntIdPropertyType ? "int" : "Guid";

        IdPropertyName = useShortIdProperty ? "Id" : $"{((SyntaxToken)name).PascalCase}Id";

        Properties.Add(new PropertyModel(this, AccessModifier.Public, new TypeModel() { Name = IdPropertyType }, IdPropertyName, PropertyAccessorModel.GetPrivateSet, key: true));

        if (!string.IsNullOrWhiteSpace(properties))
        {
            foreach (var property in properties.Split(','))
            {
                var nameValuePair = property.Split(':');

                Properties.Add(new PropertyModel(this, AccessModifier.Public, new TypeModel() { Name = nameValuePair.ElementAt(1) }, nameValuePair.ElementAt(0), PropertyAccessorModel.GetPrivateSet));
            }
        }
    }

    public LegacyAggregatesModel(string name)
        : base(name)
    {
        Name = name;
    }
}

