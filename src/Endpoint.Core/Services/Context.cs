// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Events;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Services;

public class Context : IContext
{
    private readonly ILogger<Context> _logger;

    public Context(ILogger<Context> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public T Get<T>()
        where T : class
    {
        _logger.LogInformation("Retrieving context. {typeName}", typeof(T).Name);

        var @event = new CustomEvent<T>() {  };

        DomainEvents.Raise(@event);

        return @event.Payload;
    }

    public void Set<T>(T item)
        where T : class
    {
        _logger.LogInformation("Setting context. {typeName}", typeof(T).Name);

        DomainEvents.Register<CustomEvent<T>>(x =>
        {
            x.Payload = item;
        });
    }
}
