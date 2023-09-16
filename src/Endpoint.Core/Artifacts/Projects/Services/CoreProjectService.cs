// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Projects.Services;

public class CoreProjectService : ICoreProjectService
{
    private readonly ILogger<CoreProjectService> logger;

    public CoreProjectService(ILogger<CoreProjectService> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task DoWorkAsync()
    {
        logger.LogInformation("DoWorkAsync");
    }
}
