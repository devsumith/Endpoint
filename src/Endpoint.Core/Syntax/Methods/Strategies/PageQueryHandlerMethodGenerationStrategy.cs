// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Methods.Strategies;

public class PageQueryHandlerMethodGenerationStrategy : GenericSyntaxGenerationStrategy<MethodModel>
{
    private readonly INamingConventionConverter _namingConventionConverter;

    public PageQueryHandlerMethodGenerationStrategy(
        INamingConventionConverter namingConventionConverter)
    {
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public bool CanHandle(object model, dynamic context = null)
    {
        if (model is MethodModel methodModel && context?.Entity is ClassModel entity)
        {
            return methodModel.Name == "Handle" && methodModel.Params.FirstOrDefault().Type.Name.EndsWith($"PageRequest");
        }

        return false;
    }
    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, object target, dynamic context = null)
    {
        if (context != null && target is MethodModel)
        {
            return await GenerateAsync(generator, target as MethodModel, context);
        }

        return null;
    }


    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, MethodModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        var entityName = context.Entity.Name;

        builder.AppendLine($"var query = from {((SyntaxToken)entityName).CamelCase()} in _context.{((SyntaxToken)entityName).PascalCasePlural()}");

        builder.AppendLine($"select {((SyntaxToken)entityName).CamelCase()};".Indent(1));

        builder.AppendLine("");

        builder.AppendLine($"var length = await _context.{((SyntaxToken)entityName).PascalCasePlural()}.AsNoTracking().CountAsync();");

        builder.AppendLine("");

        builder.AppendLine($"var {((SyntaxToken)entityName).CamelCasePlural()} = await query.Page(request.Index, request.PageSize).AsNoTracking()");

        builder.AppendLine(".Select(x => x.ToDto()).ToListAsync();".Indent(1));

        builder.AppendLine("");

        builder.AppendLine("return new ()");

        builder.AppendLine("{");

        builder.AppendLine("Length = length,".Indent(1));

        builder.AppendLine($"Entities = {((SyntaxToken)entityName).CamelCasePlural()}".Indent(1));

        builder.AppendLine("};");

        model.Body = builder.ToString();

        return await syntaxGenerator.GenerateAsync(model);
    }
}
