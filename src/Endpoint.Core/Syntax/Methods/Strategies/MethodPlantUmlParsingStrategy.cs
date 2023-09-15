// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Params;
using Endpoint.Core.Syntax.Types;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Syntax.Methods.Strategies;

public class MethodPlantUmlParsingStrategy : BaseSyntaxParsingStrategy<MethodModel>
{
    private readonly ILogger<MethodPlantUmlParsingStrategy> _logger;
    private readonly IContext _context;

    public MethodPlantUmlParsingStrategy(IContext context, ILogger<MethodPlantUmlParsingStrategy> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<MethodModel> ParseAsync(ISyntaxParser parser, string value)
    {
        _logger.LogInformation("Parsing for method");

        var returnType = new TypeModel(value.Replace("+", string.Empty).Split(' ').First());
        var name = value.Replace("+", string.Empty).Split(' ').ElementAt(1).Split('(').First();

        var @params = new List<ParamModel>();
        var rawParams = value.Split('(').ElementAt(1).Replace(")", string.Empty).Split(',');

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

        var isClassModel = _context.Get<TypeDeclarationModel>() is ClassModel;

        return new MethodModel()
        {
            AccessModifier = AccessModifier.Public,
            Interface = !isClassModel,
            ReturnType = returnType,
            Name = name,
            Params = @params,
            Async = returnType.Name.StartsWith("Task"),
            Body = new ("throw new NotImplementedException();")
        };
    }

}
