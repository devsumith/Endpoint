using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public abstract class PlantUmlParserStrategyBase<T>: IPlantUmlParserStrategy
    where T : class
{
    protected readonly IServiceProvider _serviceProvider;
    public PlantUmlParserStrategyBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public abstract bool CanHandle(string plantUml);

    public virtual int Priority { get; } = 0;

    public object Create(string plantUml, dynamic context = null)
    {
        using(var scope = _serviceProvider.CreateScope())
        {
            var plantUmlParserStrategy = scope.ServiceProvider.GetRequiredService<IPlantUmlParserStrategyFactory>();

            return Create(plantUmlParserStrategy, plantUml, context);
        }
    }

    protected abstract T Create(IPlantUmlParserStrategyFactory factory, string plantUml, dynamic context = null);
}
