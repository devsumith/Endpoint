// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

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

    public async Task<ISystemContext> DddCreateAsync(string name, string aggregate, string directory)
    {
        logger.LogInformation("Creating Ddd System Context. {name} {aggregate} {directory}", name, aggregate, directory);

        var model = new SystemContext(loggerFactory.CreateLogger<SystemContext>());

        model.Aggregates.Add(new Aggregate() { Name = name });

        return model;
    }
}
