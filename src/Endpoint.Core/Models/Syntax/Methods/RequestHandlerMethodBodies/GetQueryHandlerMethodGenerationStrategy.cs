// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Methods.RequestHandlerMethodBodies;

public class GetQueryHandlerMethodGenerationStrategy : MethodSyntaxGenerationStrategy
{
    private readonly INamingConventionConverter _namingConventionConverter;
    public GetQueryHandlerMethodGenerationStrategy(
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
            var entityNamePascalCasePlural = _namingConventionConverter.Convert(NamingConvention.PascalCase, entity.Name, pluralize: true);

            return methodModel.Params.First().Type.Name == $"Get{entityNamePascalCasePlural}Request";
        }

        return false;
    }

    public override int Priority => int.MaxValue;

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, MethodModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        var entityName = context.Entity.Name;

        var entityNamePascalCasePlural = _namingConventionConverter.Convert(NamingConvention.PascalCase, entityName, pluralize: true);

        builder.AppendLine("return new () {");

        builder.AppendLine($"{entityNamePascalCasePlural} = await _context.{entityNamePascalCasePlural}.AsNoTracking().ToDtosAsync(cancellationToken)".Indent(1));

        builder.AppendLine("};");

        model.Body = builder.ToString();

        return base.Create(syntaxGenerationStrategyFactory, model);
    }
}

