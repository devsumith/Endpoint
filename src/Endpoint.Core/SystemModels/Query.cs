// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Endpoint.Core.SystemModels;

public class Query
{
    public string Name { get; set; }

    public Request Request { get; set; }

    public Response Response { get; set; }

    public Handler Handler { get; set; }
}
