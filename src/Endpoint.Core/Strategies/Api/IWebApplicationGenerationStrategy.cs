// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.RouteHandlers;
using System.Collections.Generic;

namespace Endpoint.Core.Strategies.Api
{
    public interface IWebApplicationGenerationStrategy
    {
        string Create(string @namespace, string dbContextName, List<RouteHandlerModel> routeHandlers);
        string Update(List<string> existingWebApplication, string @namespace, string dbContextName, List<RouteHandlerModel> routeHandlers);
    }
}

