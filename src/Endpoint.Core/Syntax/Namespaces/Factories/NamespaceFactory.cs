// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Namespaces.Factories;

public class NamespaceFactory : INamespaceFactory
{
    private readonly ILogger<NamespaceFactory> logger;

    public NamespaceFactory(ILogger<NamespaceFactory> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task DoWorkAsync()
    {
        logger.LogInformation("DoWorkAsync");
    }
}
