// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Text;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Microsoft.VisualStudio.OLE.Interop;

namespace Endpoint.Core.Syntax.Methods.Strategies;

public class UpdateCommandHandlerMethodGenerationStrategy : GenericSyntaxGenerationStrategy<MethodModel>
{
    private readonly INamingConventionConverter namingConventionConverter;

    public UpdateCommandHandlerMethodGenerationStrategy(
        INamingConventionConverter namingConventionConverter)
    {
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, object target)
    {
        /*        if (target is MethodModel)
                {
                    return await GenerateAsync(generator, target as MethodModel);
                }*/

        return null;
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, MethodModel model)
    {
        var entity = new ClassModel();

        var entityName = entity.Name;

        var entityNamePascalCasePlural = namingConventionConverter.Convert(NamingConvention.PascalCase, entityName, pluralize: true);

        var entityNameCamelCase = namingConventionConverter.Convert(NamingConvention.CamelCase, entityName);

        var builder = new StringBuilder();

        builder.AppendLine($"var {entityNameCamelCase} = await _context.{entityNamePascalCasePlural}.SingleAsync(x => x.{entityName}Id == request.{entityName}Id);");

        builder.AppendLine(string.Empty);

        foreach (var property in entity.Properties.Where(x => x.Id == false && x.Name != $"{model.Name}Id"))
        {
            builder.AppendLine($"{entityNameCamelCase}.{property.Name} = request.{property.Name};");
        }

        builder.AppendLine(string.Empty);

        builder.AppendLine("await _context.SaveChangesAsync(cancellationToken);");

        builder.AppendLine(string.Empty);

        builder.AppendLine("return new ()");

        builder.AppendLine("{");

        builder.AppendLine($"{entityName} = {entityNameCamelCase}.ToDto()".Indent(1));

        builder.AppendLine("};");

        model.Body = new Syntax.Expressions.ExpressionModel(builder.ToString());

        return await syntaxGenerator.GenerateAsync(model);
    }
}
