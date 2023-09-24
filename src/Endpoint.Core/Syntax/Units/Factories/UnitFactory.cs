// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Units.Factories;

public class UnitFactory : IUnitFactory
{
    private readonly ILogger<UnitFactory> logger;

    public UnitFactory(ILogger<UnitFactory> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SyntaxUnitModel> CreateCommandAsync(ClassModel aggregate, RouteType routeType)
    {
        logger.LogInformation("Create Command Syntax Unit. {aggregateName}", aggregate.Name);

        var model = new SyntaxUnitModel();

        return model;
    }
}
