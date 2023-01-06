using Endpoint.Core.Models.WebArtifacts;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.Abstractions;

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
