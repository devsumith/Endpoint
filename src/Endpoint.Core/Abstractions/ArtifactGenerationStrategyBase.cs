// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.Abstractions;

public abstract class ArtifactUpdateStrategyBase<T> : IArtifactUpdateStrategy
    where T : class
{
    protected readonly IServiceProvider _serviceProvider;

    public ArtifactUpdateStrategyBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public virtual bool CanHandle(dynamic context = null, params object[] args) => args[0] is T;

    public void Update(dynamic context = null, params object[] args)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var artifactGenerator = scope.ServiceProvider
                .GetRequiredService<IArtifactGenerator>();
            Update(artifactGenerator, args[0] as T, context);
        }
    }

    public abstract void Update(IArtifactGenerator artifactGenerator, T model, dynamic context = null);
    public virtual int Priority => 0;
}

public abstract class ArtifactGenerationStrategyBase<T> : IArtifactGenerationStrategy
    where T : class
{
    protected readonly IServiceProvider _serviceProvider;

    public ArtifactGenerationStrategyBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public virtual bool CanHandle(object model, dynamic context = null) => model is T;

    public virtual void Create(object model, dynamic context = null)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var artifactGenerator = scope.ServiceProvider
                .GetRequiredService<IArtifactGenerator>();
            Create(artifactGenerator, model as T, context);
        }
    }

    public abstract void Create(IArtifactGenerator artifactGenerator, T model, dynamic context = null);
    public virtual int Priority => 0;
}
