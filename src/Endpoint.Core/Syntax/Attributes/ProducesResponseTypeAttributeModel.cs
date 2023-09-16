// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax.Attributes;

public class ProducesResponseTypeAttributeModel : AttributeModel
{
    public ProducesResponseTypeAttributeModel(string httpStatusCodeName, string typeName = null)
    {
        HttpStatusCodeName = httpStatusCodeName;
        TypeName = typeName;
    }

    public string HttpStatusCodeName { get; set; }

    public string TypeName { get; set; }
}
