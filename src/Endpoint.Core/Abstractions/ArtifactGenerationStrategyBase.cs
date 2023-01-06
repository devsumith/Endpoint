using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.Abstractions;


public abstract class ArtifactUpdateStrategyBase<T> : IArtifactUpdateStrategy
    where T : class
{
    private readonly IServiceProvider _serviceProvider;

    public ArtifactUpdateStrategyBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public virtual bool CanHandle(dynamic configuration = null, params object[] args) => args[0] is T;

    public void Update(dynamic configuration = null, params object[] args)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var artifactGenerationStrategyFactory = scope.ServiceProvider
                .GetRequiredService<IArtifactGenerationStrategyFactory>();
            Update(artifactGenerationStrategyFactory, args[0] as T, configuration);
        }
    }

    public abstract void Update(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, T model, dynamic configuration = null);
    public virtual int Priority => 0;
}

public abstract class ArtifactGenerationStrategyBase<T> : IArtifactGenerationStrategy
    where T : class
{
    private readonly IServiceProvider _serviceProvider;

    public ArtifactGenerationStrategyBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public virtual bool CanHandle(object model, dynamic configuration = null) => model is T;

    public void Create(object model, dynamic configuration = null)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var artifactGenerationStrategyFactory = scope.ServiceProvider
                .GetRequiredService<IArtifactGenerationStrategyFactory>();
            Create(artifactGenerationStrategyFactory, model as T, configuration);
        }
    }

    public abstract void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, T model, dynamic configuration = null);
    public virtual int Priority => 0;
}