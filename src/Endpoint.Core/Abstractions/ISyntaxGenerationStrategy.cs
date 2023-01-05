using Microsoft.Extensions.DependencyInjection;
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