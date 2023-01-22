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

    public virtual bool CanHandle(LitWorkspaceModel model, dynamic context = null) => model is T;

    public void Create(LitWorkspaceModel model, dynamic context = null)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var webGenerationStrategyFactory = scope.ServiceProvider
                .GetRequiredService<IWebGenerationStrategyFactory>();
            Create(webGenerationStrategyFactory, model as T, context);
        }
    }

    public abstract void Create(IWebGenerationStrategyFactory webGenerationStrategyFactory, T model, dynamic context = null);
    public virtual int Priority => 0;
}
