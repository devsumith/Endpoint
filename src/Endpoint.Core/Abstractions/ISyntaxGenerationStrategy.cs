using Endpoint.Core.Models.WebArtifacts;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Abstractions;

public interface ISyntaxGenerationStrategy
{
    bool CanHandle(object model, dynamic configuration = null);
    string Create(object model, dynamic configuration = null);
    int Priority { get; }
}

public interface ISyntaxGenerationStrategyFactory
{
    string CreateFor(object model, dynamic configuration = null);
}

public abstract class SyntaxGenerationStrategyBase<T>: ISyntaxGenerationStrategy
    where T : class
{
    private readonly IServiceProvider _serviceProvider;
    public SyntaxGenerationStrategyBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public virtual bool CanHandle(object model, dynamic configuration = null) => model is T;
    public string Create(object model, dynamic configuration = null)
    {
        using(IServiceScope scope = _serviceProvider.CreateScope())
        {
            var syntaxGenerationStrategyFactory = scope.ServiceProvider
                .GetRequiredService<ISyntaxGenerationStrategyFactory>();
            return Create(syntaxGenerationStrategyFactory, model as T, configuration);
        }        
    }

    public abstract string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, T model, dynamic configuration = null);
    public virtual int Priority => 0;
}

public interface IArtifactGenerationStrategy
{
    bool CanHandle(object model, dynamic configuration = null);
    void Create(object model, dynamic configuration = null);
    int Priority { get; }
}

public interface IArtifactGenerationStrategyFactory
{
    void CreateFor(object model, dynamic configuration = null);
}



public interface IWebGenerationStrategy
{
    bool CanHandle(WebModel model, dynamic configuration = null);
    void Create(WebModel model, dynamic configuration = null);
    int Priority { get; }
}

public interface IWebGenerationStrategyFactory
{
    void CreateFor(WebModel model, dynamic configuration = null);
}

public class ArtifactGenerationStrategyFactory : IArtifactGenerationStrategyFactory
{
    private readonly IEnumerable<IArtifactGenerationStrategy> _strategies;
    public ArtifactGenerationStrategyFactory(IEnumerable<IArtifactGenerationStrategy> strategies)
    {
        _strategies = strategies;
    }
    public void CreateFor(object model, dynamic configuration = null)
    {
        var strategy = _strategies.Where(x => x.CanHandle(model, configuration))
            .OrderBy(x => x.Priority)
            .FirstOrDefault();

        strategy.Create(model);
    }
}


public class WebGenerationStrategyFactory : IWebGenerationStrategyFactory
{
    private readonly IEnumerable<IWebGenerationStrategy> _strategies;
    public WebGenerationStrategyFactory(IEnumerable<IWebGenerationStrategy> strategies)
    {
        _strategies = strategies;
    }
    public void CreateFor(WebModel model, dynamic configuration = null)
    {
        var strategy = _strategies.Where(x => x.CanHandle(model, configuration))
            .OrderBy(x => x.Priority)
            .FirstOrDefault();

        strategy.Create(model);
    }
}



public abstract class WebGenerationStrategyBase<T>: IWebGenerationStrategy
    where T : class
{
    private readonly IServiceProvider _serviceProvider;

    public WebGenerationStrategyBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public virtual bool CanHandle(WebModel model, dynamic configuration = null) => model is T;

    public void Create(WebModel model, dynamic configuration = null)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var webGenerationStrategyFactory = scope.ServiceProvider
                .GetRequiredService<IWebGenerationStrategyFactory>();
            Create(webGenerationStrategyFactory, model as T, configuration);
        }
    }

    public abstract void Create(IWebGenerationStrategyFactory webGenerationStrategyFactory, T model, dynamic configuration = null);
    public virtual int Priority => 0;
}


public abstract class ArtifactGenerationStrategyStrategyBase<T> : IArtifactGenerationStrategy
    where T : class
{
    private readonly IServiceProvider _serviceProvider;

    public ArtifactGenerationStrategyStrategyBase(IServiceProvider serviceProvider)
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