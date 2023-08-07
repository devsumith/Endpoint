// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Abstractions;

public class SyntaxGenerator : ISyntaxGenerator
{
    private readonly ILogger<SyntaxGenerator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentStack<KeyValuePair<Type, Func<dynamic, Task<string>>>> _strategies = new ConcurrentStack<KeyValuePair<Type, Func<dynamic, Task<string>>>>();

    public SyntaxGenerator(ILogger<SyntaxGenerator> logger, IServiceProvider serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task<string> GenerateAsync<T>(T model, dynamic context = null)
    {
        var generateAsync = _strategies
            .Where(x => x.Key == typeof(T))
            .Select(x => x.Value)
            .FirstOrDefault();

        if (generateAsync == null)
        {
            var strategy = _serviceProvider.GetRequiredService<ISyntaxGenerationStrategy<T>>();

            generateAsync = (dynamic model) => strategy.GenerateAsync(this, model);

            _strategies.Push(new(typeof(T), generateAsync));
        }

        return await generateAsync(model);

    }
}


