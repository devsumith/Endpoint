// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Entities.Legacy;
using System.Collections.Generic;

namespace Endpoint.Core.Strategies.Application;

public class AggregateRootGenerationStrategy : SyntaxGenerationStrategyBase<LegacyAggregatesModel>, IAggregateRootGenerationStrategy
{
    public AggregateRootGenerationStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }

    public override string Create(ISyntaxGenerator syntaxGenerator, LegacyAggregatesModel model, dynamic context = null)
    {
        var content = new List<string>
        {
            $"public class {((SyntaxToken)model.Name).PascalCase}",

            "{"
        };

        foreach (var property in model.Properties)
        {
            content.Add(($"public {property.Type} {property.Name}" + " { get; set; }").Indent(1));
        }

        content.Add("}");

        return string.Join(Environment.NewLine, content);
    }
}

