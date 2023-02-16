// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Fields;


public class FieldsSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<List<FieldModel>>
{
    private readonly ILogger<FieldsSyntaxGenerationStrategy> _logger;
    public FieldsSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<FieldsSyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, List<FieldModel> model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        foreach(var field in model)
        {
            builder.AppendLine(Create(syntaxGenerationStrategyFactory, field, context));

            if(field != model.Last())
                builder.AppendLine();
        }

        return builder.ToString();
    }

    private string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, FieldModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append(syntaxGenerationStrategyFactory.CreateFor(model.AccessModifier));

        if (model.ReadOnly)
            builder.Append(" readonly");

        

        if(!string.IsNullOrEmpty(model.DefaultValue))
        {
            builder.Append($" {syntaxGenerationStrategyFactory.CreateFor(model.Type)} {model.Name} = {model.DefaultValue};");
        }
        else
        {
            builder.Append($" {syntaxGenerationStrategyFactory.CreateFor(model.Type)} {model.Name};");
        }
        return builder.ToString();
    }
}
