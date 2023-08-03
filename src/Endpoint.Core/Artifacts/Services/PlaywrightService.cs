// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Services;

public class PlaywrightService : IPlaywrightService
{
    private readonly ILogger<PlaywrightService> _logger;
    private readonly ICommandService _commandService;

    public PlaywrightService(
        ILogger<PlaywrightService> logger,
        ICommandService commandService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public void Create(string directory)
    {
        _commandService.Start("npm init playwright@latest", directory);
    }

    public void TestCreate(string description, string directory)
    {
        throw new NotImplementedException();
    }
}


