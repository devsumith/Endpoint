// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.WebArtifacts;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.Abstractions;

public abstract class WebGenerationStrategyBase<T> : IWebGenerationStrategy
    where T : class
{
    private readonly IServiceProvider _serviceProvider;

    public WebGenerationStrategyBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public virtual bool CanHandle(LitWorkspaceModel model, dynamic context = null) => model is T;

    public void Create(LitWorkspaceModel model, dynamic context = null)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var webGenerator = scope.ServiceProvider
                .GetRequiredService<IWebGenerator>();
            Create(webGenerator, model as T, context);
        }
    }

    public abstract void Create(IWebGenerator webGenerator, T model, dynamic context = null);
    public virtual int Priority => 0;
}

