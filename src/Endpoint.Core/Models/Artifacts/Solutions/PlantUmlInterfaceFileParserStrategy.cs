using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax.Interfaces;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class PlantUmlInterfaceFileParserStrategy : PlantUmlParserStrategyBase<ObjectFileModel<InterfaceModel>>
{
    public PlantUmlInterfaceFileParserStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }

    public override bool CanHandle(string plantUml) => plantUml.StartsWith("class");

    protected override ObjectFileModel<InterfaceModel> Create(IPlantUmlParserStrategyFactory factory, string plantUml, dynamic context = null)
    {
        throw new NotImplementedException();
    }
}
