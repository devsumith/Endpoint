// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax.ExpressionBodies;

public class ExpressionBodyModel
{
    public ExpressionBodyModel(string body)
    {
        Body = body;
    }

    public string Body { get; set; }
}
