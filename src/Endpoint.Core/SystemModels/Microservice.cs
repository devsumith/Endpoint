// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.SystemModels;

/// <summary>
/// Microservice.
/// </summary>
public class Microservice
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Microservice"/> class.
    /// puliic constructor.
    /// </summary>
    /// <param name="name">Expecting name that ends in Service. Example OrderService.</param>
    public Microservice(string name)
    {
        Name = name;
        SchemaRootName = name.Replace("Service", string.Empty);
    }

    public string Name { get; set; }

    public string SchemaRootName { get; set; }

    public string Directory { get; set; }

    public List<Project> Projects { get; set; } = new ();

    public List<Aggregate> Aggregates { get; set; } = new ();
}
