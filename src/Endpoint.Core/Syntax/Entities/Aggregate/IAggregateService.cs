// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;
using System.Threading.Tasks;

namespace Endpoint.Core.Syntax.Entities.Aggregate;

public interface IAggregateService
{
    Task<ClassModel> Add(string name, string properties, string directory, string microserviceName);
    void CommandCreate(string routeType, string name, string aggregate, string properties, string directory);
    void QueryCreate(string routeType, string name, string aggregate, string properties, string directory);

}


