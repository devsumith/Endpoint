using Endpoint.Core.Models.Syntax.Properties;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class PlantUmlFieldParserStrategy : PlantUmlParserStrategyBase<PropertyModel>
{
    public PlantUmlFieldParserStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }

    public override bool CanHandle(string plantUml) => plantUml.StartsWith("#");

    protected override PropertyModel Create(IPlantUmlParserStrategyFactory factory, string plantUml, dynamic context = null)
    {
        throw new NotImplementedException();
    }
}
