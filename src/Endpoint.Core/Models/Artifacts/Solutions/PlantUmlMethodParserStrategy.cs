using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Types;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class PlantUmlMethodParserStrategy : PlantUmlParserStrategyBase<MethodModel>
{
    public PlantUmlMethodParserStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }

    public override bool CanHandle(string plantUml) => plantUml.StartsWith("+");

    protected override MethodModel Create(IPlantUmlParserStrategyFactory factory, string plantUml, dynamic context = null)
    {
        var returnType = new TypeModel(plantUml.Replace("+", string.Empty).Split(' ').First());
        var name = plantUml.Replace("+", string.Empty).Split(' ').ElementAt(1).Split('(').First();

        var @params = new List<ParamModel>();
        foreach(var p in plantUml.Split('(').ElementAt(1).Replace(")", string.Empty).Split(','))
        {
            var parts = p.Split(' ');
            var t = new TypeModel(parts[0]);
            var n = parts[1];

            @params.Add(new ParamModel
            {
                Type = t,
                Name = n
            });
        }

        return new MethodModel()
        {
            Interface = context.TypeDeclarationModel is InterfaceModel,
            ReturnType = returnType,
            Name = name,
            Params = @params,
            Async = returnType.Name.StartsWith("Task")
        };
    }
}
