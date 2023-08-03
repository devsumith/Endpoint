// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Classes.Strategies;

public class ClassSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<ClassModel>
{
    private readonly ILogger<ClassSyntaxGenerationStrategy> _logger;
    public ClassSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<ClassSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override bool CanHandle(object model, dynamic context = null)
    {
        return model as ClassModel != null;
    }

    public override async Task<string> CreateAsync(ISyntaxGenerator syntaxGenerator, ClassModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        if (model.UsingAsDirectives.Count > 0)
        {
            foreach (var directive in model.UsingAsDirectives)
            {
                builder.AppendLine($"using {directive.Alias} = {directive.Name};");
            }

            builder.AppendLine();
        }

        foreach (var attribute in model.Attributes)
        {
            builder.AppendLine(await syntaxGenerator.CreateAsync(attribute));
        }

        builder.Append(await syntaxGenerator.CreateAsync(model.AccessModifier));

        if (model.Static)
            builder.Append(" static");

        builder.Append($" class {model.Name}");

        if (model.Implements.Count > 0)
        {
            builder.Append(": ");

            builder.Append(string.Join(',', model.Implements.Select(async x => await syntaxGenerator.CreateAsync(x, context))));
        }

        if (model.Properties.Count + model.Methods.Count + model.Constructors.Count + model.Fields.Count == 0)
        {
            builder.Append(" { }");

            return builder.ToString();
        }

        builder.AppendLine($"");

        builder.AppendLine("{");


        if (model.Fields.Count > 0)
            builder.AppendLine(((string)await syntaxGenerator.CreateAsync(model.Fields, context)).Indent(1));

        if (model.Constructors.Count > 0)
            builder.AppendLine(((string)await syntaxGenerator.CreateAsync(model.Constructors, context)).Indent(1));

        if (model.Properties.Count > 0)
            builder.AppendLine(((string)await syntaxGenerator.CreateAsync(model.Properties, context)).Indent(1));

        if (model.Methods.Count > 0)
            builder.AppendLine(((string)await syntaxGenerator.CreateAsync(model.Methods, context)).Indent(1));

        builder.AppendLine("}");

        return builder.ToString();
    }
}

