// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Abstractions;

public class ArtifactGenerator : IArtifactGenerator
{
    private readonly ILogger<ArtifactGenerator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentStack<KeyValuePair<Type, Func<dynamic, Task>>> _generateAsyncFuncs = new ConcurrentStack<KeyValuePair<Type, Func<dynamic, Task>>>();


    public ArtifactGenerator(

        ILogger<ArtifactGenerator> logger,
        IServiceProvider serviceProvider
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task GenerateAsync<T>(T model, dynamic context = null)
    {
        var generateAsync = _generateAsyncFuncs
            .Where(x => x.Key == typeof(T))
            .Select(x => x.Value)
            .FirstOrDefault();

        if (generateAsync == null)
        {
            var strategy = _serviceProvider.GetRequiredService<IArtifactGenerationStrategy<T>>();

            generateAsync = (dynamic model) => strategy.GenerateAsync(this, model);

            _generateAsyncFuncs.Push(new(typeof(T), generateAsync));
        }

        await generateAsync(model);

    }
}

