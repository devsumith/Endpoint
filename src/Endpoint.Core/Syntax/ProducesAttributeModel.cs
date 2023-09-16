// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Net;

namespace Endpoint.Core.Syntax;

public class ProducesAttributeModel
{
    public ProducesAttributeModel(string type, HttpStatusCode httpStatusCode)
    {
        Type = type;
        HttpStatusCode = HttpStatusCode;
    }

    public string StatusCode => HttpStatusCode switch
    {
        HttpStatusCode.OK => string.Empty,
        _ => string.Empty
    };

    public string Type { get; private set; }

    public HttpStatusCode HttpStatusCode { get; private set; }
}
