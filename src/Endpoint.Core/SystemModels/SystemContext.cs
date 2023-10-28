// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.SystemModels;

public class SystemContext : ISystemContext
{
    private readonly ILogger<SystemContext> logger;

    public SystemContext(ILogger<SystemContext> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<Aggregate> Aggregates { get; set; } = new ();

    public List<Command> Commands { get; set; } = new ();

    public List<Query> Queries { get; set; } = new ();

    public List<Controller> Controllers { get; set; } = new ();

    public List<Microservice> Microservices { get; set; } = new ();

    public List<Solution> Solutions { get; set; } = new ();

    public List<Entity> Entities { get; set; } = new ();

    public List<Type> Types { get; set; } = new ();

    public List<BuildingBlock> BuildingBlocks { get; set; } = new ();
}
