// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Methods.Strategies;

public class GetByIdQueryHandlerMethodGenerationStrategy : MethodSyntaxGenerationStrategy
{
    private readonly INamingConventionConverter _namingConventionConverter;

    public GetByIdQueryHandlerMethodGenerationStrategy(
        IServiceProvider serviceProvider,
        INamingConventionConverter namingConventionConverter,
        ILogger<MethodSyntaxGenerationStrategy> logger)
        : base(serviceProvider, logger)
    {
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public bool CanHandle(object model, dynamic context = null)
    {
        if (model is MethodModel methodModel && context?.Entity is ClassModel entity)
        {
            return methodModel.Name == "Handle" && methodModel.Params.FirstOrDefault().Type.Name.StartsWith($"Get{entity.Name}ByIdRequest");
        }

        return false;
    }

    public int Priority => int.MaxValue;

    public async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, MethodModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        var entityName = context.Entity.Name;

        var entityNamePascalCasePlural = _namingConventionConverter.Convert(NamingConvention.PascalCase, entityName, pluralize: true);

        builder.AppendLine("return new () {");

        builder.AppendLine($"{entityName} = (await _context.{entityNamePascalCasePlural}.AsNoTracking().SingleOrDefaultAsync(x => x.{entityName}Id == request.{entityName}Id)).ToDto()".Indent(1));

        builder.AppendLine("};");

        model.Body = builder.ToString();

        return await base.GenerateAsync(syntaxGenerator, model);
    }
}

