using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using System.Linq;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class PlantUmlPropertyParserStrategy : PlantUmlParserStrategyBase<PropertyModel>
{
    public PlantUmlPropertyParserStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }

    public override bool CanHandle(string plantUml) => plantUml.StartsWith("-");

    protected override PropertyModel Create(IPlantUmlParserStrategyFactory factory, string plantUml, dynamic context = null)
    {
        var typeName = plantUml.Replace("-", string.Empty).Split(' ').First();

        var name = plantUml.Replace("-", string.Empty).Split(' ').ElementAt(1);

        var propertyModel = new PropertyModel(context.TypeDeclarationModel, AccessModifier.Public, new TypeModel(typeName), name, PropertyAccessorModel.GetSet);

        return propertyModel;    
    }
}
