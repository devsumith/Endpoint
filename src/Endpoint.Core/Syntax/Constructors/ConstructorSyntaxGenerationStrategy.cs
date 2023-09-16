// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Constructors;

public class ConstructorSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<ConstructorModel>
{
    private readonly ILogger<ConstructorSyntaxGenerationStrategy> _logger;
    private readonly INamingConventionConverter _namingConventionConverter;

    public ConstructorSyntaxGenerationStrategy(

        INamingConventionConverter namingConventionConverter,
        ILogger<ConstructorSyntaxGenerationStrategy> logger)

    {
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }




    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, ConstructorModel model)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append(await syntaxGenerator.GenerateAsync(model.AccessModifier));

        builder.Append($" {model.Name}");

        builder.Append('(');

        builder.Append(string.Join(',', await Task.WhenAll(model.Params.Select(async x => await syntaxGenerator.GenerateAsync(x)))));

        builder.Append(')');

        if (model.BaseParams.Count > 0)
        {
            builder.AppendLine($": base({string.Join(',', model.BaseParams)})".Indent(1));
        }

        builder.AppendLine("{");


        foreach (var param in model.Params)
        {

            var field = model.Class.Fields.SingleOrDefault(x => x.Type.Name == param.Type.Name);

            if (field != null)
            {
                var fieldName = _namingConventionConverter.Convert(NamingConvention.CamelCase, field.Name);

                var paramName = _namingConventionConverter.Convert(NamingConvention.CamelCase, param.Name);

                builder.AppendLine($"{fieldName} = {paramName} ?? throw new ArgumentNullException(nameof({paramName}));".Indent(1));
            }
        }

        if (!string.IsNullOrEmpty(model.Body))
        {
            builder.AppendLine(model.Body.Indent(1));
        }

        builder.Append("}");

        return builder.ToString();
    }
}
