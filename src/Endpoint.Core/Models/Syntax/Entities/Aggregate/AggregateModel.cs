// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Services;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

public class AggregateModel
{
    public AggregateModel(INamingConventionConverter namingConventionConverter, string serviceName, ClassModel aggregate, string directory)
    {
        Queries = new List<QueryModel>();
        Commands = new List<CommandModel>();
        Directory = directory;
        MicroserviceName = serviceName;
        Aggregate = aggregate;
        AggregateDto = aggregate.CreateDto();
        AggregateExtensions = new DtoExtensionsModel(namingConventionConverter, $"{aggregate.Name}Extensions", aggregate);

        foreach (var routeType in Enum.GetValues<RouteType>()) { 

            switch(routeType)
            {
                case RouteType.Page:
                    break;

                case RouteType.Get:
                case RouteType.GetById:                
                    Queries.Add(new QueryModel(serviceName, namingConventionConverter,aggregate,routeType));
                    break;

                case RouteType.Delete:
                case RouteType.Create:
                case RouteType.Update:
                    Commands.Add(new CommandModel(serviceName, aggregate, routeType));
                    break;

                default:
                    throw new NotSupportedException();
            }
        }
    }

    public string MicroserviceName { get; set; }
    public ClassModel Aggregate { get; set; }
    public ClassModel AggregateDto { get; set; }
    public DtoExtensionsModel AggregateExtensions { get; set; }
    public List<CommandModel> Commands { get; set; }
    public List<QueryModel> Queries { get; set; }
    public string Directory { get; set; }

}

