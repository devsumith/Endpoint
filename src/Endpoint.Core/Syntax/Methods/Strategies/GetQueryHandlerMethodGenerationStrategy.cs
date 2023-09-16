// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Methods.Strategies;

public class GetQueryHandlerMethodGenerationStrategy : GenericSyntaxGenerationStrategy<MethodModel>
{
    private readonly INamingConventionConverter _namingConventionConverter;

    public GetQueryHandlerMethodGenerationStrategy(
        INamingConventionConverter namingConventionConverter)
    {
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, object target)
    {
        /*        if (target is MethodModel)
                {
                    return await GenerateAsync(generator, target as MethodModel);
                }*/

        return null;
    }
    public bool CanHandle(object model)
    {
        /*        if (model is MethodModel methodModel && context?.Entity is ClassModel entity)
                {
                    var entityNamePascalCasePlural = _namingConventionConverter.Convert(NamingConvention.PascalCase, entity.Name, pluralize: true);

                    return methodModel.Params.First().Type.Name == $"Get{entityNamePascalCasePlural}Request";
                }*/

        return false;
    }

    public int Priority => int.MaxValue;

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, MethodModel model)
    {
        var builder = new StringBuilder();

        var entityName = ""; // ""; // context.Entity.Name;

        var entityNamePascalCasePlural = _namingConventionConverter.Convert(NamingConvention.PascalCase, entityName, pluralize: true);

        builder.AppendLine("return new () {");

        builder.AppendLine($"{entityNamePascalCasePlural} = await _context.{entityNamePascalCasePlural}.AsNoTracking().ToDtosAsync(cancellationToken)".Indent(1));

        builder.AppendLine("};");

        model.Body = new Syntax.Expressions.ExpressionModel(builder.ToString());

        return await syntaxGenerator.GenerateAsync(model);
    }
}

