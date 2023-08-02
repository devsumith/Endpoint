// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Properties;

namespace Endpoint.Core.Syntax;

public class RuleForModel
{
    public RuleForModel(PropertyModel property)
    {
        Property = property;
    }
    public PropertyModel Property { get; set; }
}

