// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Syntax.Documents.Services;

public interface IAggregateService
{
    Task<ClassModel> AddAsync(string name, string properties, string directory, string microserviceName);

    Task CommandCreateAsync(string routeType, string name, string aggregate, string properties, string directory);

    Task QueryCreateAsync(string routeType, string name, string aggregate, string properties, string directory);
}
