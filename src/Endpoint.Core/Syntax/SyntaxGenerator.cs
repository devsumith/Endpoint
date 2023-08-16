// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Endpoint.Core.Syntax;

public class SyntaxGenerator : ISyntaxGenerator
{
    private readonly ILogger<SyntaxGenerator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentStack<Tuple<Type, int, Func<ISyntaxGenerator, object, dynamic, Task<string>>>> _generateAsyncFuncs = new ConcurrentStack<Tuple<Type, int, Func<ISyntaxGenerator, object, dynamic, Task<string>>>>();

    public SyntaxGenerator(ILogger<SyntaxGenerator> logger, IServiceProvider serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task<string> GenerateAsync<T>(T model, dynamic context = null)
    {        
        if (!_generateAsyncFuncs.Where(x => x.Item1 == typeof(T)).Any())
        {
            var inner = typeof(IGenericSyntaxGenerationStrategy<>).MakeGenericType(model.GetType());

            var type = typeof(IEnumerable<>).MakeGenericType(inner);

            var strategies = _serviceProvider.GetRequiredService(type) as IEnumerable<ISyntaxGenerationStrategy>;

            var orderedStrategies = strategies!.OrderByDescending(x => x.GetPriority());

            foreach (var strategy in orderedStrategies)
            {
                _generateAsyncFuncs.Push(new(model.GetType(), strategy.GetPriority(), strategy.GenerateAsync));
            }
        }

        var result = string.Empty;

        foreach (var strategy in _generateAsyncFuncs
            .Where(x => x.Item1 == typeof(T))
            .OrderByDescending(x => x.Item2)
            .Select(x => x.Item3))
        {
            result = await strategy(this, model, context);

            if (result != null)
            {                
                return result;
            }
        }

        throw new Exception();
    }
}


