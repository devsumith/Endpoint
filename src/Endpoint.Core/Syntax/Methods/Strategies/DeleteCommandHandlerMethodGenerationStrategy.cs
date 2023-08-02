// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Methods.Strategies;

public class DeleteCommandHandlerMethodGenerationStrategy : MethodSyntaxGenerationStrategy
{
    private readonly INamingConventionConverter _namingConventionConverter;

    public DeleteCommandHandlerMethodGenerationStrategy(
        IServiceProvider serviceProvider,
        INamingConventionConverter namingConventionConverter,
        ILogger<MethodSyntaxGenerationStrategy> logger)
        : base(serviceProvider, logger)
    {
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public override bool CanHandle(object model, dynamic context = null)
    {
        if (model is MethodModel methodModel && context?.Entity is ClassModel entity)
        {
            return methodModel.Name == "Handle" && methodModel.Params.FirstOrDefault().Type.Name.StartsWith($"Delete{entity.Name}Request");
        }

        return false;
    }

    public override int Priority => int.MaxValue;

    public override string Create(ISyntaxGenerator syntaxGenerator, MethodModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        var entityName = context.Entity.Name;

        var entityNamePascalCasePlural = _namingConventionConverter.Convert(NamingConvention.PascalCase, entityName, pluralize: true);

        var entityNameCamelCase = _namingConventionConverter.Convert(NamingConvention.CamelCase, entityName); ;

        builder.AppendJoin(Environment.NewLine, new string[]
        {
            $"var {entityNameCamelCase} = await _context.{entityNamePascalCasePlural}.FindAsync(request.{entityName}Id);",
            "",
            $"_context.{entityNamePascalCasePlural}.Remove({entityNameCamelCase});",
            "",
            "await _context.SaveChangesAsync(cancellationToken);",
            "",
            "return new ()",
            "{",
            $"{entityName} = {entityNameCamelCase}.ToDto()".Indent(1),
            "};"

        });

        model.Body = builder.ToString();

        return base.Create(syntaxGenerator, model);
    }
}
