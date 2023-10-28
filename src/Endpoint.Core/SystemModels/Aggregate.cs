// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.SystemModels;

public class Aggregate
{
    public Aggregate()
    {
        Properties = new List<Property>();
        Commands = new List<Command>();
        Queries = new List<Query>();
        Entities = new List<Entity>();
    }

    public string Name { get; set; }

    public List<Property> Properties { get; set; }

    public List<Command> Commands { get; set; }

    public List<Query> Queries { get; set; }

    public List<Entity> Entities { get; set; }
}
