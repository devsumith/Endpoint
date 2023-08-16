// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Artifacts;

public class ArtifactGenerator : IArtifactGenerator
{
    private readonly ILogger<ArtifactGenerator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentStack<Tuple<Type, int, Func<dynamic, Task>>> _generateAsyncFuncs = new ConcurrentStack<Tuple<Type, int, Func<dynamic, Task>>>();

    public ArtifactGenerator(

        ILogger<ArtifactGenerator> logger,
        IServiceProvider serviceProvider
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task GenerateAsync(object model, dynamic context = null)
    {
        var generateAsyncs = _generateAsyncFuncs.Where(x => x.Item1 == model.GetType()).OrderByDescending(x => x.Item2).Select(x => x.Item3);

        var generateAsync = generateAsyncs.FirstOrDefault();

        if (generateAsync == null)
        {
            var inner = typeof(IGenericArtifactGenerationStrategy<>).MakeGenericType(model.GetType());

            var type = typeof(IEnumerable<>).MakeGenericType(inner);

            var strategies = _serviceProvider.GetRequiredService(type) as IEnumerable<IArtifactGenerationStrategy>;

            var orderedStrategies = strategies!.OrderByDescending(x => x.GetPriority());

            foreach (var strategy in orderedStrategies)
            {
                if (await strategy.GenerateAsync(this, model))
                {
                    generateAsync = (object model) => strategy.GenerateAsync(this, model, context);

                    _generateAsyncFuncs.Push(new (model.GetType(), strategy.GetPriority(), generateAsync));

                    break;
                }
            }
        }
        else
        {
            await generateAsync(model);
        }
            
    }
}

