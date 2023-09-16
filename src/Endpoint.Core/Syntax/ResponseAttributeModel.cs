// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Syntax;

public class ResponseAttributeModel
{
    public int StatusCode { get; set; }

    public string ResponseType { get; set; }

    public string ContentType { get; set; }

    public List<string> Params { get; set; }
}
