// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Endpoint.Core.Models.Syntax.Attributes;

public class SwaggerOperationAttributeModel : AttributeModel
{
    public SwaggerOperationAttributeModel(string summary, string description)
    {
        Summary = summary;
        Description = description;
    }
    public string Summary { get; set; }
    public string Description { get; set; }
}

