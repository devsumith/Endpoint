// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Endpoint.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax;

public class SyntaxParser : ISyntaxParser
{
    private readonly ILogger<SyntaxParser> logger;
    private readonly IServiceProvider serviceProvider;
    private readonly IObjectCache cache;

    public SyntaxParser(ILogger<SyntaxParser> logger, IServiceProvider serviceProvider, IObjectCache cache)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<T> ParseAsync<T>(string value)
        where T : SyntaxModel
    {
        logger.LogInformation("Parsing syntax. {typeName}", typeof(T).Name);

        var inner = typeof(ISyntaxParsingStrategy<>).MakeGenericType(typeof(T));

        var type = typeof(IEnumerable<>).MakeGenericType(inner);

        var strategies = cache.FromCacheOrService(() => serviceProvider.GetRequiredService(type) as IEnumerable<ISyntaxParsingStrategy>, typeof(T).FullName);

        var orderedStrategies = strategies!.OrderByDescending(x => x.GetPriority());

        foreach (var strategy in orderedStrategies)
        {
            var result = await strategy.ParseObjectAsync(this, value);

            if (result != null)
            {
                return result as T;
            }
        }

        throw new InvalidOperationException();
    }
}
