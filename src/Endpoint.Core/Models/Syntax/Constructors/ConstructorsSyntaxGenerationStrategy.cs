// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Constructors;

public class ConstructorsSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<List<ConstructorModel>>
{
    private readonly ILogger<ConstructorsSyntaxGenerationStrategy> _logger;
    public ConstructorsSyntaxGenerationStrategy(
        ILogger<ConstructorsSyntaxGenerationStrategy> logger,
        IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, List<ConstructorModel> model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        foreach(var ctor in model)
        {
            builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(ctor));

            if(ctor != model.Last())
            {
                builder.AppendLine();
            }
        }

        return builder.ToString();
    }
}

