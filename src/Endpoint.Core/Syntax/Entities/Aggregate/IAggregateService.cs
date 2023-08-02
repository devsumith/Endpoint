// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;
using System.Threading.Tasks;

namespace Endpoint.Core.Syntax.Entities.Aggregate;

public interface IAggregateService
{
    Task<ClassModel> Add(string name, string properties, string directory, string microserviceName);
    Task CommandCreate(string routeType, string name, string aggregate, string properties, string directory);
    Task QueryCreate(string routeType, string name, string aggregate, string properties, string directory);

}


