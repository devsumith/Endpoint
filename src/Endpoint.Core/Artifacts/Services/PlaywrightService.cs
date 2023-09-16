// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Services;

public class PlaywrightService : IPlaywrightService
{
    private readonly ILogger<PlaywrightService> logger;
    private readonly ICommandService commandService;

    public PlaywrightService(
        ILogger<PlaywrightService> logger,
        ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public void Create(string directory)
    {
        commandService.Start("npm init playwright@latest", directory);
    }

    public void TestCreate(string description, string directory)
    {
        throw new NotImplementedException();
    }
}
