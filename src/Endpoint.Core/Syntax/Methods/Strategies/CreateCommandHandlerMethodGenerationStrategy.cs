// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Methods.Strategies;


public class CreateCommandHandlerMethodGenerationStrategy : GenericSyntaxGenerationStrategy<MethodModel>
{
    private readonly INamingConventionConverter _namingConventionConverter;

    public CreateCommandHandlerMethodGenerationStrategy(INamingConventionConverter namingConventionConverter)
    {
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, object target)
    {
        if (target is MethodModel)
        {
            return await GenerateAsync(generator, target as MethodModel);
        }

        return null;
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, MethodModel model)
    {
        var builder = new StringBuilder();

        var entity = new ClassModel();

        var entityName = entity.Name;

        var entityNameCamelCase = _namingConventionConverter.Convert(NamingConvention.CamelCase, entityName);

        builder.AppendLine($"var {entityNameCamelCase} = new {((SyntaxToken)entityName).PascalCase()}();");

        builder.AppendLine("");

        builder.AppendLine($"_context.{((SyntaxToken)entityName).PascalCasePlural()}.Add({entityNameCamelCase});");

        builder.AppendLine("");

        foreach (var property in entity.Properties.Where(x => x.Name != $"{entityName}Id"))
        {
            builder.AppendLine($"{entityNameCamelCase}.{property.Name} = request.{property.Name};");
        }

        builder.AppendLine("");

        builder.AppendLine("await _context.SaveChangesAsync(cancellationToken);");

        builder.AppendLine("");

        builder.AppendLine("return new ()");

        builder.AppendLine("{");

        builder.AppendLine($"{((SyntaxToken)entityName).PascalCase()} = {entityNameCamelCase}.ToDto()".Indent(1));

        builder.AppendLine("};");

        model.Body = new Syntax.Expressions.ExpressionModel(builder.ToString());

        return await syntaxGenerator.GenerateAsync(model);
    }
}

