// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.FullStack;

public class FullStackFactory: IFullStackFactory
{
    private readonly ILogger<FullStackFactory> _logger;

    public FullStackFactory(ILogger<FullStackFactory> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
    }

    public Task<FullStackModel> CreateAsync()
    {
        throw new NotImplementedException();
    }
}