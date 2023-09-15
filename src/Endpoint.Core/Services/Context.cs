// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Enpoint.Core.Events;
using Microsoft.Extensions.Logging;
using System.Linq;

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

        T item = Activator.CreateInstance(typeof(T)) as T;

        DomainEvents.Raise(item);

        return item;
    }

    public void Set<T>(T item)
        where T : class
    {
        _logger.LogInformation("Setting context. {typeName}", typeof(T).Name);

        DomainEvents.Register<T>(x =>
        {
            var sourceProperties = item.GetType().GetProperties();
            var destinationProperties = x.GetType().GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.FirstOrDefault(x => x.Name == sourceProperty.Name);
                if (destinationProperty != null && destinationProperty.PropertyType == sourceProperty.PropertyType)
                {
                    destinationProperty.SetValue(x, sourceProperty.GetValue(item));
                }
            }
        });
    }

}



