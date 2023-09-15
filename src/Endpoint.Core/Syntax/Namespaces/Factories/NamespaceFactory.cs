// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Endpoint.Core.Syntax.Namespaces.Factories;

public class NamespaceFactory : INamespaceFactory
{
    private readonly ILogger<NamespaceFactory> _logger;

    public NamespaceFactory(ILogger<NamespaceFactory> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task DoWorkAsync()
    {
        _logger.LogInformation("DoWorkAsync");
    }

}

