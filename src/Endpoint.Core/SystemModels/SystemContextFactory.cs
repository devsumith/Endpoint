// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using Endpoint.Core.Artifacts.Projects.Enums;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.SystemModels;

public class SystemContextFactory : ISystemContextFactory
{
    private readonly ILogger<SystemContextFactory> logger;
    private readonly ILoggerFactory loggerFactory;

    public SystemContextFactory(ILogger<SystemContextFactory> logger, ILoggerFactory loggerFactory)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
    }

    public async Task<ISystemContext> DddCreateAsync(string systemName, string microserviceName, string aggregate, string properties, string directory)
    {
        logger.LogInformation("Creating Ddd System Context. {name} {aggregate} {directory}", microserviceName, aggregate, directory);

        var model = new SystemContext(loggerFactory.CreateLogger<SystemContext>());

        model.Name = systemName;

        model.Directory = directory;

        var microservice = new Microservice(microserviceName);

        microservice.Projects.Add(new Project(DotNetProjectType.ClassLib, LibraryType.Core, $"{microservice.Name}.Core", Path.Combine(model.Directory, "src", "Services", microservice.SchemaRootName, microservice.Name)));

        microservice.Projects.Add(new Project(DotNetProjectType.ClassLib, LibraryType.Infrastructure, $"{microservice.Name}.Infrastructure", Path.Combine(model.Directory, "src", "Services", microservice.SchemaRootName, microservice.Name)));

        microservice.Projects.Add(new Project(DotNetProjectType.Web, LibraryType.Api, $"{microservice.Name}.Api", Path.Combine(model.Directory, "src", "Services", microservice.SchemaRootName, microservice.Name)));

        model.Microservices.Add(microservice);

        model.Aggregates.Add(new Aggregate() { Name = microserviceName });

        return model;
    }
}
