// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax;

public class SyntaxFactory : ISyntaxFactory
{
    private readonly ILogger<SyntaxFactory> logger;

    public SyntaxFactory(ILogger<SyntaxFactory> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task DoWorkAsync()
    {
        logger.LogInformation("DoWorkAsync");
    }
}
