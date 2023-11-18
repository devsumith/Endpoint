// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.SystemModels;

public class Aggregate
{
    public Aggregate(string name, string properties)
    {
        Properties = new List<Property>();
        Commands = new List<Command>();
        Queries = new List<Query>();
        Entities = new List<Entity>();


        Name = name;
        foreach (var property in properties.Split(','))
        {
            var parts = property.Split(':');
            var propName = parts[0];
            var propType = parts[1];

            Properties.Add(new Property()
            {
                Name = propName,
                Type = new Type() { Name = propType },
            });
        }
    }

    public string Name { get; set; }

    public List<Property> Properties { get; set; }

    public List<Command> Commands { get; set; }

    public List<Query> Queries { get; set; }

    public List<Entity> Entities { get; set; }

    public string MicroserviceName { get; set; }

    public Microservice Microservice { get; set; }

    public string ProjectName { get; set; }

    public Project Project { get; set; }

    public string SolutionName { get; set; }

    public Solution Solution { get; set; }
}
