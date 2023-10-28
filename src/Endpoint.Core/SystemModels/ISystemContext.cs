// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.SystemModels;

public interface ISystemContext
{
    List<Aggregate> Aggregates { get; set; }

    List<Command> Commands { get; set; }

    List<Query> Queries { get; set; }

    List<Controller> Controllers { get; set; }

    List<Microservice> Microservices { get; set; }

    List<Solution> Solutions { get; set; }

    List<Entity> Entities { get; set; }

    List<Type> Types { get; set; }

    List<BuildingBlock> BuildingBlocks { get; set; }
}