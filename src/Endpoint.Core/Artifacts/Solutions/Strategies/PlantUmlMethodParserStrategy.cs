// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Syntax.Params;
using Endpoint.Core.Syntax.Types;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Artifacts.Solutions.Strategies;

public class PlantUmlMethodParserStrategy : PlantUmlParserStrategyBase<MethodModel>
{
    private readonly ILogger<PlantUmlMethodParserStrategy> _logger;

    public PlantUmlMethodParserStrategy(ILogger<PlantUmlMethodParserStrategy> logger, IServiceProvider serviceProvider)
        : base(serviceProvider)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override bool CanHandle(string plantUml) => plantUml.StartsWith("+");

    protected override MethodModel Create(IPlantUmlParserStrategyFactory factory, string plantUml, dynamic context = null)
    {
        var returnType = new TypeModel(plantUml.Replace("+", string.Empty).Split(' ').First());
        var name = plantUml.Replace("+", string.Empty).Split(' ').ElementAt(1).Split('(').First();

        var @params = new List<ParamModel>();
        var rawParams = plantUml.Split('(').ElementAt(1).Replace(")", string.Empty).Split(',');

        foreach (var p in rawParams)
        {
            if (string.IsNullOrEmpty(p))
                break;

            var parts = p.Split(' ');
            var t = new TypeModel(parts[0]);
            var n = parts[1];

            @params.Add(new ParamModel
            {
                Type = t,
                Name = n
            });
        }

        var isClassModel = context.TypeDeclarationModel is ClassModel;

        return new MethodModel()
        {
            AccessModifier = AccessModifier.Public,
            IsInterface = !isClassModel,
            ReturnType = returnType,
            Name = name,
            Params = @params,
            Async = returnType.Name.StartsWith("Task"),
            Body = new Syntax.Expressions.ExpressionModel("throw new NotImplementedException();")
        };
    }
}

