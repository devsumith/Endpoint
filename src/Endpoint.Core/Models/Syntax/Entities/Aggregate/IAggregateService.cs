// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

public interface IAggregateService
{
    Task Add(string name, string properties, string directory, string microserviceName);
    void CommandCreate(string name, string aggregate, string directory);
    void QueryCreate(string Name, string aggregate, string directory);

}


